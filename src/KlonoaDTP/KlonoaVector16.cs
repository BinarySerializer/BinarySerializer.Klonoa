namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// A vector with 16-bit values
    /// </summary>
    public class KlonoaVector16 : BinarySerializable
    {
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            X = s.Serialize<short>(X, name: nameof(X));
            Y = s.Serialize<short>(Y, name: nameof(Y));
            Z = s.Serialize<short>(Z, name: nameof(Z));
        }
    }
}