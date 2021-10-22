namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// A vector with 20-bit values
    /// </summary>
    public class KlonoaVector20 : BinarySerializable
    {
        public KlonoaInt20 X { get; set; }
        public KlonoaInt20 Y { get; set; }
        public KlonoaInt20 Z { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            X = s.SerializeObject<KlonoaInt20>(X, name: nameof(X));
            Y = s.SerializeObject<KlonoaInt20>(Y, name: nameof(Y));
            Z = s.SerializeObject<KlonoaInt20>(Z, name: nameof(Z));
        }
    }
}