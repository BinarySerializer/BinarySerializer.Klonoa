namespace BinarySerializer.Klonoa.KH
{
    public class MM_KH_Compressed_ArchiveFile<T> : ArchiveFile<T>
        where T : BinarySerializable, new()
    {
        public MM_KH_Compressed_ArchiveFile() => Pre_FileEncoder = new MM_KH_Encoder();
    }
}