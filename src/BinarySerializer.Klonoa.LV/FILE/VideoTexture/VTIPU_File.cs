using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class VTIPU_File : IPU_File
    {
        public byte[] TextureDescriptor { get; set; } // TODO: Parse this
        public Pointer[] FramePointers { get; set; }
        public override int FPS => 60;
        public override bool IsAligned => true;

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("vtip", 4, throwIfNoMatch: true);
            DataSize = s.Serialize<uint>(DataSize, name: nameof(DataSize));
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));
            FrameCount = s.Serialize<uint>(FrameCount, name: nameof(FrameCount));
            TextureDescriptor = s.SerializeArray<byte>(TextureDescriptor, 16, name: nameof(TextureDescriptor));
            FramePointers = s.SerializePointerArray(FramePointers, FrameCount + 2, anchor: Offset + 0x20, name: nameof(FramePointers)); // Add 2 for the final frame delimiter and the end block
            s.Align(16);
            FrameData = s.SerializeArray<byte>(FrameData, DataSize, name: nameof(FrameData));
        }
    }
}