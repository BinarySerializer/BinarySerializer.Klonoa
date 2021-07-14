namespace BinarySerializer.KlonoaDTP
{
    public class SpriteAnimation : BinarySerializable
    {
        public Pointer Pre_OffsetAnchor { get; set; }

        public byte FramesCount { get; set; }
        public byte Flags { get; set; } // 0 or 0x80 - determines play direction?
        public ushort FramesOffset { get; set; }

        public SpriteAnimationFrame[] Frames { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FramesCount = s.Serialize<byte>(FramesCount, name: nameof(FramesCount));
            Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
            FramesOffset = s.Serialize<ushort>(FramesOffset, name: nameof(FramesOffset));

            s.DoAt(Pre_OffsetAnchor + FramesOffset, () => Frames = s.SerializeObjectArray<SpriteAnimationFrame>(Frames, FramesCount, name: nameof(Frames)));
        }
    }
}