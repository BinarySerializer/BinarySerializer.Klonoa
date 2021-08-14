namespace BinarySerializer.Klonoa.LV
{
    public abstract class BINHeader_BaseFileDescriptor : BinarySerializable
    {
        protected const uint SectorSize = 2048;
        public abstract uint FILE_Offset { get; }
        public abstract uint FILE_Length { get; }
        public Pointer FilePointer { get; set; }
    }
}