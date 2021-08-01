namespace BinarySerializer.KlonoaDTP
{
    public abstract class BaseEnemyData : BinarySerializable
    {
        public Pointer[] Pre_DataPointers { get; set; }

        public int DespawnDistance { get; set; } = -1; // The distance from Klonoa before despawning
        public EnemyWaypoint[] Waypoints { get; set; } = new EnemyWaypoint[0];
    }
}