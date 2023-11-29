namespace BinarySerializer.Klonoa.BV
{
    public class VMDM_Keyframe : BinarySerializable
    {
        /// <summary>
        /// The frame index of this keyframe.
        /// Divide by 300 to get the actual time value.
        /// </summary>
        public ushort Frame { get; set; }

        /// <summary>
        /// Start index of this keyframe's vectors in the <c>Vectors</c> buffer.<br/>
        /// The first vector is a translation vector for the root bone, while the rest of the vectors are rotation vectors for the other bones.
        /// </summary>
        public ushort VectorsOffset { get; set; }

        public float Time => Frame / 300.0f;
        
        public override void SerializeImpl(SerializerObject s)
        {
            Frame = s.Serialize<ushort>(Frame, name: nameof(Frame));
            VectorsOffset = s.Serialize<ushort>(VectorsOffset, name: nameof(VectorsOffset));
        }
    }
}