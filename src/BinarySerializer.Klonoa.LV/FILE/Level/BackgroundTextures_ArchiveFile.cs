namespace BinarySerializer.Klonoa.LV {
    public class BackgroundTextures_ArchiveFile : ArchiveFile {
        public GSTextures_File[] Textures { get; set; }
        public RawData_File DummyFile { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Textures ??= new GSTextures_File[OffsetTable.FilesCount];
            for (int i = 0; i < OffsetTable.FilesCount; i++)
            {
                if (GetFileEndPointer(i) - OffsetTable.FilePointers[i] != 0x10)
                {
                    Textures[i] = SerializeFile(s, Textures[i], i, name: $"{nameof(Textures)}[{i}]");
                } 
                else
                {
                    DummyFile ??= SerializeFile(s, DummyFile, i, name: $"{nameof(DummyFile)}[{i}]");
                    FlagAsParsed(i, DummyFile, name: nameof(DummyFile));
                }
            }
        }
    }
}