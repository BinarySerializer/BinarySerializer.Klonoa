namespace BinarySerializer.Klonoa.KH
{
    public class Animation_File : BaseFile
    {
        public short PalettesCount { get; set; }
        public short AnimationsCount { get; set; }
        public uint PaletteOffset { get; set; }
        public uint AnimationGroupOffsetsOffset { get; set; }
        public uint TileSetOffset { get; set; }

        public uint[] AnimationGroupOffsets { get; set; }
        public AnimationGroup[] AnimationGroups { get; set; }
        public byte[] TileSet { get; set; }
        public RGBA5551Color[] Palette { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("AN", 2);
            s.SerializePadding(2, logIfNotNull: true);

            PalettesCount = s.Serialize<short>(PalettesCount, name: nameof(PalettesCount));
            AnimationsCount = s.Serialize<short>(AnimationsCount, name: nameof(AnimationsCount));
            PaletteOffset = s.Serialize<uint>(PaletteOffset, name: nameof(PaletteOffset));
            AnimationGroupOffsetsOffset = s.Serialize<uint>(AnimationGroupOffsetsOffset, name: nameof(AnimationGroupOffsetsOffset));
            TileSetOffset = s.Serialize<uint>(TileSetOffset, name: nameof(TileSetOffset));
            s.SerializePadding(12, logIfNotNull: true);

            s.DoAt(Offset + AnimationGroupOffsetsOffset, () => AnimationGroupOffsets = s.SerializeArray<uint>(AnimationGroupOffsets, AnimationsCount, name: nameof(AnimationGroupOffsets)));

            AnimationGroups ??= new AnimationGroup[AnimationsCount];

            for (int i = 0; i < AnimationGroups.Length; i++)
                s.DoAt(Offset + AnimationGroupOffsets[i], () => AnimationGroups[i] = s.SerializeObject<AnimationGroup>(AnimationGroups[i], name: $"{nameof(AnimationGroups)}[{i}]"));

            s.DoAt(Offset + TileSetOffset, () => TileSet = s.SerializeArray<byte>(TileSet, PaletteOffset - TileSetOffset, name: nameof(TileSet)));
            s.DoAt(Offset + PaletteOffset, () => Palette = s.SerializeObjectArray<RGBA5551Color>(Palette, PalettesCount * 16, name: nameof(Palette)));

            s.Goto(Offset + PaletteOffset + PalettesCount * 16 * 2);
        }
    }
}