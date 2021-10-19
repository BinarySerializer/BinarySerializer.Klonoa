using BinarySerializer.GBA;

namespace BinarySerializer.Klonoa.KH
{
    public class AnimationSprite : BinarySerializable
    {
        public sbyte XPos { get; set; }
        public sbyte YPos { get; set; }
        public GBA_OBJ_ATTR ObjAttr { get; set; }
        public byte PaletteMode { get; set; }
        public byte PaletteIndex { get; set; }
        public byte PaletteMemoryIndex { get; set; } // The index in memory, not in the animation file
        public ushort TileSetLength { get; set; }
        public uint TileSetOffset { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            XPos = s.Serialize<sbyte>(XPos, name: nameof(XPos));
            YPos = s.Serialize<sbyte>(YPos, name: nameof(YPos));
            ObjAttr = s.SerializeObject<GBA_OBJ_ATTR>(ObjAttr, name: nameof(ObjAttr));
            s.SerializeBitValues<byte>(bitFunc =>
            {
                PaletteMode = (byte)bitFunc(PaletteMode, 4, name: nameof(PaletteMode));
                PaletteIndex = (byte)bitFunc(PaletteIndex, 4, name: nameof(PaletteIndex));
            });
            PaletteMemoryIndex = s.Serialize<byte>(PaletteMemoryIndex, name: nameof(PaletteMemoryIndex));
            TileSetLength = s.Serialize<ushort>(TileSetLength, name: nameof(TileSetLength));
            TileSetOffset = s.Serialize<uint>(TileSetOffset, name: nameof(TileSetOffset));
        }
    }
}