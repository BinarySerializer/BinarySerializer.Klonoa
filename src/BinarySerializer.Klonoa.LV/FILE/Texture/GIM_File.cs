using BinarySerializer.PlayStation.PS2;

namespace BinarySerializer.Klonoa.LV
{
    // Basic 4bpp/8bpp texture format
    public class GIM_File : BaseFile
    {
        public uint TextureCount { get; set; }
        public ushort Texture_DBP { get; set; }
        public ushort Texture_DBW { get; set; }
        public GS.PixelStorageMode Texture_DPSM { get; set; }
        public uint Texture_BPPFlag { get; set; } // Not sure what this does, but its value is 8 when the texture is 4bpp and 2 when it is 8bpp. BPP is already defined in DPSM, so might as well use that.
        public ushort Texture_DSAX { get; set; } // X
        public ushort Texture_DSAY { get; set; } // Y
        public ushort Texture_RRW { get; set; } // Width
        public ushort Texture_RRH { get; set; } // Height
        public byte[] Texture_Data { get; set; }
        public ushort Palette_DBP { get; set; }
        public ushort Palette_DBW { get; set; }
        public GS.PixelStorageMode Palette_DPSM { get; set; }
        public uint Palette_BPPFlag { get; set; }
        public ushort Palette_DSAX { get; set; }
        public ushort Palette_DSAY { get; set; }
        public ushort Palette_RRW { get; set; }
        public ushort Palette_RRH { get; set; }
        public PS2_RGBA8888Color[] Palette { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Header
            s.SerializeMagicString("GIM", 3);
            s.SerializePadding(1);
            TextureCount = s.Serialize<uint>(TextureCount, name: nameof(TextureCount));
            s.SerializePadding(8);

            // Texture data

            // BITBLTBUF register info
            s.DoBits<int>(b =>
            {
                Texture_DBP = b.SerializeBits<ushort>(Texture_DBP, 14, name: nameof(Texture_DBP));
                b.SerializePadding(2);
                Texture_DBW = b.SerializeBits<ushort>(Texture_DBW, 6, name: nameof(Texture_DBW));
                b.SerializePadding(2);
                Texture_DPSM = b.SerializeBits<GS.PixelStorageMode>(Texture_DPSM, 6, name: nameof(Texture_DPSM));
                b.SerializePadding(2);
            });

            Texture_BPPFlag = s.Serialize<uint>(Texture_BPPFlag, name: nameof(Texture_BPPFlag));
            Texture_DSAX = s.Serialize<ushort>(Texture_DSAX, name: nameof(Texture_DSAX));
            Texture_DSAY = s.Serialize<ushort>(Texture_DSAY, name: nameof(Texture_DSAY));
            Texture_RRW = s.Serialize<ushort>(Texture_RRW, name: nameof(Texture_RRW));
            Texture_RRH = s.Serialize<ushort>(Texture_RRH, name: nameof(Texture_RRH));
            Texture_Data = s.SerializeArray<byte>(Texture_Data, Texture_DPSM == GS.PixelStorageMode.PSMT4 ? (Texture_RRW * Texture_RRH) / 2 : Texture_RRW * Texture_RRH, name: nameof(Texture_Data));

            // Palette

            // BITBLTBUF register info
            s.DoBits<int>(b =>
            {
                Palette_DBP = b.SerializeBits<ushort>(Palette_DBP, 14, name: nameof(Palette_DBP));
                b.SerializePadding(2);
                Palette_DBW = b.SerializeBits<ushort>(Palette_DBW, 6, name: nameof(Palette_DBW));
                b.SerializePadding(2);
                Palette_DPSM = b.SerializeBits<GS.PixelStorageMode>(Palette_DPSM, 6, name: nameof(Palette_DPSM));
                b.SerializePadding(2);
            });

            Palette_BPPFlag = s.Serialize<uint>(Palette_BPPFlag, name: nameof(Palette_BPPFlag));
            Palette_DSAX = s.Serialize<ushort>(Palette_DSAX, name: nameof(Palette_DSAX));
            Palette_DSAY = s.Serialize<ushort>(Palette_DSAY, name: nameof(Palette_DSAY));
            Palette_RRW = s.Serialize<ushort>(Palette_RRW, name: nameof(Palette_RRW));
            Palette_RRH = s.Serialize<ushort>(Palette_RRH, name: nameof(Palette_RRH));
            Palette = s.SerializeObjectArray<PS2_RGBA8888Color>(Palette, Palette_RRW * Palette_RRH, name: nameof(Palette));
        }
    }
}