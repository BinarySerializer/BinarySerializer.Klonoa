namespace BinarySerializer.Klonoa.BV
{
    public class VMDG_WeightedVector : VMD_Vector
    {
        /// <summary>
        /// The index of the bone that affects this vertex.
        /// </summary>
        public short Bone { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            base.SerializeImpl(s);
            Bone = s.Serialize<short>(Bone, name: nameof(Bone));
        }
    }
}