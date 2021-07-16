namespace BinarySerializer.KlonoaDTP
{
    public class IDX : BinarySerializable
    {
        public string Header { get; set; }
        public IDXEntry[] Entries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Header = s.SerializeString(Header, 8, name: nameof(Header));

            Entries ??= new IDXEntry[25];

            for (int i = 0; i < Entries.Length; i++)
                Entries[i] = s.SerializeObject<IDXEntry>(Entries[i], x => x.Pre_BlockIndex = i, name: $"{nameof(Entries)}[{i}]");
        }
    }
}