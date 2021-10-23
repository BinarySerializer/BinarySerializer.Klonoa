namespace BinarySerializer.Klonoa.KH
{
    public class KlonoaSettings_KH : KlonoaSettings
    {
        public override KlonoaGameVersion Version => KlonoaGameVersion.KH;

        /// <summary>
        /// If set then only this map will be serialized. If null then all maps will be serialized.
        /// </summary>
        public MapID SerializeMap { get; set; }

        public class MapID
        {
            public MapID(int id1, int id2, int id3)
            {
                ID1 = id1;
                ID2 = id2;
                ID3 = id3;
            }

            public int ID1 { get; }
            public int ID2 { get; }
            public int ID3 { get; }
        }
    }
}