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
            public short[] Data { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Data = s.SerializeArray<short>(Data, Pre_FileSize / 2, name: nameof(Data));
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