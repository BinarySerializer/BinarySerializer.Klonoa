namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// A texture coordinate with 32-bit values used in models.
    /// </summary>
    public class KlonoaLV_UV32 : BinarySerializable
    {
        public int U { get; set; }
        public int V { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            U = s.Serialize<int>(U, name: nameof(U));
            V = s.Serialize<int>(V, name: nameof(V));
        }
    }
}