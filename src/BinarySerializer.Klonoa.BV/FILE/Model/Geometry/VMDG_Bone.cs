namespace BinarySerializer.Klonoa.BV
{
    public class VMDG_Bone : BinarySerializable
    {
        /// <summary>
        /// The name of this bone.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The local translation (position) vector of this bone.
        /// </summary>
        public VMD_Vector Translation { get; set; }

        /// <summary>
        /// The index of the parent bone. -1 if there is no parent bone.
        /// </summary>
        public sbyte Parent { get; set; }

        /// <summary>
        /// Unknown. (Always -1?)
        /// </summary>
        public sbyte Byte_17 { get; set; }

        /// <summary>
        /// The local rotation vector of this bone.
        /// </summary>
        public VMD_Vector Rotation { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializePadding(4, logIfNotNull: true);
            Name = s.SerializeString(Name, 12, name: nameof(Name));
            Translation = s.SerializeObject<VMD_Vector>(Translation, name: nameof(Translation));
            Parent = s.Serialize<sbyte>(Parent, name: nameof(Parent));
            Byte_17 = s.Serialize<sbyte>(Byte_17, name: nameof(Byte_17));
            Rotation = s.SerializeObject<VMD_Vector>(Rotation, name: nameof(Rotation));
            s.SerializePadding(2, logIfNotNull: true);
        }
    }
}