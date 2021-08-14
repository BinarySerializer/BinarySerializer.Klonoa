namespace BinarySerializer.Klonoa.LV
{
    public class BINFileDescriptor : BinarySerializable
    {
        private const uint SectorSize = 2048;

        public uint FileOffsetValue { get; set; }
        public uint FileOffset => FileOffsetValue * SectorSize;
        public Pointer FilePointer { get; set; }
        public uint FileLengthValue { get; set; }
        public uint FileLength => FileLengthValue * SectorSize;

        public override void SerializeImpl(SerializerObject s)
        {
            FileOffsetValue = s.Serialize<uint>(FileOffsetValue, name: nameof(FileOffsetValue));
            s.Log($"{nameof(FileOffset)}: {FileOffset}");
            FileLengthValue = s.Serialize<uint>(FileLengthValue, name: nameof(FileLengthValue));
            s.Log($"{nameof(FileLength)}: {FileLength}");
        }
    }
}