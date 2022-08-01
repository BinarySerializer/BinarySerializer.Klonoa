namespace BinarySerializer.Klonoa.LV
{
    public class LevelCommonAssets_ArchiveFile : ArchiveFile
    {
        public LevelSectors_ArchiveFile SectorData;
        public LevelModels_ArchiveFile ModelData;
        public RawData_ArchiveFile Archive_2; // Contains video files (PS2 .ipu) for things like water, as well as some other files with float values

        protected override void SerializeFiles(SerializerObject s)
        {
            SectorData = SerializeFile(s, SectorData, 0, name: nameof(SectorData));
            ModelData = SerializeFile(s, ModelData, 1, name: nameof(ModelData));
            Archive_2 = SerializeFile(s, Archive_2, 2, name: nameof(Archive_2));
        }
    }
}