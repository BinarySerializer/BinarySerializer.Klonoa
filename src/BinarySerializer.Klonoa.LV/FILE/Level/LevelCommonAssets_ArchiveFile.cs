namespace BinarySerializer.Klonoa.LV
{
    public class LevelCommonAssets_ArchiveFile : ArchiveFile
    {
        public LevelSectors_ArchiveFile LevelSectors;
        public LevelModels_ArchiveFile LevelModels;
        public RawData_ArchiveFile Archive_2; // Contains video files (PS2 .ipu) for things like water, as well as some other files with float values

        protected override void SerializeFiles(SerializerObject s)
        {
            LevelSectors = SerializeFile(s, LevelSectors, 0, name: nameof(LevelSectors));
            LevelModels = SerializeFile(s, LevelModels, 1, name: nameof(LevelModels));
            Archive_2 = SerializeFile(s, Archive_2, 2, name: nameof(Archive_2));
        }
    }
}