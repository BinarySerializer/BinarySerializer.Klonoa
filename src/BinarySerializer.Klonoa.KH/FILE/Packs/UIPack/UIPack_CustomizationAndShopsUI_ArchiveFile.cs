namespace BinarySerializer.Klonoa.KH
{
    public class UIPack_CustomizationAndShopsUI_ArchiveFile : ArchiveFile
    {
        public Graphics_File File_0 { get; set; }
        public Graphics_File File_1 { get; set; }
        public Graphics_File File_2 { get; set; }
        public ArchiveFile<Graphics_File> File_3 { get; set; }
        public ArchiveFile<ArchiveFile<Graphics_File>> File_4 { get; set; }
        public ArchiveFile<ArchiveFile<Graphics_File>> File_5 { get; set; }
        public ArchiveFile<ArchiveFile<Graphics_File>> File_6 { get; set; }
        public TextCommands File_7 { get; set; }
        public TextCommands File_8 { get; set; }
        public TextCommands File_9 { get; set; }
        public TextCommands File_10 { get; set; }
        public TextCommands File_11 { get; set; }
        public TextCommands File_12 { get; set; }
        public TextCommands File_13 { get; set; }
        public TextCommands File_14 { get; set; }
        public TextCommands File_15 { get; set; }
        public TextCommands File_16 { get; set; }
        public TextCommands File_17 { get; set; }
        public TextCommands File_18 { get; set; }
        public TextCommands File_19 { get; set; }
        public TextCommands File_20 { get; set; }
        public TextCommands File_21 { get; set; }
        public TextCommands File_22 { get; set; }
        public TextCommands File_23 { get; set; }
        public TextCommands File_24 { get; set; }
        public TextCommands File_25 { get; set; }
        public TextCommands File_26 { get; set; }
        public TextCommands File_27 { get; set; }
        public TextCommands File_28 { get; set; }
        public TextCommands File_29 { get; set; }
        public TextCommands File_30 { get; set; }
        public TextCommands File_31 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<Graphics_File>(s, File_0, 0, name: nameof(File_0));
            File_1 = SerializeFile<Graphics_File>(s, File_1, 1, name: nameof(File_1));
            File_2 = SerializeFile<Graphics_File>(s, File_2, 2, name: nameof(File_2));
            File_3 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_3, 3, name: nameof(File_3));
            File_4 = SerializeFile<ArchiveFile<ArchiveFile<Graphics_File>>>(s, File_4, 4, name: nameof(File_4));
            File_5 = SerializeFile<ArchiveFile<ArchiveFile<Graphics_File>>>(s, File_5, 5, name: nameof(File_5));
            File_6 = SerializeFile<ArchiveFile<ArchiveFile<Graphics_File>>>(s, File_6, 6, name: nameof(File_6));
            File_7 = SerializeFile<TextCommands>(s, File_7, 7, name: nameof(File_7));
            File_8 = SerializeFile<TextCommands>(s, File_8, 8, name: nameof(File_8));
            File_9 = SerializeFile<TextCommands>(s, File_9, 9, name: nameof(File_9));
            File_10 = SerializeFile<TextCommands>(s, File_10, 10, name: nameof(File_10));
            File_11 = SerializeFile<TextCommands>(s, File_11, 11, name: nameof(File_11));
            File_12 = SerializeFile<TextCommands>(s, File_12, 12, name: nameof(File_12));
            File_13 = SerializeFile<TextCommands>(s, File_13, 13, name: nameof(File_13));
            File_14 = SerializeFile<TextCommands>(s, File_14, 14, name: nameof(File_14));
            File_15 = SerializeFile<TextCommands>(s, File_15, 15, name: nameof(File_15));
            File_16 = SerializeFile<TextCommands>(s, File_16, 16, name: nameof(File_16));
            File_17 = SerializeFile<TextCommands>(s, File_17, 17, name: nameof(File_17));
            File_18 = SerializeFile<TextCommands>(s, File_18, 18, name: nameof(File_18));
            File_19 = SerializeFile<TextCommands>(s, File_19, 19, name: nameof(File_19));
            File_20 = SerializeFile<TextCommands>(s, File_20, 20, name: nameof(File_20));
            File_21 = SerializeFile<TextCommands>(s, File_21, 21, name: nameof(File_21));
            File_22 = SerializeFile<TextCommands>(s, File_22, 22, name: nameof(File_22));
            File_23 = SerializeFile<TextCommands>(s, File_23, 23, name: nameof(File_23));
            File_24 = SerializeFile<TextCommands>(s, File_24, 24, name: nameof(File_24));
            File_25 = SerializeFile<TextCommands>(s, File_25, 25, name: nameof(File_25));
            File_26 = SerializeFile<TextCommands>(s, File_26, 26, name: nameof(File_26));
            File_27 = SerializeFile<TextCommands>(s, File_27, 27, name: nameof(File_27));
            File_28 = SerializeFile<TextCommands>(s, File_28, 28, name: nameof(File_28));
            File_29 = SerializeFile<TextCommands>(s, File_29, 29, name: nameof(File_29));
            File_30 = SerializeFile<TextCommands>(s, File_30, 30, name: nameof(File_30));
            File_31 = SerializeFile<TextCommands>(s, File_31, 31, name: nameof(File_31));
        }
    }
}