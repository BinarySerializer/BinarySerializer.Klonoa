namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// A vector with 16-bit values. Y and Z axes are inverted.
    /// </summary>
    public class KlonoaLV_Vector16 : BinarySerializable
    {
        public bool Pre_Padding { get; set; } = false;

        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }

        // Normal vector
        public float NormalX => X / 0x1000;
        public float NormalY => Y / 0x1000;
        public float NormalZ => Z / 0x1000;

        // Rotation in degrees as fixed (xyz) Euler angles
        public float RotationX => (X / System.Int16.MaxValue) * 180;
        public float RotationY => (Y / System.Int16.MaxValue) * 180;
        public float RotationZ => (Z / System.Int16.MaxValue) * 180;

        public override void SerializeImpl(SerializerObject s)
        {
            X = s.Serialize<short>(X, name: nameof(X));
            Y = s.Serialize<short>(Y, name: nameof(Y));
            Z = s.Serialize<short>(Z, name: nameof(Z));
            if (Pre_Padding) s.SerializePadding(2);
        }
    }
}