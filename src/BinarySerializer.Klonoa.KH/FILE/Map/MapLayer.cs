using System;

namespace BinarySerializer.Klonoa.KH
{
    public class MapLayer : BinarySerializable
    {
        public Pointer Pre_SharedDataPointer { get; set; }
        public uint TileMapOffset { get; set; }
        public uint PaletteOffset { get; set; }
        public uint TileSetOffset { get; set; } // Gets appended after shared tile set
        public uint CollisionMapOffset { get; set; } // Collision is 16x16 tiles
        public short Width { get; set; }
        public short Height { get; set; }
        public uint TileMapLength { get; set; }
        public uint TileSetLength { get; set; }
        public uint CollisionMapLength { get; set; }
        public uint SharedTileSetOffset { get; set; }
        public uint SharedTileSetLength { get; set; }
        public ushort Layer { get; set; } // 10 for collision, otherwise 0-3
        public ushort Priority { get; set; } // 0 for collision, otherwise 1 or 2
        public ushort Ushort_38 { get; set; } // 0, 2 or 4
        public ushort Ushort_3A { get; set; } // Value between 0-10
        public ushort Ushort_3C { get; set; }
        public ushort Ushort_3E { get; set; }

        // Parsed from offsets
        public GraphicsTile[] TileMap { get; set; }
        public RGBA5551Color[] Palette { get; set; }
        public byte[] TileSet { get; set; }
        public byte[] CollisionMap { get; set; }
        public byte[] SharedTileSet { get; set; }

        private void DoAtOffset(SerializerObject s, uint offset, Action action)
        {
            if (offset == 0)
                return;

            s.DoAt(Offset + offset, action);
        }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("KS", 4);
            TileMapOffset = s.Serialize<uint>(TileMapOffset, name: nameof(TileMapOffset));
            PaletteOffset = s.Serialize<uint>(PaletteOffset, name: nameof(PaletteOffset));
            TileSetOffset = s.Serialize<uint>(TileSetOffset, name: nameof(TileSetOffset));
            CollisionMapOffset = s.Serialize<uint>(CollisionMapOffset, name: nameof(CollisionMapOffset));
            Width = s.Serialize<short>(Width, name: nameof(Width));
            Height = s.Serialize<short>(Height, name: nameof(Height));
            TileMapLength = s.Serialize<uint>(TileMapLength, name: nameof(TileMapLength));
            TileSetLength = s.Serialize<uint>(TileSetLength, name: nameof(TileSetLength));
            CollisionMapLength = s.Serialize<uint>(CollisionMapLength, name: nameof(CollisionMapLength));
            SharedTileSetOffset = s.Serialize<uint>(SharedTileSetOffset, name: nameof(SharedTileSetOffset));
            SharedTileSetLength = s.Serialize<uint>(SharedTileSetLength, name: nameof(SharedTileSetLength));
            Layer = s.Serialize<ushort>(Layer, name: nameof(Layer));
            Priority = s.Serialize<ushort>(Priority, name: nameof(Priority));
            s.SerializePadding(8, logIfNotNull: true);
            Ushort_38 = s.Serialize<ushort>(Ushort_38, name: nameof(Ushort_38));
            Ushort_3A = s.Serialize<ushort>(Ushort_3A, name: nameof(Ushort_3A));
            Ushort_3C = s.Serialize<ushort>(Ushort_3C, name: nameof(Ushort_3C));
            Ushort_3E = s.Serialize<ushort>(Ushort_3E, name: nameof(Ushort_3E));

            DoAtOffset(s, TileMapOffset, () => TileMap = s.SerializeObjectArray<GraphicsTile>(TileMap, TileMapLength / 2, name: nameof(TileMap)));
            DoAtOffset(s, PaletteOffset, () => Palette = s.SerializeObjectArray<RGBA5551Color>(Palette, 256, name: nameof(Palette)));
            DoAtOffset(s, TileSetOffset, () => TileSet = s.SerializeArray<byte>(TileSet, TileSetLength, name: nameof(TileSet)));
            DoAtOffset(s, CollisionMapOffset, () => CollisionMap = s.SerializeArray<byte>(CollisionMap, CollisionMapLength, name: nameof(CollisionMap)));

            if (SharedTileSetOffset != 0)
                s.DoAt(Pre_SharedDataPointer + SharedTileSetOffset, () =>
                {
                    s.DoEncoded(new BytePairEncoder(), () =>
                    {
                        SharedTileSet = s.SerializeArray<byte>(SharedTileSet, SharedTileSetLength, name: nameof(SharedTileSet));
                    });
                });
        }
    }
}