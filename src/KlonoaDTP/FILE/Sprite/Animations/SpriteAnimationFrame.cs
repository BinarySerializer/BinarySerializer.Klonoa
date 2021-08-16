namespace BinarySerializer.Klonoa.DTP
{
    public class SpriteAnimationFrame : BinarySerializable
    {
        public byte SpriteIndex { get; set; }
        public byte Byte_01 { get; set; }
        public byte Byte_02 { get; set; }
        public byte FrameDelay { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SpriteIndex = s.Serialize<byte>(SpriteIndex, name: nameof(SpriteIndex));
            Byte_01 = s.Serialize<byte>(Byte_01, name: nameof(Byte_01));
            Byte_02 = s.Serialize<byte>(Byte_02, name: nameof(Byte_02));
            FrameDelay = s.Serialize<byte>(FrameDelay, name: nameof(FrameDelay));
        }
    }
}