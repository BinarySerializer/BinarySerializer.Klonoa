namespace BinarySerializer.Klonoa.DTP
{
    public class GameObjectData_GeyserPlatformPosition : BinarySerializable
    {
        public KlonoaVector16 Position { get; set; }
        public ushort Ushort_06 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Position = s.SerializeObject<KlonoaVector16>(Position, name: nameof(Position));
            Ushort_06 = s.Serialize<ushort>(Ushort_06, name: nameof(Ushort_06));
        }
    }
}