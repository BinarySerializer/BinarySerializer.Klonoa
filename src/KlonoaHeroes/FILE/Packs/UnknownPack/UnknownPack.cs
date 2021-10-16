namespace BinarySerializer.Klonoa.KH
{
    public class UnknownPack : PF_ArchiveFile
    {
        public Graphics_File File_0 { get; set; }
        public ArchiveFile<Graphics_File> File_1 { get; set; }
        public ArchiveFile<Graphics_File> File_2 { get; set; }
        public UnknownPack_File3_ArchiveFile File_3 { get; set; }
        public RawData_File File_4 { get; set; }
        public RawData_File File_5 { get; set; }
        public Graphics_File File_6 { get; set; }
        public Graphics_File File_7 { get; set; }
        public UnknownPack_File8_ArchiveFile File_8 { get; set; }
        public UnknownPack_File9_ArchiveFile File_9 { get; set; }
        public UnknownPack_File10_ArchiveFile File_10 { get; set; }
        public UnknownPack_File11_ArchiveFile File_11 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<Graphics_File>(s, File_0, 0, name: nameof(File_0));
            File_1 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_1, 1, name: nameof(File_1));
            File_2 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_2, 2, name: nameof(File_2));
            File_3 = SerializeFile<UnknownPack_File3_ArchiveFile>(s, File_3, 3, name: nameof(File_3));
            File_4 = SerializeFile<RawData_File>(s, File_4, 4, name: nameof(File_4));
            File_5 = SerializeFile<RawData_File>(s, File_5, 5, name: nameof(File_5));
            File_6 = SerializeFile<Graphics_File>(s, File_6, 6, name: nameof(File_6));
            File_7 = SerializeFile<Graphics_File>(s, File_7, 7, fileEncoder: new BytePairEncoder(), name: nameof(File_7));
            File_8 = SerializeFile<UnknownPack_File8_ArchiveFile>(s, File_8, 8, name: nameof(File_8));
            File_9 = SerializeFile<UnknownPack_File9_ArchiveFile>(s, File_9, 9, fileEncoder: new BytePairEncoder(), name: nameof(File_9));
            File_10 = SerializeFile<UnknownPack_File10_ArchiveFile>(s, File_10, 10, fileEncoder: new BytePairEncoder(), name: nameof(File_10));
            File_11 = SerializeFile<UnknownPack_File11_ArchiveFile>(s, File_11, 11, fileEncoder: new BytePairEncoder(), name: nameof(File_11));
        }
    }
}