namespace BinarySerializer.Klonoa.LV
{
    public class BINHeader_File : BaseFile
    {
        public uint FilesCount { get; set; }
        public BINFileDescriptor[] FileDescriptors { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FilesCount = s.Serialize<uint>(FilesCount, name: nameof(FilesCount));
            FileDescriptors = s.SerializeArray<BINFileDescriptor>(FileDescriptors, FilesCount, name: nameof(FileDescriptors));
        }
    }
}