namespace BinarySerializer.Klonoa.KH
{
    public class EnemyObjects : BinarySerializable
    {
        public ushort ObjectsCount { get; set; }
        public byte[] Bytes_04 { get; set; }
        public uint ObjectsOffset { get; set; }

        public EnemyObject[] Objects { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("TK", 2);
            ObjectsCount = s.Serialize<ushort>(ObjectsCount, name: nameof(ObjectsCount));
            Bytes_04 = s.SerializeArray<byte>(Bytes_04, 8, name: nameof(Bytes_04));
            ObjectsOffset = s.Serialize<uint>(ObjectsOffset, name: nameof(ObjectsOffset));

            s.DoAt(Offset + ObjectsOffset, () => Objects = s.SerializeObjectArray<EnemyObject>(Objects, ObjectsCount, name: nameof(Objects)));
        }
    }
}