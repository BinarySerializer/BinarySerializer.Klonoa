namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// VIF-unpacked V2-16 UV
    /// </summary>
    public class VIFGeometry_UV : BinarySerializable
    {
        public int U { get; set; }
        public int V { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            U = s.Serialize<int>(U, name: nameof(U));
            V = s.Serialize<int>(V, name: nameof(V));
            s.SerializePadding(4); // Indeterminate
            s.SerializePadding(4); // Indeterminate
        }
    }
}