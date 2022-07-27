namespace BinarySerializer.Klonoa
{
    /// <summary>
    /// A file where the data gets serialized as raw bytes
    /// </summary>
    public class RawData_File : BaseFile
    {
        public byte[] Data { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            var size = Pre_FileSize;

            if (size == -1)
            {
                s.SystemLog?.LogWarning($"No data parsed for raw data file at {Offset} due to the file size not being specified");
                size = 0;
            }

            Data = s.SerializeArray<byte>(Data, size, name: nameof(Data));
        }
    }
}