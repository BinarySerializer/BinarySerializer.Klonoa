namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// VIF-unpacked V3-16 vertex
    /// </summary>
    public class VPMMicroMemVertex : BinarySerializable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            X = s.Serialize<int>(X, name: nameof(X));
            Y = s.Serialize<int>(Y, name: nameof(Y));
            Z = s.Serialize<int>(Z, name: nameof(Z));
            s.SerializePadding(4); // Indeterminate
        }
    }
}