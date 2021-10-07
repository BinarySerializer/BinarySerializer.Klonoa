namespace BinarySerializer.Klonoa.KH
{
    public class Graphics_File : BaseFile
    {
        public string Magic { get; set; } // CT
        public ushort TileMapWidth { get; set; }
        public ushort TileMapHeight { get; set; }
        public byte BPP { get; set; } // 4 or 8
        public byte PalettesCount { get; set; }
        public uint PaletteOffset { get; set; }
        public ushort TileSetWidth { get; set; }
        public ushort TileSetHeight { get; set; }
        public uint TileSetLength { get; set; }
        public uint TileSetOffset { get; set; }
        public uint TileMapLength { get; set; }
        public uint TileMapOffset { get; set; }

        public RGBA5551Color[] Palette { get; set; }
        public byte[] TileSet { get; set; }
        public ushort[] TileMap { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Magic = s.SerializeString(Magic, 2, name: nameof(Magic));
            TileMapWidth = s.Serialize<ushort>(TileMapWidth, name: nameof(TileMapWidth));
            TileMapHeight = s.Serialize<ushort>(TileMapHeight, name: nameof(TileMapHeight));
            BPP = s.Serialize<byte>(BPP, name: nameof(BPP));
            PalettesCount = s.Serialize<byte>(PalettesCount, name: nameof(PalettesCount));
            PaletteOffset = s.Serialize<uint>(PaletteOffset, name: nameof(PaletteOffset));
            TileSetWidth = s.Serialize<ushort>(TileSetWidth, name: nameof(TileSetWidth));
            TileSetHeight = s.Serialize<ushort>(TileSetHeight, name: nameof(TileSetHeight));
            TileSetLength = s.Serialize<uint>(TileSetLength, name: nameof(TileSetLength));
            TileSetOffset = s.Serialize<uint>(TileSetOffset, name: nameof(TileSetOffset));
            TileMapLength = s.Serialize<uint>(TileMapLength, name: nameof(TileMapLength));
            TileMapOffset = s.Serialize<uint>(TileMapOffset, name: nameof(TileMapOffset));

            s.DoAt(Offset + PaletteOffset, () => Palette = s.SerializeObjectArray<RGBA5551Color>(Palette, PalettesCount * 16, name: nameof(Palette)));
            s.DoAt(Offset + TileSetOffset, () => TileSet = s.SerializeArray<byte>(TileSet, TileSetLength, name: nameof(TileSet)));
            s.DoAt(Offset + TileMapOffset, () => TileMap = s.SerializeArray<ushort>(TileMap, TileMapLength / 2, name: nameof(TileMap)));
            
            s.Goto(Offset + TileMapOffset + TileMapLength);
        }
    }
}