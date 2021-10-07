namespace BinarySerializer.Klonoa.KH
{
    // TODO: Is it a story pack? Or other data as well? Game has multiple top-level archives using slightly different headers.
    public class StoryPack_ArchiveFile : ArchiveFile
    {
        public MM_KH_CompressedRawData_ArchiveFile File_0 { get; set; }
        public MM_KH_CompressedRawData_ArchiveFile File_1 { get; set; }
        public ArchiveFile<RawData_ArchiveFile> File_2 { get; set; }
        public ArchiveFile<ArchiveFile<RawData_ArchiveFile>> File_3 { get; set; }

        // TODO: There are 20 files total

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<MM_KH_CompressedRawData_ArchiveFile>(s, File_0, 0, name: nameof(File_0));
            File_1 = SerializeFile<MM_KH_CompressedRawData_ArchiveFile>(s, File_1, 1, name: nameof(File_1));
            File_2 = SerializeFile<ArchiveFile<RawData_ArchiveFile>>(s, File_2, 2, name: nameof(File_2));
            File_3 = SerializeFile<ArchiveFile<ArchiveFile<RawData_ArchiveFile>>>(s, File_3, 3, name: nameof(File_3));
        }
    }
}