namespace BinarySerializer.Klonoa.DTP
{
    public class CutscenePlayerSprite_File : BaseFile
    {
        // XPos = 0x3c0, YPos = 0x180, 8bpp
        public byte Height { get; set; } // Always same as height
        public byte Width { get; set; } // Always half of width

        public byte ImgHeight { get; set; } // Always same as height
        public byte ImgWidth { get; set; } // Always 2x the width

        public byte[] ImgData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Height = s.Serialize<byte>(Height, name: nameof(Height));
            Width = s.Serialize<byte>(Width, name: nameof(Width));
            ImgHeight = s.Serialize<byte>(ImgHeight, name: nameof(ImgHeight));
            ImgWidth = s.Serialize<byte>(ImgWidth, name: nameof(ImgWidth));
            ImgData = s.SerializeArray<byte>(ImgData, ImgWidth * ImgHeight, name: nameof(ImgData));
        }
    }
}