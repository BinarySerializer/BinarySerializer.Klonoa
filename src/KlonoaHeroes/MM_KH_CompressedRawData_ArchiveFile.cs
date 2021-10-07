namespace BinarySerializer.Klonoa.KH
{
    public class MM_KH_CompressedRawData_ArchiveFile : ArchiveFile<RawData_File>
    {
        public MM_KH_CompressedRawData_ArchiveFile() => Pre_FileEncoder = new MM_KH_Encoder();
    }
}