namespace BinarySerializer.Klonoa.DTP
{
    public class IDX : BinarySerializable
    {
        public LoaderConfiguration_DTP Pre_LoaderConfig { get; set; }

        public string Header { get; set; }
        public IDXEntry[] Entries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            if (Pre_LoaderConfig == null)
                s.LogWarning($"{nameof(Pre_LoaderConfig)} has not been set for the IDX data. The data will be parsed, but file types will not be determined.");

            Header = s.SerializeString(Header, 8, name: nameof(Header));
            Entries = s.SerializeObjectArray<IDXEntry>(Entries, 25, x => x.Pre_LoaderConfig = Pre_LoaderConfig, name: nameof(Entries));
        }
    }
}