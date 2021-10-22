namespace BinarySerializer.Klonoa.KH
{
    public class StoryPack_File0_ArchiveFile : ArchiveFile
    {
        // Only the first graphics file in each group has a palette defined
        public Graphics_File[][] Files { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Files ??= new Graphics_File[OffsetTable.FilesCount / 3][];

            for (int i = 0; i < Files.Length; i++)
            {
                Files[i] ??= new Graphics_File[3];

                for (int j = 0; j < Files[i].Length; j++)
                {
                    Files[i][j] = SerializeFile<Graphics_File>(s, Files[i][j], i * 3 + j, fileEncoder: new BytePairEncoder(), name: $"{nameof(Files)}[{i}][{j}]");
                }
            }
        }
    }
}