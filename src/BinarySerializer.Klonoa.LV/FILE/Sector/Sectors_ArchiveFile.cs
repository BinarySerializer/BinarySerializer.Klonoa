namespace BinarySerializer.Klonoa.LV
{
    public class LevelSectors_ArchiveFile : ArchiveFile
    {
        public RCN_File RouteConnections { get; set; }
        public LevelSector_ArchiveFile[] Sectors { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            RouteConnections = SerializeFile(s, RouteConnections, 0, name: nameof(RouteConnections));
            Sectors = new LevelSector_ArchiveFile[OffsetTable.FilesCount - 1];
            for (int i = 1; i < OffsetTable.FilesCount; i++)
            {
                Sectors[i - 1] = SerializeFile(s, Sectors[i - 1], i, name: $"{nameof(Sectors)}[{i}]");
            }
        }
    }
}