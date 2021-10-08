namespace BinarySerializer.Klonoa.KH
{
    // TODO: Is it a story pack? Or other data as well? Game has multiple top-level archives using slightly different headers.
    public class StoryPack_ArchiveFile : ArchiveFile
    {
        public StoryPack_File0_ArchiveFile File_0 { get; set; }
        public MM_KH_Compressed_ArchiveFile<Graphics_File> File_1 { get; set; }
        public ArchiveFile<ArchiveFile<Graphics_File>> File_2 { get; set; }
        public ArchiveFile<ArchiveFile<ArchiveFile<Cutscene_File>>> Cutscenes { get; set; }
        public ArchiveFile<Graphics_File> File_4 { get; set; }
        public Graphics_File File_5 { get; set; }
        public MM_KH_Compressed_ArchiveFile<Graphics_File> File_6 { get; set; }
        public MM_KH_Compressed_ArchiveFile<Graphics_File> File_7 { get; set; }
        public Graphics_File File_8 { get; set; }
        public ArchiveFile<ArchiveFile<Graphics_File>> File_9 { get; set; }
        public ArchiveFile<RawData_ArchiveFile> File_10 { get; set; }
        public RawData_File File_11 { get; set; }
        public ArchiveFile<RawData_ArchiveFile> File_12 { get; set; }
        public ArchiveFile<Graphics_File> File_13 { get; set; }
        public RawData_ArchiveFile File_14 { get; set; }

        // Null
        public RawData_File File_15 { get; set; }
        public RawData_File File_16 { get; set; }
        public RawData_File File_17 { get; set; }
        public RawData_File File_18 { get; set; }
        public RawData_File File_19 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<StoryPack_File0_ArchiveFile>(s, File_0, 0, name: nameof(File_0));
            File_1 = SerializeFile<MM_KH_Compressed_ArchiveFile<Graphics_File>>(s, File_1, 1, name: nameof(File_1));
            File_2 = SerializeFile<ArchiveFile<ArchiveFile<Graphics_File>>>(s, File_2, 2, name: nameof(File_2));
            Cutscenes = SerializeFile<ArchiveFile<ArchiveFile<ArchiveFile<Cutscene_File>>>>(s, Cutscenes, 3, name: nameof(Cutscenes));
            File_4 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_4, 4, name: nameof(File_4));
            File_5 = SerializeFile<Graphics_File>(s, File_5, 5, name: nameof(File_5));
            File_6 = SerializeFile<MM_KH_Compressed_ArchiveFile<Graphics_File>>(s, File_6, 6, name: nameof(File_6));
            File_7 = SerializeFile<MM_KH_Compressed_ArchiveFile<Graphics_File>>(s, File_7, 7, name: nameof(File_7));
            File_8 = SerializeFile<Graphics_File>(s, File_8, 8, name: nameof(File_8));
            File_9 = SerializeFile<ArchiveFile<ArchiveFile<Graphics_File>>>(s, File_9, 9, name: nameof(File_9));
            File_10 = SerializeFile<ArchiveFile<RawData_ArchiveFile>>(s, File_10, 10, name: nameof(File_10));
            File_11 = SerializeFile<RawData_File>(s, File_11, 11, name: nameof(File_11));
            File_12 = SerializeFile<ArchiveFile<RawData_ArchiveFile>>(s, File_12, 12, name: nameof(File_12));
            File_13 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_13, 13, name: nameof(File_13));
            File_14 = SerializeFile<RawData_ArchiveFile>(s, File_14, 14, name: nameof(File_14));
            File_15 = SerializeFile<RawData_File>(s, File_15, 15, name: nameof(File_15));
            File_16 = SerializeFile<RawData_File>(s, File_16, 16, name: nameof(File_16));
            File_17 = SerializeFile<RawData_File>(s, File_17, 17, name: nameof(File_17));
            File_18 = SerializeFile<RawData_File>(s, File_18, 18, name: nameof(File_18));
            File_19 = SerializeFile<RawData_File>(s, File_19, 19, name: nameof(File_19));
        }
    }
}