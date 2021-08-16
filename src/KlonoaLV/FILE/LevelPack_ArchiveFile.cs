namespace BinarySerializer.Klonoa.LV
{
    public class LevelPack_ArchiveFile : ArchiveFile
    {
        public RawData_ArchiveFile File_0 { get; set; }
        public RawData_ArchiveFile File_1 { get; set; }
        public RawData_ArchiveFile File_2 { get; set; }
        public RawData_ArchiveFile File_3 { get; set; }
        public RawData_ArchiveFile File_4 { get; set; }
        public RawData_ArchiveFile File_5 { get; set; }
        public RawData_ArchiveFile File_6 { get; set; }
        public RawData_ArchiveFile File_7 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile(s, File_0, 0, name: nameof(File_0));
            File_1 = SerializeFile(s, File_1, 1, name: nameof(File_1));
            File_2 = SerializeFile(s, File_2, 2, name: nameof(File_2));
            File_3 = SerializeFile(s, File_3, 3, name: nameof(File_3));
            File_4 = SerializeFile(s, File_4, 4, name: nameof(File_4));
            File_5 = SerializeFile(s, File_5, 5, name: nameof(File_5));
            File_6 = SerializeFile(s, File_6, 6, name: nameof(File_6));
            File_7 = SerializeFile(s, File_7, 7, name: nameof(File_7));
        }
    }
}