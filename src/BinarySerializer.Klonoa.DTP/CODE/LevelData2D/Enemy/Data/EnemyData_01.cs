namespace BinarySerializer.Klonoa.DTP
{
    public class EnemyData_01 : BaseEnemyData
    {
        public byte[] Bytes_08 { get; set; }
        public int Int_10 { get; set; }
        public short Short_14 { get; set; }
        public short Short_16 { get; set; }
        public byte[] Bytes_18 { get; set; }
        public short Short_1C { get; set; }
        public short Short_1E { get; set; }
        public byte[] Bytes_20 { get; set; }

        protected override void SerializeData(SerializerObject s)
        {
            Bytes_08 = s.SerializeArray<byte>(Bytes_08, 8, name: nameof(Bytes_08));
            Int_10 = s.Serialize<int>(Int_10, name: nameof(Int_10));
            Short_14 = s.Serialize<short>(Short_14, name: nameof(Short_14));
            Short_16 = s.Serialize<short>(Short_16, name: nameof(Short_16));
            Bytes_18 = s.SerializeArray<byte>(Bytes_18, 4, name: nameof(Bytes_18));
            Short_1C = s.Serialize<short>(Short_1C, name: nameof(Short_1C));
            Short_1E = s.Serialize<short>(Short_1E, name: nameof(Short_1E));
            Bytes_20 = s.SerializeArray<byte>(Bytes_20, 2, name: nameof(Bytes_20));
            WaypointsIndex = s.Serialize<short>(WaypointsIndex, name: nameof(WaypointsIndex));
            WaypointsCount = (short)(WaypointsIndex > -1 ? 1 : 0);
        }
    }
}