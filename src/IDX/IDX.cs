namespace BinarySerializer.KlonoaDTP
{
    public class IDX : BinarySerializable
    {
        public string Header { get; set; }
        public IDXEntry[] Entries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Header = s.SerializeString(Header, 8, name: nameof(Header));
            Entries = s.SerializeObjectArray<IDXEntry>(Entries, 25, name: nameof(Entries));
        }
    }
}