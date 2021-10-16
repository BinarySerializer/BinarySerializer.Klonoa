namespace BinarySerializer.Klonoa.KH
{
    public class Animation_File : BaseFile
    {
        public string Magic { get; set; }
        public short PalettesCount { get; set; }
        public short ADCount { get; set; }
        public uint PaletteOffset { get; set; }
        public uint ADOffset { get; set; }
        public uint TileSetOffset { get; set; }

        public uint[] ADOffsets { get; set; }
        public byte[] TileSet { get; set; }
        public RGBA5551Color[] Palette { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Magic = s.SerializeString(Magic, 4, name: nameof(Magic));

            if (Magic != "AN")
                throw new BinarySerializableException(this, $"Invalid magic header '{Magic}'");

            PalettesCount = s.Serialize<short>(PalettesCount, name: nameof(PalettesCount));
            ADCount = s.Serialize<short>(ADCount, name: nameof(ADCount));
            PaletteOffset = s.Serialize<uint>(PaletteOffset, name: nameof(PaletteOffset));
            ADOffset = s.Serialize<uint>(ADOffset, name: nameof(ADOffset));
            TileSetOffset = s.Serialize<uint>(TileSetOffset, name: nameof(TileSetOffset));
            s.SerializePadding(12, logIfNotNull: true);

            s.DoAt(Offset + ADOffset, () => ADOffsets = s.SerializeArray<uint>(ADOffsets, ADCount, name: nameof(ADOffsets)));
            // TODO: Each AD struct has AF structs with AS structs
            
            s.DoAt(Offset + TileSetOffset, () => TileSet = s.SerializeArray<byte>(TileSet, PaletteOffset - TileSetOffset, name: nameof(TileSet)));
            s.DoAt(Offset + PaletteOffset, () => Palette = s.SerializeObjectArray<RGBA5551Color>(Palette, PalettesCount * 16, name: nameof(Palette)));

            s.Goto(Offset + PaletteOffset + PalettesCount * 16 * 2);
        }
    }
}