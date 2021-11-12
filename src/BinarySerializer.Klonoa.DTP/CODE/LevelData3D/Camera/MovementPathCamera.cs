namespace BinarySerializer.Klonoa.DTP
{
    public class MovementPathCamera : BinarySerializable
    {
        public int Int_00 { get; set; } // Position from path start
        public short Short_04 { get; set; }
        public short Short_06 { get; set; }
        public short Short_08 { get; set; }
        public short Short_0A { get; set; }
        public short Short_0C { get; set; }
        public short Short_0E { get; set; }
        public short Short_10 { get; set; }
        public CameraType Type { get; set; }
        public Pointer PositionPointer { get; set; }

        // Serialized from pointers
        public KlonoaVector16 AbsolutePosition { get; set; }
        public KlonoaVector16 AbsoluteRotation { get; set; }

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
            Type = s.Serialize<CameraType>(Type, name: nameof(Type));
            PositionPointer = s.SerializePointer(PositionPointer, allowInvalid: true, name: nameof(PositionPointer));

            s.DoAt(PositionPointer, () =>
            {
                AbsolutePosition = s.SerializeObject<KlonoaVector16>(AbsolutePosition, name: nameof(AbsolutePosition));
                s.SerializePadding(2);
                AbsoluteRotation = s.SerializeObject<KlonoaVector16>(AbsoluteRotation, name: nameof(AbsoluteRotation));
                s.SerializePadding(2);
            });
        }

        public enum CameraType : short
        {
            RelativeToPath = 0,
            Absolute = 1,
            Unknown = 2,
        }
    }
}