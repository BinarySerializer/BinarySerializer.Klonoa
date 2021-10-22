namespace BinarySerializer.Klonoa.DTP
{
    public class EnemyWaypoint : BinarySerializable
    {
        public int Int_00 { get; set; }
        public int Int_04 { get; set; }
        public short MovementPathIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Int_00 = s.Serialize<int>(Int_00, name: nameof(Int_00));
            Int_04 = s.Serialize<int>(Int_04, name: nameof(Int_04));
            MovementPathIndex = s.Serialize<short>(MovementPathIndex, name: nameof(MovementPathIndex));
            s.SerializePadding(2, logIfNotNull: true);
        }
    }
}