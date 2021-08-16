namespace BinarySerializer.Klonoa.LV
{
    public class BINHeader_FileDescriptor : BINHeader_BaseFileDescriptor
    {
        public uint FileOffsetValue { get; set; }
        public override uint FILE_Offset => FileOffsetValue * SectorSize;
        public uint FileLengthValue { get; set; }
        public override uint FILE_Length => FileLengthValue * SectorSize;

        public override void SerializeImpl(SerializerObject s)
        {
            FileOffsetValue = s.Serialize<uint>(FileOffsetValue, name: nameof(FileOffsetValue));
            s.Log($"{nameof(FILE_Offset)}: {FILE_Offset}");
            FileLengthValue = s.Serialize<uint>(FileLengthValue, name: nameof(FileLengthValue));
            s.Log($"{nameof(FILE_Length)}: {FILE_Length}");
        }
    }
}