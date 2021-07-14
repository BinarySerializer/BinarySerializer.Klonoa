namespace BinarySerializer.KlonoaDTP
{
    /// <summary>
    /// A sector of a level
    /// </summary>
    public class Sector_ArchiveFile : BaseArchiveFile
    {
        // TODO: Parse these
        public RawData_File LevelModel { get; set; } // TMD file
        public RawData_File File_1 { get; set; }
        public RawData_File File_2 { get; set; }
        public RawData_File File_3 { get; set; } // 28-byte entries - objects?
        public ArchiveFile<MovementPath_File> MovementPaths { get; set; }
        public RawData_File File_5 { get; set; } // Array of int32?

        public override void SerializeImpl(SerializerObject s)
        {
            // Every third level (every boss) is not compressed
            var isCompressed = !Loader.GetLoader(s.Context).IsBossFight;

            // TODO: Decompress sector archive
            if (isCompressed)
            {
                s.LogWarning($"TODO: Decompress sector archive");
                return;
            }

            base.SerializeImpl(s);
        }

        protected override void SerializeFiles(SerializerObject s)
        {
            LevelModel = SerializeFile<RawData_File>(s, LevelModel, 0, name: nameof(LevelModel));
            File_1 = SerializeFile<RawData_File>(s, File_1, 1, name: nameof(File_1));
            File_2 = SerializeFile<RawData_File>(s, File_2, 2, name: nameof(File_2));
            File_3 = SerializeFile<RawData_File>(s, File_3, 3, name: nameof(File_3));
            MovementPaths = SerializeFile<ArchiveFile<MovementPath_File>>(s, MovementPaths, 4, name: nameof(MovementPaths));
            File_5 = SerializeFile<RawData_File>(s, File_5, 5, name: nameof(File_5));
        }
    }
}