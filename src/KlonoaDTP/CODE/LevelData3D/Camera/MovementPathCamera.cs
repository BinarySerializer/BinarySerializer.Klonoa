namespace BinarySerializer.Klonoa.DTP
{
    public class MovementPathCamera : BinarySerializable
    {
        public int Int_00 { get; set; }
        public short Short_04 { get; set; }
        public short Short_06 { get; set; }
        public short Short_08 { get; set; }
        public short Short_0A { get; set; }
        public short Short_0C { get; set; }
        public short Short_0E { get; set; }
        public short Short_10 { get; set; }
        public short Short_12 { get; set; } // Is 1 when the pointer is valid
        public Pointer Pointer_14 { get; set; }

        // Serialized from pointers
        public UnknownStruct[] Structs { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Int_00 = s.Serialize<int>(Int_00, name: nameof(Int_00));
            Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
            Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
            Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
            Short_0A = s.Serialize<short>(Short_0A, name: nameof(Short_0A));
            Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
            Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));
            Short_10 = s.Serialize<short>(Short_10, name: nameof(Short_10));
            Short_12 = s.Serialize<short>(Short_12, name: nameof(Short_12));
            Pointer_14 = s.SerializePointer(Pointer_14, allowInvalid: true, name: nameof(Pointer_14));

            s.DoAt(Pointer_14, () => Structs = s.SerializeObjectArray<UnknownStruct>(Structs, 2, name: nameof(Structs)));
        }

        // Vector?
        public class UnknownStruct : BinarySerializable
        {
            public short Short_00 { get; set; }
            public short Short_02 { get; set; }
            public short Short_04 { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
                Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
                Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
                s.SerializePadding(2, logIfNotNull: true);
            }
        }
    }
}