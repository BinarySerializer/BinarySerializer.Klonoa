namespace BinarySerializer.Klonoa.LV
{
    public class LevelSectors_ArchiveFile : ArchiveFile
    {
        public RawData_File Descriptor { get; set; } // Not sure what this file is for, but just assume it's some sort of descriptor given its position in the archive
        public LevelSector_ArchiveFile[] Sectors { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Descriptor = SerializeFile(s, Descriptor, 0, name: nameof(Descriptor));
            Sectors = new LevelSector_ArchiveFile[OffsetTable.FilesCount - 1];
            for (int i = 1; i < OffsetTable.FilesCount; i++)
            {
                Sectors[i - 1] = SerializeFile(s, Sectors[i - 1], i, name: $"{nameof(Sectors)}[{i}]");
            }
        }
    }
}