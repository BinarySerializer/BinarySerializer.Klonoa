namespace BinarySerializer.KlonoaDTP
{
    /// <summary>
    /// A file where the data gets serialized as raw bytes
    /// </summary>
    public class RawData_File : BaseFile
    {
        public byte[] Data { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Data = s.SerializeArray<byte>(Data, Pre_FileSize, name: nameof(Data));
        }
    }
}