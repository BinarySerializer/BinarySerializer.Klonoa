namespace BinarySerializer.Klonoa.LV
{
    public class LevelDataPack_ArchiveFile : ArchiveFile
    {
        public int Pre_Level { get; set; } = -1; // Semi-hacky way to pass level down for parsing hard-coded files

        public LevelCommonAssets_ArchiveFile CommonAssets { get; set; } 
        public RawData_ArchiveFile Archive_1 { get; set; } // Contains geometry and textures for dream stones, not sure about the other files
        public ArchiveFile<RawData_ArchiveFile> Archive_2 { get; set; } // ? (seems like the number of archives inside this archive is equal to the number of level sectors)
        public LevelMiscAssets_ArchiveFile MiscAssets { get; set; }
        public RawData_ArchiveFile File_4 { get; set; } // ?
        public RawData_File Boundary_0 { get; set; } 
        public RawData_File Boundary_1 { get; set; }
        public RawData_File Boundary_2 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            CommonAssets = SerializeFile(s, CommonAssets, 0, onPreSerialize: x => x.Pre_Level = Pre_Level, name: nameof(CommonAssets));
            Archive_1 = SerializeFile(s, Archive_1, 1, name: nameof(Archive_1));
            Archive_2 = SerializeFile(s, Archive_2, 2, name: nameof(Archive_2));
            MiscAssets = SerializeFile(s, MiscAssets, 3, name: nameof(MiscAssets));
            File_4 = SerializeFile(s, File_4, 4, name: nameof(File_4));
            Boundary_0 = SerializeFile(s, Boundary_0, 5, name: nameof(Boundary_0));
            Boundary_1 = SerializeFile(s, Boundary_1, 6, name: nameof(Boundary_1));
            Boundary_2 = SerializeFile(s, Boundary_2, 7, name: nameof(Boundary_2));
        }
    }
}