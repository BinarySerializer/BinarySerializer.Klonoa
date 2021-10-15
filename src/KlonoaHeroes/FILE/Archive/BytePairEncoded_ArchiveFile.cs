namespace BinarySerializer.Klonoa.KH
{
    public class BytePairEncoded_ArchiveFile<T> : ArchiveFile<T>
        where T : BinarySerializable, new()
    {
        public BytePairEncoded_ArchiveFile() => Pre_ArchivedFilesEncoder = new BytePairEncoder();
    }
}