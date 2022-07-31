namespace BinarySerializer.Klonoa.LV
{
    public class Cutscenes_ArchiveFile : ArchiveFile
    {
        public CutsceneFile[] Cutscenes { get; set; }
        public GSTextures_File[] Font { get; set; } // Japan version only
        public RawData_File DummyFile { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Cutscenes ??= new CutsceneFile[OffsetTable.FilesCount / 2];
            Font ??= new GSTextures_File[OffsetTable.FilesCount / 2];
            for (int i = 0; i < OffsetTable.FilesCount / 2; i++)
            {
                Cutscenes[i] = SerializeFile(s, Cutscenes[i], i * 2, name: $"{nameof(Cutscenes)}[{i}]");
                int fontIdx = i * 2 + 1;
                if (GetFileEndPointer(fontIdx) - OffsetTable.FilePointers[fontIdx] != 0x10)
                {
                    Font[i] = SerializeFile(s, Font[i], fontIdx, name: $"{nameof(Font)}[{i}]");
                }
                else
                {
                    DummyFile = SerializeFile(s, DummyFile, fontIdx, name: $"{nameof(DummyFile)}[{i}]");
                    FlagAsParsed(fontIdx, DummyFile, name: nameof(DummyFile));
                }
            }
        }
    }
}