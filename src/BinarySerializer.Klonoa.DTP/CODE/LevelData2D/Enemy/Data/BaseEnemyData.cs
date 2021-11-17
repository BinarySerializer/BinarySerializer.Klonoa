namespace BinarySerializer.Klonoa.DTP
{
    public abstract class BaseEnemyData : BinarySerializable
    {
        public EnemyObject Pre_EnemyObj { get; set; }
        public LevelData2D Pre_LevelData2D { get; set; }

        public int DespawnDistance { get; set; } = -1; // The distance from Klonoa before despawning

        public short WaypointsIndex { get; set; }
        public short WaypointsCount { get; set; }
        public EnemyWaypoint[] Waypoints { get; set; } = new EnemyWaypoint[0];

        public short MovementDataIndex { get; set; }
        public short MovementDataCount { get; set; }
        public EnemyMovementData[] MovementData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            DespawnDistance = s.Serialize<int>(DespawnDistance, name: nameof(DespawnDistance));
            MovementDataIndex = s.Serialize<short>(MovementDataIndex, name: nameof(MovementDataIndex));
            MovementDataCount = s.Serialize<short>(MovementDataCount, name: nameof(MovementDataCount));

            SerializeData(s);

            if (WaypointsCount > 0)
                s.DoAt(Pre_LevelData2D.Enemy_WaypointsPointer + WaypointsIndex * 0xC, () =>
                    Waypoints = s.SerializeObjectArray<EnemyWaypoint>(Waypoints, WaypointsCount, name: nameof(Waypoints)));

            if (MovementDataCount > 0)
                s.DoAt(Pre_LevelData2D.Enemy_AdditionalDataPointer + MovementDataIndex * 0x14, () =>
                    MovementData = s.SerializeObjectArray<EnemyMovementData>(MovementData, MovementDataCount, name: nameof(MovementData)));
        }

        protected abstract void SerializeData(SerializerObject s);
    }
}