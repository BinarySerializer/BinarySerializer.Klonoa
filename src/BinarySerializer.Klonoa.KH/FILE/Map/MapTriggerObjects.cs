namespace BinarySerializer.Klonoa.KH
{
    public class MapTriggerObjects : BinarySerializable
    {
        public byte[] Bytes_04 { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
        public short Short_14 { get; set; } // Depth?
        public ushort TriggersObjectsCount { get; set; }
        public uint TriggerIndexMapOffset { get; set; }
        public uint TriggerObjectsOffset { get; set; }

        public byte[] TriggerIndexMap { get; set; }
        public MapTriggerObject[] TriggerObjects { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("KE", 4);
            Bytes_04 = s.SerializeArray<byte>(Bytes_04, 12, name: nameof(Bytes_04));
            Width = s.Serialize<short>(Width, name: nameof(Width));
            Height = s.Serialize<short>(Height, name: nameof(Height));
            Short_14 = s.Serialize<short>(Short_14, name: nameof(Short_14));
            TriggersObjectsCount = s.Serialize<ushort>(TriggersObjectsCount, name: nameof(TriggersObjectsCount));
            TriggerIndexMapOffset = s.Serialize<uint>(TriggerIndexMapOffset, name: nameof(TriggerIndexMapOffset));
            TriggerObjectsOffset = s.Serialize<uint>(TriggerObjectsOffset, name: nameof(TriggerObjectsOffset));

            s.DoAt(Offset + TriggerIndexMapOffset, () => TriggerIndexMap = s.SerializeArray<byte>(TriggerIndexMap, (Width / 8) * (Height / 8), name: nameof(TriggerIndexMap)));
            s.DoAt(Offset + TriggerObjectsOffset, () => TriggerObjects = s.SerializeObjectArray<MapTriggerObject>(TriggerObjects, TriggersObjectsCount, name: nameof(TriggerObjects)));
        }
    }
}