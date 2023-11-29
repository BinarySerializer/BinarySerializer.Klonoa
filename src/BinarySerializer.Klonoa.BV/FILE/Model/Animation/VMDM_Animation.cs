namespace BinarySerializer.Klonoa.BV
{
    public class VMDM_Animation : BinarySerializable
    {
        /// <summary>
        /// The index of the texcoord map to use for all model meshes.<br/>
        /// If this index is out of bounds, use index 0.
        /// </summary>
        public byte TexcoordMap { get; set; }

        /// <summary>
        /// Unknown. Might be bit flags?
        /// </summary>
        public byte Byte_01 { get; set; }

        /// <summary>
        /// Unknown.
        /// </summary>
        public ushort Ushort_02 { get; set; }

        /// <summary>
        /// The index of this animation's name in the <c>Strings</c> array.
        /// </summary>
        public ushort NameIndex { get; set; }

        /// <summary>
        /// The number of bones to animate. This should match the number of bones in the associated <c>VMDG_File</c>.
        /// </summary>
        public ushort BoneCount { get; set; }

        /// <summary>
        /// The index of the root bone's name in the <c>Strings</c> array.
        /// </summary>
        public ushort RootBoneNameIndex { get; set; }

        /// <summary>
        /// Start index of this animations's vectors in the <c>Vectors</c> buffer.<br/>
        /// Starting at this index, there exists a <c>VMDM_Keyframe</c> where <c>Frame</c> equals 0.
        /// </summary>
        public ushort VectorsOffset { get; set; }

        /// <summary>
        /// The number of keyframes this animation has (not counting the implicit start keyframe at <c>VectorsOffset</c>).
        /// </summary>
        public ushort KeyframeCount { get; set; }

        /// <summary>
        /// Start index of this animations's keyframes in the <c>Keyframes</c> buffer.<br/>
        /// </summary>
        public ushort KeyframesOffset { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            TexcoordMap = s.Serialize<byte>(TexcoordMap, name: nameof(TexcoordMap));
            Byte_01 = s.Serialize<byte>(Byte_01, name: nameof(Byte_01));
            Ushort_02 = s.Serialize<ushort>(Ushort_02, name: nameof(Ushort_02));
            NameIndex = s.Serialize<ushort>(NameIndex, name: nameof(NameIndex));
            BoneCount = s.Serialize<ushort>(BoneCount, name: nameof(BoneCount));
            RootBoneNameIndex = s.Serialize<ushort>(RootBoneNameIndex, name: nameof(RootBoneNameIndex));
            VectorsOffset = s.Serialize<ushort>(VectorsOffset, name: nameof(VectorsOffset));
            KeyframeCount = s.Serialize<ushort>(KeyframeCount, name: nameof(KeyframeCount));
            KeyframesOffset = s.Serialize<ushort>(KeyframesOffset, name: nameof(KeyframesOffset));
        }
    }
}