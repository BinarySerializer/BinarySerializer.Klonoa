namespace BinarySerializer.Klonoa.LV
{
    public class BINHeader_BGMFileDescriptor : BINHeader_BaseFileDescriptor
    {
        public uint FilesOffsetValue { get; set; }
        public override uint FILE_Offset => FilesOffsetValue * SectorSize;
        public uint FileLength { get; set; }
        public uint FileAbsoluteLength => FileLength + (SectorSize - FileLength % SectorSize);
        public override uint FILE_Length => FileAbsoluteLength * FilesCount;
        public uint FilesCount { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FilesOffsetValue = s.Serialize<uint>(FilesOffsetValue, name: nameof(FilesOffsetValue));
            s.Log("{0}: {1}", nameof(FILE_Offset), FILE_Offset);
            FileLength = s.Serialize<uint>(FileLength, name: nameof(FileLength));
            s.Log("{0}: {1}", nameof(FileAbsoluteLength), FileAbsoluteLength);
            FilesCount = s.Serialize<uint>(FilesCount, name: nameof(FilesCount));
        }
    }
}