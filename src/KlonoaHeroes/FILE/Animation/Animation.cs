namespace BinarySerializer.Klonoa.KH
{
    public class Animation : BinarySerializable
    {
        public ushort FramesCount { get; set; }
        public uint FrameOffsetsOffset { get; set; }
        public int[] FrameOffsets { get; set; }

        public AnimationFrame[] Frames { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("AF", 2);
            FramesCount = s.Serialize<ushort>(FramesCount, name: nameof(FramesCount));
            FrameOffsetsOffset = s.Serialize<uint>(FrameOffsetsOffset, name: nameof(FrameOffsetsOffset));
            s.SerializePadding(8, logIfNotNull: true);

            s.DoAt(Offset + FrameOffsetsOffset, () => FrameOffsets = s.SerializeArray<int>(FrameOffsets, FramesCount, name: nameof(FrameOffsets)));

            Frames ??= new AnimationFrame[FramesCount];

            for (int i = 0; i < Frames.Length; i++)
                s.DoAt(Offset + FrameOffsets[i], () => Frames[i] = s.SerializeObject<AnimationFrame>(Frames[i], name: $"{nameof(Frames)}[{i}]"));
        }
    }
}