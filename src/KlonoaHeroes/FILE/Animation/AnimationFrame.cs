namespace BinarySerializer.Klonoa.KH
{
    public class AnimationFrame : BinarySerializable
    {
        public byte Speed { get; set; } // Time in frames
        public byte SpritesCount { get; set; }
        public uint SpritesOffset { get; set; }

        public AnimationSprite[] Sprites { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("AS", 2);
            Speed = s.Serialize<byte>(Speed, name: nameof(Speed));
            SpritesCount = s.Serialize<byte>(SpritesCount, name: nameof(SpritesCount));
            SpritesOffset = s.Serialize<uint>(SpritesOffset, name: nameof(SpritesOffset));
            s.SerializePadding(8, logIfNotNull: true);
            s.DoAt(Offset + SpritesOffset, () => Sprites = s.SerializeObjectArray<AnimationSprite>(Sprites, SpritesCount, name: nameof(Sprites)));
        }
    }
}