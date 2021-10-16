namespace BinarySerializer.Klonoa.KH
{
    public class UnknownPack_File9_ArchiveFile : PF_ArchiveFile
    {
        public Graphics_File File_0 { get; set; }
        public Graphics_File File_1 { get; set; }
        public Graphics_File File_2 { get; set; }
        public TextCommands File_3 { get; set; }
        public ArchiveFile<TextCommands> File_4 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<Graphics_File>(s, File_0, 0, name: nameof(File_0));
            File_1 = SerializeFile<Graphics_File>(s, File_1, 1, name: nameof(File_1));
            File_2 = SerializeFile<Graphics_File>(s, File_2, 2, name: nameof(File_2));
            File_3 = SerializeFile<TextCommands>(s, File_3, 3, name: nameof(File_3));
            File_4 = SerializeFile<ArchiveFile<TextCommands>>(s, File_4, 4, name: nameof(File_4));
        }
    }
}