namespace BinarySerializer.Klonoa.DTP
{
    // TODO: Rename - EnemyRange? Seems to define some range they are in on the paths (not really the movement range though).
    public class EnemyWaypoint : BinarySerializable
    {
        public int StartPosition { get; set; }
        public int EndPosition { get; set; }
        public short MovementPathIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            StartPosition = s.Serialize<int>(StartPosition, name: nameof(StartPosition));
            EndPosition = s.Serialize<int>(EndPosition, name: nameof(EndPosition));
            MovementPathIndex = s.Serialize<short>(MovementPathIndex, name: nameof(MovementPathIndex));
            s.SerializePadding(2, logIfNotNull: true);
        }
    }
}