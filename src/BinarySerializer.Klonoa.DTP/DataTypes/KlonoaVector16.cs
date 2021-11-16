namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// A vector with 16-bit values
    /// </summary>
    public class KlonoaVector16 : BinarySerializable
    {
        public KlonoaVector16()
        {
            
        }

        public KlonoaVector16(short x, short y, short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            X = s.Serialize<short>(X, name: nameof(X));
            Y = s.Serialize<short>(Y, name: nameof(Y));
            Z = s.Serialize<short>(Z, name: nameof(Z));
        }

        public static KlonoaVector16 operator +(KlonoaVector16 v1, KlonoaVector16 v2) => 
            new KlonoaVector16((short)(v1.X + v2.X), (short)(v1.Y + v2.Y), (short)(v1.Z + v2.Z));

        public override bool UseShortLog => true;
        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}