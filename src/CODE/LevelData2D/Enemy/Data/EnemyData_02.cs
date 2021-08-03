namespace BinarySerializer.KlonoaDTP
{
    public class EnemyData_02 : BaseEnemyData
    {
        public short Short_04 { get; set; }
        public short Short_06 { get; set; }
        public byte[] Bytes_08 { get; set; }
        public short SpawnObjectsIndex { get; set; }
        public short SpawnObjectsCount { get; set; }
        public short Short_14 { get; set; }
        public short Short_16 { get; set; }
        public short Short_18 { get; set; }
        public ushort Ushort_1A { get; set; }
        public short WaypointsIndex { get; set; }
        public short WaypointsCount { get; set; }
        public short Short_20 { get; set; }
        public byte[] Bytes_22 { get; set; }

        public EnemyObject[] SpawnObjects { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            DespawnDistance = s.Serialize<int>(DespawnDistance, name: nameof(DespawnDistance));
            Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
            Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
            Bytes_08 = s.SerializeArray<byte>(Bytes_08, 8, name: nameof(Bytes_08));
            SpawnObjectsIndex = s.Serialize<short>(SpawnObjectsIndex, name: nameof(SpawnObjectsIndex));
            SpawnObjectsCount = s.Serialize<short>(SpawnObjectsCount, name: nameof(SpawnObjectsCount));
            Short_14 = s.Serialize<short>(Short_14, name: nameof(Short_14));
            Short_16 = s.Serialize<short>(Short_16, name: nameof(Short_16));
            Short_18 = s.Serialize<short>(Short_18, name: nameof(Short_18));
            Ushort_1A = s.Serialize<ushort>(Ushort_1A, name: nameof(Ushort_1A));
            WaypointsIndex = s.Serialize<short>(WaypointsIndex, name: nameof(WaypointsIndex));
            WaypointsCount = s.Serialize<short>(WaypointsCount, name: nameof(WaypointsCount));
            Short_20 = s.Serialize<short>(Short_20, name: nameof(Short_20));
            Bytes_22 = s.SerializeArray<byte>(Bytes_22, 2, name: nameof(Bytes_22));

            if (WaypointsCount > 0)
                s.DoAt(Pre_DataPointers[48] + WaypointsIndex * 0xC, () => Waypoints = s.SerializeObjectArray<EnemyWaypoint>(Waypoints, WaypointsCount, name: nameof(Waypoints)));

            s.DoAt(Pre_DataPointers[41] + SpawnObjectsIndex * 0xC, () => SpawnObjects = s.SerializeObjectArray<EnemyObject>(SpawnObjects, SpawnObjectsCount, x =>
            {
                x.Pre_DataPointers = Pre_DataPointers;
                x.Pre_IsSpawnedObject = true;

                // Copy properties
                x.XPos = Pre_EnemyObj.XPos;
                x.YPos = Pre_EnemyObj.YPos;
                x.ZPos = Pre_EnemyObj.ZPos;
            }, name: nameof(SpawnObjects)));
        }
    }
}