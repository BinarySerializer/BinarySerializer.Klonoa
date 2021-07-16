using BinarySerializer.PS1;

namespace BinarySerializer.KlonoaDTP
{
    /// <summary>
    /// A sector of a level
    /// </summary>
    public class Sector_ArchiveFile : BaseArchiveFile
    {
        public PS1_TMD LevelModel { get; set; }
        // TODO: Parse these
        public File_1_Data File_1 { get; set; }
        public File_2_Data File_2 { get; set; }
        public File_3_Data File_3 { get; set; }
        public ArchiveFile<MovementPath_File> MovementPaths { get; set; }
        public RawData_File File_5 { get; set; } // Array of int32?

        public override void SerializeImpl(SerializerObject s)
        {
            // Every third level (every boss) is not compressed
            var isCompressed = !Loader.GetLoader(s.Context).IsBossFight;

            if (isCompressed)
            {
                var compresedSize = Pre_FileSize;

                s.DoEncoded(new LevelSectorEncoder(), () =>
                {
                    Pre_FileSize = s.CurrentLength;

                    var start = s.CurrentPointer;

                    // Serialize the offset table
                    OffsetTable = s.SerializeObject<OffsetTable>(OffsetTable, name: nameof(OffsetTable));

                    // Serialize the files
                    SerializeFiles(s);

                    s.Goto(start + s.CurrentLength);
                }, allowLocalPointers: true);

                // Go to the end of the archive
                s.Goto(Offset + compresedSize);

                return;
            }

            base.SerializeImpl(s);
        }

        protected override void SerializeFiles(SerializerObject s)
        {
            LevelModel = SerializeFile<PS1_TMD>(s, LevelModel, 0, name: nameof(LevelModel));
            File_1 = SerializeFile<File_1_Data>(s, File_1, 1, name: nameof(File_1));
            File_2 = SerializeFile<File_2_Data>(s, File_2, 2, name: nameof(File_2));
            File_3 = SerializeFile<File_3_Data>(s, File_3, 3, name: nameof(File_3));
            MovementPaths = SerializeFile<ArchiveFile<MovementPath_File>>(s, MovementPaths, 4, name: nameof(MovementPaths));
            File_5 = SerializeFile<RawData_File>(s, File_5, 5, name: nameof(File_5));
        }

        // TODO: Parse and move to separate files 
        public class File_1_Data : BaseFile
        {
            public short[] Data { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Data = s.SerializeArray<short>(Data, Pre_FileSize / 2, name: nameof(Data));
            }
        }
        public class File_2_Data : BaseFile
        {
            public ushort Data1Count { get; set; }
            public ushort Data1ItemLength { get; set; }
            public ushort Ushort_04 { get; set; }

            public short Short_08 { get; set; }
            public short Short_0A { get; set; }
            public short Short_0C { get; set; }

            public ushort Offset1 { get; set; }
            public ushort Offset2 { get; set; }
            public ushort Offset3 { get; set; }

            public ushort[][] Data1 { get; set; } // Offsets to Offset3

            public override void SerializeImpl(SerializerObject s)
            {
                Data1Count = s.Serialize<ushort>(Data1Count, name: nameof(Data1Count));
                Data1ItemLength = s.Serialize<ushort>(Data1ItemLength, name: nameof(Data1ItemLength));
                Ushort_04 = s.Serialize<ushort>(Ushort_04, name: nameof(Ushort_04));
                s.SerializePadding(2, logIfNotNull: true);
                Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
                Short_0A = s.Serialize<short>(Short_0A, name: nameof(Short_0A));
                Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
                s.SerializePadding(2, logIfNotNull: true);
                Offset1 = s.Serialize<ushort>(Offset1, name: nameof(Offset1));
                Offset2 = s.Serialize<ushort>(Offset2, name: nameof(Offset2));
                Offset3 = s.Serialize<ushort>(Offset3, name: nameof(Offset3));

                s.DoAt(Offset + Offset1 * 2, () =>
                {
                    Data1 ??= new ushort[Data1Count][];

                    for (int i = 0; i < Data1.Length; i++)
                        Data1[i] = s.SerializeArray<ushort>(Data1[i], Data1ItemLength, name: $"{nameof(Data1)}[{i}]");
                });
            }
        }
        public class File_3_Data : BaseFile
        {
            public Entry[] Entries { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Entries = s.SerializeObjectArray<Entry>(Entries, Pre_FileSize / 28, name: nameof(Entries));
            }

            public class Entry : BinarySerializable
            {
                public short[] Data { get; set; }

                public override void SerializeImpl(SerializerObject s)
                {
                    Data = s.SerializeArray<short>(Data, 28 / 2, name: nameof(Data));
                }
            }
        }
    }
}