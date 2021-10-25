namespace BinarySerializer.Klonoa.KH
{
    public class MenuPack_CutsceneViewer_ArchiveFile : PF_ArchiveFile
    {
        public Graphics_File File_0 { get; set; }
        public Graphics_File File_1 { get; set; }
        public Graphics_File File_2 { get; set; }
        public RawData_File File_3 { get; set; }
        public ArchiveFile<TextCommands> File_4 { get; set; }
        public TextCommands File_5 { get; set; }
        public TextCommands File_6 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<Graphics_File>(s, File_0, 0, name: nameof(File_0));
            File_1 = SerializeFile<Graphics_File>(s, File_1, 1, name: nameof(File_1));
            File_2 = SerializeFile<Graphics_File>(s, File_2, 2, name: nameof(File_2));
            File_3 = SerializeFile<RawData_File>(s, File_3, 3, name: nameof(File_3));
            File_4 = SerializeFile<ArchiveFile<TextCommands>>(s, File_4, 4, name: nameof(File_4));
            File_5 = SerializeFile<TextCommands>(s, File_5, 5, name: nameof(File_5));
            File_6 = SerializeFile<TextCommands>(s, File_6, 6, name: nameof(File_6));
        }
    }
}