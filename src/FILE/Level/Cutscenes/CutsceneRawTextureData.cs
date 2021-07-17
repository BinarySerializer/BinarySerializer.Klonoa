namespace BinarySerializer.KlonoaDTP
{
    public class CutsceneRawTextureData : BaseFile
    {
        public byte Byte_00 { get; set; } // Always same as height
        public byte Byte_01 { get; set; } // Always half of width

        // XPos = 0x3c0, YPos = 0x180, 8bpp
        public byte Height { get; set; }
        public byte Width { get; set; }

        public byte[] ImgData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Byte_00 = s.Serialize<byte>(Byte_00, name: nameof(Byte_00));
            Byte_01 = s.Serialize<byte>(Byte_01, name: nameof(Byte_01));
            Height = s.Serialize<byte>(Height, name: nameof(Height));
            Width = s.Serialize<byte>(Width, name: nameof(Width));
            ImgData = s.SerializeArray<byte>(ImgData, Width * Height, name: nameof(ImgData));
        }
    }
}