namespace BinarySerializer.Klonoa.LV
{
    public class LightRouteData : BinarySerializable
    {
        public uint Route { get; set; }
        public uint Height { get; set; }
        public byte Flag { get; set; }
        public byte Type { get; set; }
        public ushort Index { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Route = s.Serialize<uint>(Route, name: nameof(Route));
            Height = s.Serialize<uint>(Height, name: nameof(Height));
            Flag = s.Serialize<byte>(Flag, name: nameof(Flag));
            Type = s.Serialize<byte>(Type, name: nameof(Type));
            Index = s.Serialize<ushort>(Index, name: nameof(Index));
            s.SerializePadding(4, logIfNotNull: true);
        }
    }
}