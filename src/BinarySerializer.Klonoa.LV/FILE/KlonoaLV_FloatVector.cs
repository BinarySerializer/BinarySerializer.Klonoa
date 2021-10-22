namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// A vector with 32-bit float values. Y and Z axes are inverted.
    /// </summary>
    public class KlonoaLV_FloatVector : BinarySerializable
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            X = s.Serialize<float>(X, name: nameof(X));
            Y = s.Serialize<float>(Y, name: nameof(Y));
            Z = s.Serialize<float>(Z, name: nameof(Z));
        }
    }
}