﻿namespace BinarySerializer.Klonoa.DTP
{
    public class EnemyData_02 : BaseEnemyData
    {
        public byte[] Bytes_08 { get; set; }
        public short SpawnObjectsIndex { get; set; }
        public short SpawnObjectsCount { get; set; }
        public short Short_14 { get; set; }
        public short Short_16 { get; set; }
        public short Short_18 { get; set; }
        public ushort Ushort_1A { get; set; }
        public short Short_20 { get; set; }
        public byte[] Bytes_22 { get; set; }

        public EnemyObject[] SpawnObjects { get; set; }

        protected override void SerializeData(SerializerObject s)
        {
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

            s.DoAt(Pre_LevelData2D.Enemy_SpawnObjectsPointer + SpawnObjectsIndex * 0xC, () => SpawnObjects = s.SerializeObjectArray<EnemyObject>(SpawnObjects, SpawnObjectsCount, x =>
            {
                x.Pre_LevelData2D = Pre_LevelData2D;
                x.Pre_IsSpawnedObject = true;

                // Copy properties
                x.Position = Pre_EnemyObj.Position;
            }, name: nameof(SpawnObjects)));
        }
    }
}