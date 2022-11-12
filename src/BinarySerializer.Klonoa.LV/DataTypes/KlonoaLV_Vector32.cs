namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// A vector with 32-bit values. Y and Z axes are inverted.
    /// </summary>
    public class KlonoaLV_Vector32 : BinarySerializable
    {
        public bool Pre_Padding { get; set; } = false;

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            X = s.Serialize<int>(X, name: nameof(X));
            Y = s.Serialize<int>(Y, name: nameof(Y));
            Z = s.Serialize<int>(Z, name: nameof(Z));
            if (Pre_Padding) s.SerializePadding(4);
        }
    }
}