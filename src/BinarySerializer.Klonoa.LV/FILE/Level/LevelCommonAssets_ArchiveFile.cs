namespace BinarySerializer.Klonoa.LV
{
    public class LevelCommonAssets_ArchiveFile : ArchiveFile
    {
        public int Pre_Level { get; set; }

        public LevelSectors_ArchiveFile SectorData;
        public LevelModels_ArchiveFile ModelData;
        public VTPack_ArchiveFile VTData;

        protected override void SerializeFiles(SerializerObject s)
        {
            SectorData = SerializeFile(s, SectorData, 0, name: nameof(SectorData));
            ModelData = SerializeFile(s, ModelData, 1, name: nameof(ModelData));
            VTData = SerializeFile(s, VTData, 2,
                onPreSerialize: x => {
                    x.Pre_Level = Pre_Level;
                    x.Pre_SectorCount = SectorData.Sectors.Length;
                },
                name: nameof(VTData));
        }
    }
}