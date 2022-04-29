using BinarySerializer.Nintendo.GBA;

namespace BinarySerializer.Klonoa.KH
{
    public class Graphics_File : BaseFile
    {
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

        public bool IsAffine { get; set; }
        public bool HasTileMap => TileMapLength != 0;

        public RGBA5551Color[] Palette { get; set; }
        public byte[] TileSet { get; set; }
        public MapTile[] TileMap { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("CT", 2);
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
            s.DoAt(Offset + TileMapOffset, () =>
            {
                // In affine mode each tile is 8-bit rather than 16-bit
                int tilesCount = (TileMapWidth / Constants.TileSize) * (TileMapHeight / Constants.TileSize);
                
                IsAffine = tilesCount == TileMapLength;

                if (!HasTileMap)
                    tilesCount = 0;

                return TileMap = s.SerializeObjectArray<MapTile>(TileMap, tilesCount, x => x.Pre_IsAffine = IsAffine, name: nameof(TileMap));
            });
            
            if (TileMapOffset != 0)
                s.Goto(Offset + TileMapOffset + TileMapLength);
            else
                s.Goto(Offset + TileSetOffset + TileSetLength);
        }
    }
}