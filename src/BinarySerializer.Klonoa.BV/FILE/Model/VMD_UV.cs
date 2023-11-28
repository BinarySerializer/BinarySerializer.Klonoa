namespace BinarySerializer.Klonoa.BV
{
    public class VMD_UV : BinarySerializable
    {
        public byte U { get; set; }
        public byte V { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            U = s.Serialize<byte>(U, nameof(U));
            V = s.Serialize<byte>(V, nameof(V));
        }
    }
}