namespace BinarySerializer.Klonoa.BV
{
    /// <summary>
    /// Right-handed, Y-up vector.
    /// Signed fixed point (12:4) integers.
    /// </summary>
    public class VMD_Vector : BinarySerializable
    {
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            X = s.Serialize<short>(X, nameof(X));
            Y = s.Serialize<short>(Y, nameof(Y));
            Z = s.Serialize<short>(Z, nameof(Z));
        }
    }
}