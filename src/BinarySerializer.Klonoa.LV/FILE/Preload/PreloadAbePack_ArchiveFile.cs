namespace BinarySerializer.Klonoa.LV
{
    public class PreloadAbePack_ArchiveFile : ArchiveFile
    {
        public GIMPack_ArchiveFile[] GIMPacks { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            GIMPacks ??= new GIMPack_ArchiveFile[OffsetTable.FilesCount];
            for (int i = 0; i < GIMPacks.Length; i++) {
                GIMPacks[i] = SerializeFile(s, GIMPacks[i], i, name: $"{nameof(GIMPacks)}[{i}]");
            }
        }
    }
}