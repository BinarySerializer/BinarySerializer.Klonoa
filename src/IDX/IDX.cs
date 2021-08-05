namespace BinarySerializer.KlonoaDTP
{
    public class IDX : BinarySerializable
    {
        public LoaderConfiguration Pre_LoaderConfig { get; set; }

        public string Header { get; set; }
        public IDXEntry[] Entries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            if (Pre_LoaderConfig == null)
                s.LogWarning($"{nameof(Pre_LoaderConfig)} has not been set for the IDX data. The data will be parsed, but file types will not be determined.");

            Header = s.SerializeString(Header, 8, name: nameof(Header));

            Entries ??= new IDXEntry[25];

            for (int i = 0; i < Entries.Length; i++)
                Entries[i] = s.SerializeObject<IDXEntry>(Entries[i], x =>
                {
                    x.Pre_BlockIndex = i;
                    x.Pre_LoaderConfig = Pre_LoaderConfig;
                }, name: $"{nameof(Entries)}[{i}]");
        }
    }
}