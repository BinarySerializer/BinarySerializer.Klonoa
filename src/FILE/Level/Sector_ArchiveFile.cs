using BinarySerializer.PS1;

namespace BinarySerializer.KlonoaDTP
{
    /// <summary>
    /// A sector of a level
    /// </summary>
    public class Sector_ArchiveFile : ArchiveFile
    {
        public PS1_TMD LevelModel { get; set; }
        public File_1_Data File_1 { get; set; } // TODO: Parse - removing this in-game causes level model to not render
        public LevelCollision_File LevelCollision { get; set; }
        public LevelCollisionItems_File LevelCollisionItems { get; set; }
        public ArchiveFile<MovementPath_File> MovementPaths { get; set; }
        public File_5_Data File_5 { get; set; } // TODO: Parse - setting some -1 values to 0 completely breaks the vram

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
            LevelModel = SerializeFile<PS1_TMD>(s, LevelModel, 0, logIfNotFullyParsed: false, name: nameof(LevelModel));
            File_1 = SerializeFile<File_1_Data>(s, File_1, 1, name: nameof(File_1));
            LevelCollision = SerializeFile<LevelCollision_File>(s, LevelCollision, 2, name: nameof(LevelCollision));
            LevelCollisionItems = SerializeFile<LevelCollisionItems_File>(s, LevelCollisionItems, 3, name: nameof(LevelCollisionItems));
            MovementPaths = SerializeFile<ArchiveFile<MovementPath_File>>(s, MovementPaths, 4, name: nameof(MovementPaths));
            File_5 = SerializeFile<File_5_Data>(s, File_5, 5, onPreSerialize: x => x.Pre_ObjsCount = LevelModel.ObjectsCount, name: nameof(File_5));
        }

        // TODO: Parse and move to separate file
        public class File_1_Data : BaseFile
        {
            public ushort Ushort_00 { get; set; }
            public ushort Ushort_02 { get; set; }
            public ushort Ushort_04 { get; set; }
            public ushort Ushort_06 { get; set; }
            public short Short_08 { get; set; }
            public short Short_0A { get; set; }
            public short Short_0C { get; set; }
            public short Short_0E { get; set; }

            public short[] ObjIndices { get; set; } // Indices to level model objects

            public override void SerializeImpl(SerializerObject s)
            {
                Ushort_00 = s.Serialize<ushort>(Ushort_00, name: nameof(Ushort_00));
                Ushort_02 = s.Serialize<ushort>(Ushort_02, name: nameof(Ushort_02));
                Ushort_04 = s.Serialize<ushort>(Ushort_04, name: nameof(Ushort_04));
                Ushort_06 = s.Serialize<ushort>(Ushort_06, name: nameof(Ushort_06));
                Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
                Short_0A = s.Serialize<short>(Short_0A, name: nameof(Short_0A));
                Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
                Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));

                ObjIndices = s.SerializeArray<short>(ObjIndices, Ushort_00 * Ushort_02 * Ushort_04, name: nameof(ObjIndices));
            }
        }

        // TODO: Parse and move to separate file
        public class File_5_Data : BaseFile
        {
            public uint Pre_ObjsCount { get; set; }

            public Entry[] Entries { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Entries = s.SerializeObjectArray<Entry>(Entries, Pre_ObjsCount, name: nameof(Entries));
            }

            public class Entry : BinarySerializable
            {
                public int[] Data { get; set; }

                public override void SerializeImpl(SerializerObject s)
                {
                    Data = s.SerializeArray<int>(Data, 8, name: nameof(Data));
                }
            }
        }
    }
}