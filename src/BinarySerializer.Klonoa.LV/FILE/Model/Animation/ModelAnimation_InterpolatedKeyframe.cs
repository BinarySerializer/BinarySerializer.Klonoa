namespace BinarySerializer.Klonoa.LV
{
    public class ModelAnimation_InterpolatedKeyframes : BinarySerializable
    {
        /// <summary>
        /// The number of keyframes for this joint's animation.
        /// </summary>
        public ushort KeyframeCount { get; set; }

        /// <summary>
        /// The frame index associated with each keyframe.
        /// </summary>
        public ushort[] FrameIndices { get; set; }

        /// <summary>
        /// The last value of the frame indices (not used).
        /// </summary>
        public ushort KeyframeTotal { get; set; }

        /// <summary>
        /// The vector associated with each keyframe.
        /// For translations: Multiply the vectors by the Scale value in ModelAnimation.
        /// For rotations: Divide the vectors by 0x7FFF to get intrinsic Euler angles.
        /// </summary>
        public KlonoaLV_Vector16[] Vectors { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            KeyframeCount = s.Serialize<ushort>(KeyframeCount, name: nameof(KeyframeCount));
            FrameIndices = s.SerializeArray<ushort>(FrameIndices, KeyframeCount, name: nameof(FrameIndices));
            KeyframeTotal = s.Serialize<ushort>(KeyframeTotal, name: nameof(KeyframeTotal));
            Vectors = s.SerializeObjectArray<KlonoaLV_Vector16>(Vectors, KeyframeCount, name: nameof(Vectors));
        }
    }
}