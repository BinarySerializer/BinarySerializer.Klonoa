namespace BinarySerializer.Klonoa.DTP
{
    public class SpriteAnimationFrame : BinarySerializable
    {
        public byte SpriteIndex { get; set; }
        public sbyte XPosition { get; set; }
        public byte PlayerAnimation { get; set; } // 0xFF = Normal Sprite, Ox99 = Player Sprite, 0xXX = Normal Player Animation
        public byte FrameDelay { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SpriteIndex = s.Serialize<byte>(SpriteIndex, name: nameof(SpriteIndex));
            XPosition = s.Serialize<sbyte>(XPosition, name: nameof(XPosition));
            PlayerAnimation = s.Serialize<byte>(PlayerAnimation, name: nameof(PlayerAnimation));
            FrameDelay = s.Serialize<byte>(FrameDelay, name: nameof(FrameDelay));
        }
    }
}