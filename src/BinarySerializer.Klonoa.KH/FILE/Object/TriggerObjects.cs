namespace BinarySerializer.Klonoa.KH
{
    public class TriggerObjects : BinarySerializable
    {
        public byte[] Bytes_04 { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
        public short Short_14 { get; set; } // Depth?
        public ushort ObjectsCount { get; set; }
        public uint ObjectsIndexMapOffset { get; set; }
        public uint ObjectsOffset { get; set; }

        public byte[] ObjectsIndexMap { get; set; }
        public TriggerObject[] Objects { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("KE", 4);
            Bytes_04 = s.SerializeArray<byte>(Bytes_04, 12, name: nameof(Bytes_04));
            Width = s.Serialize<short>(Width, name: nameof(Width));
            Height = s.Serialize<short>(Height, name: nameof(Height));
            Short_14 = s.Serialize<short>(Short_14, name: nameof(Short_14));
            ObjectsCount = s.Serialize<ushort>(ObjectsCount, name: nameof(ObjectsCount));
            ObjectsIndexMapOffset = s.Serialize<uint>(ObjectsIndexMapOffset, name: nameof(ObjectsIndexMapOffset));
            ObjectsOffset = s.Serialize<uint>(ObjectsOffset, name: nameof(ObjectsOffset));

            s.DoAt(Offset + ObjectsIndexMapOffset, () => ObjectsIndexMap = s.SerializeArray<byte>(ObjectsIndexMap, (Width / 8) * (Height / 8), name: nameof(ObjectsIndexMap)));
            s.DoAt(Offset + ObjectsOffset, () => Objects = s.SerializeObjectArray<TriggerObject>(Objects, ObjectsCount, name: nameof(Objects)));
        }
    }
}