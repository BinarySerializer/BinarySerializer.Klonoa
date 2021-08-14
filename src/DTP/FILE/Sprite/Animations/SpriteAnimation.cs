namespace BinarySerializer.Klonoa.DTP
{
    public class SpriteAnimation : BinarySerializable
    {
        public Pointer Pre_OffsetAnchor { get; set; }

        public int FramesCount { get; set; }
        public bool LoopAnimation { get; set; }
        public ushort FramesOffset { get; set; }

        public SpriteAnimationFrame[] Frames { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeBitValues<ushort>(bitFunc =>
            {
                FramesCount = bitFunc(FramesCount, 15, name: nameof(FramesCount));
                LoopAnimation = bitFunc(LoopAnimation ? 1 : 0, 1, name: nameof(LoopAnimation)) == 1;
            });
            FramesOffset = s.Serialize<ushort>(FramesOffset, name: nameof(FramesOffset));

            s.DoAt(Pre_OffsetAnchor + FramesOffset, () => Frames = s.SerializeObjectArray<SpriteAnimationFrame>(Frames, FramesCount, name: nameof(Frames)));
        }
    }
}