using System.Linq;

namespace BinarySerializer.Klonoa.DTP
{
    public class LevelData2D : BinarySerializable
    {
        public Pointer Enemy_ObjectIndexTablesPointer { get; set; }
        public Pointer Pointer_04 { get; set; }
        public Pointer Enemy_Data_28_Pointer { get; set; }
        public Pointer Enemy_Data_01_Pointer { get; set; }
        public Pointer Enemy_Data_11_Pointer { get; set; }
        public Pointer Enemy_Data_15_Pointer { get; set; }
        public Pointer Enemy_Data_18_Pointer { get; set; }
        public Pointer Enemy_Data_14_Pointer { get; set; }
        public Pointer Enemy_Data_17_Pointer { get; set; }
        public Pointer Enemy_Data_35_Pointer { get; set; }
        public Pointer Enemy_Data_22_Pointer { get; set; }
        public Pointer Enemy_Data_24_Pointer { get; set; }
        public Pointer Enemy_Data_38_Pointer { get; set; }
        public Pointer Enemy_Data_02_Pointer { get; set; }
        public Pointer Enemy_Data_36_Pointer { get; set; }
        public Pointer Enemy_Data_26_Pointer { get; set; }
        public Pointer Enemy_Data_29_Pointer { get; set; }
        public Pointer Enemy_Data_32_Pointer { get; set; }
        public Pointer Enemy_Data_04_Pointer { get; set; }
        public Pointer Enemy_Data_05_Pointer { get; set; }
        public Pointer Enemy_Data_06_Pointer { get; set; }
        public Pointer Enemy_Data_13_Pointer { get; set; }
        public Pointer Enemy_Data_25_Pointer { get; set; }
        public Pointer Pointer_5C { get; set; }
        public Pointer Enemy_Data_39_Pointer { get; set; }
        public Pointer Enemy_Data_20_Pointer { get; set; }
        public Pointer Enemy_Data_37_Pointer { get; set; }
        public Pointer Enemy_Data_40_Pointer { get; set; }
        public Pointer Enemy_Data_16_Pointer { get; set; }
        public Pointer Enemy_Data_31_Pointer { get; set; }
        public Pointer Enemy_Data_34_Pointer { get; set; }
        public Pointer Enemy_Data_27_Pointer { get; set; }
        public Pointer Enemy_Data_12_Pointer { get; set; }
        public Pointer Enemy_Data_30_Pointer { get; set; }
        public Pointer Enemy_Data_03_Pointer { get; set; }
        public Pointer Enemy_Data_08_Pointer { get; set; }
        public Pointer Enemy_Data_33_Pointer { get; set; }
        public Pointer Enemy_Data_09_Pointer { get; set; }
        public Pointer Enemy_Data_07_Pointer { get; set; }
        public Pointer Pointer_9C { get; set; }
        public Pointer Enemy_ObjectsPointer { get; set; }
        public Pointer Enemy_SpawnObjectsPointer { get; set; }
        public Pointer Collectible_ObjectsPointer { get; set; }
        public Pointer Collectible_SectorIndicesPointer { get; set; }
        public Pointer Collectible_DreamStonesInfosPointer { get; set; }
        public Pointer Pointer_B4 { get; set; }
        public Pointer Pointer_B8 { get; set; }
        public Pointer Enemy_AdditionalDataPointer { get; set; }
        public Pointer Enemy_WaypointsPointer { get; set; }
        public Pointer Pointer_C4 { get; set; }

        // Enemies
        public EnemyObjectIndexTables Enemy_ObjectIndexTables { get; set; }
        public EnemyObject[] Enemy_Objects { get; set; }

        // Collectibles
        public ushort[] Collectible_SectorIndices { get; set; }
        public DreamStonesCollectiblesInfo[] Collectible_DreamStonesInfos { get; set; }
        public CollectibleObject[] Collectible_Objects { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize pointers
            Enemy_ObjectIndexTablesPointer = s.SerializePointer(Enemy_ObjectIndexTablesPointer, name: nameof(Enemy_ObjectIndexTablesPointer));
            Pointer_04 = s.SerializePointer(Pointer_04, name: nameof(Pointer_04));
            Enemy_Data_28_Pointer = s.SerializePointer(Enemy_Data_28_Pointer, name: nameof(Enemy_Data_28_Pointer));
            Enemy_Data_01_Pointer = s.SerializePointer(Enemy_Data_01_Pointer, name: nameof(Enemy_Data_01_Pointer));
            Enemy_Data_11_Pointer = s.SerializePointer(Enemy_Data_11_Pointer, name: nameof(Enemy_Data_11_Pointer));
            Enemy_Data_15_Pointer = s.SerializePointer(Enemy_Data_15_Pointer, name: nameof(Enemy_Data_15_Pointer));
            Enemy_Data_18_Pointer = s.SerializePointer(Enemy_Data_18_Pointer, name: nameof(Enemy_Data_18_Pointer));
            Enemy_Data_14_Pointer = s.SerializePointer(Enemy_Data_14_Pointer, name: nameof(Enemy_Data_14_Pointer));
            Enemy_Data_17_Pointer = s.SerializePointer(Enemy_Data_17_Pointer, name: nameof(Enemy_Data_17_Pointer));
            Enemy_Data_35_Pointer = s.SerializePointer(Enemy_Data_35_Pointer, name: nameof(Enemy_Data_35_Pointer));
            Enemy_Data_22_Pointer = s.SerializePointer(Enemy_Data_22_Pointer, name: nameof(Enemy_Data_22_Pointer));
            Enemy_Data_24_Pointer = s.SerializePointer(Enemy_Data_24_Pointer, name: nameof(Enemy_Data_24_Pointer));
            Enemy_Data_38_Pointer = s.SerializePointer(Enemy_Data_38_Pointer, name: nameof(Enemy_Data_38_Pointer));
            Enemy_Data_02_Pointer = s.SerializePointer(Enemy_Data_02_Pointer, name: nameof(Enemy_Data_02_Pointer));
            Enemy_Data_36_Pointer = s.SerializePointer(Enemy_Data_36_Pointer, name: nameof(Enemy_Data_36_Pointer));
            Enemy_Data_26_Pointer = s.SerializePointer(Enemy_Data_26_Pointer, name: nameof(Enemy_Data_26_Pointer));
            Enemy_Data_29_Pointer = s.SerializePointer(Enemy_Data_29_Pointer, name: nameof(Enemy_Data_29_Pointer));
            Enemy_Data_32_Pointer = s.SerializePointer(Enemy_Data_32_Pointer, name: nameof(Enemy_Data_32_Pointer));
            Enemy_Data_04_Pointer = s.SerializePointer(Enemy_Data_04_Pointer, name: nameof(Enemy_Data_04_Pointer));
            Enemy_Data_05_Pointer = s.SerializePointer(Enemy_Data_05_Pointer, name: nameof(Enemy_Data_05_Pointer));
            Enemy_Data_06_Pointer = s.SerializePointer(Enemy_Data_06_Pointer, name: nameof(Enemy_Data_06_Pointer));
            Enemy_Data_13_Pointer = s.SerializePointer(Enemy_Data_13_Pointer, name: nameof(Enemy_Data_13_Pointer));
            Enemy_Data_25_Pointer = s.SerializePointer(Enemy_Data_25_Pointer, name: nameof(Enemy_Data_25_Pointer));
            Pointer_5C = s.SerializePointer(Pointer_5C, name: nameof(Pointer_5C));
            Enemy_Data_39_Pointer = s.SerializePointer(Enemy_Data_39_Pointer, name: nameof(Enemy_Data_39_Pointer));
            Enemy_Data_20_Pointer = s.SerializePointer(Enemy_Data_20_Pointer, name: nameof(Enemy_Data_20_Pointer));
            Enemy_Data_37_Pointer = s.SerializePointer(Enemy_Data_37_Pointer, name: nameof(Enemy_Data_37_Pointer));
            Enemy_Data_40_Pointer = s.SerializePointer(Enemy_Data_40_Pointer, name: nameof(Enemy_Data_40_Pointer));
            Enemy_Data_16_Pointer = s.SerializePointer(Enemy_Data_16_Pointer, name: nameof(Enemy_Data_16_Pointer));
            Enemy_Data_31_Pointer = s.SerializePointer(Enemy_Data_31_Pointer, name: nameof(Enemy_Data_31_Pointer));
            Enemy_Data_34_Pointer = s.SerializePointer(Enemy_Data_34_Pointer, name: nameof(Enemy_Data_34_Pointer));
            Enemy_Data_27_Pointer = s.SerializePointer(Enemy_Data_27_Pointer, name: nameof(Enemy_Data_27_Pointer));
            Enemy_Data_12_Pointer = s.SerializePointer(Enemy_Data_12_Pointer, name: nameof(Enemy_Data_12_Pointer));
            Enemy_Data_30_Pointer = s.SerializePointer(Enemy_Data_30_Pointer, name: nameof(Enemy_Data_30_Pointer));
            Enemy_Data_03_Pointer = s.SerializePointer(Enemy_Data_03_Pointer, name: nameof(Enemy_Data_03_Pointer));
            Enemy_Data_08_Pointer = s.SerializePointer(Enemy_Data_08_Pointer, name: nameof(Enemy_Data_08_Pointer));
            Enemy_Data_33_Pointer = s.SerializePointer(Enemy_Data_33_Pointer, name: nameof(Enemy_Data_33_Pointer));
            Enemy_Data_09_Pointer = s.SerializePointer(Enemy_Data_09_Pointer, name: nameof(Enemy_Data_09_Pointer));
            Enemy_Data_07_Pointer = s.SerializePointer(Enemy_Data_07_Pointer, name: nameof(Enemy_Data_07_Pointer));
            Pointer_9C = s.SerializePointer(Pointer_9C, name: nameof(Pointer_9C));
            Enemy_ObjectsPointer = s.SerializePointer(Enemy_ObjectsPointer, name: nameof(Enemy_ObjectsPointer));
            Enemy_SpawnObjectsPointer = s.SerializePointer(Enemy_SpawnObjectsPointer, name: nameof(Enemy_SpawnObjectsPointer));
            Collectible_ObjectsPointer = s.SerializePointer(Collectible_ObjectsPointer, name: nameof(Collectible_ObjectsPointer));
            Collectible_SectorIndicesPointer = s.SerializePointer(Collectible_SectorIndicesPointer, name: nameof(Collectible_SectorIndicesPointer));
            Collectible_DreamStonesInfosPointer = s.SerializePointer(Collectible_DreamStonesInfosPointer, name: nameof(Collectible_DreamStonesInfosPointer));
            Pointer_B4 = s.SerializePointer(Pointer_B4, name: nameof(Pointer_B4));
            Pointer_B8 = s.SerializePointer(Pointer_B8, name: nameof(Pointer_B8));
            Enemy_AdditionalDataPointer = s.SerializePointer(Enemy_AdditionalDataPointer, name: nameof(Enemy_AdditionalDataPointer));
            Enemy_WaypointsPointer = s.SerializePointer(Enemy_WaypointsPointer, name: nameof(Enemy_WaypointsPointer));
            Pointer_C4 = s.SerializePointer(Pointer_C4, name: nameof(Pointer_C4));

            // Serialize enemies
            s.DoAt(Enemy_ObjectIndexTablesPointer, () => Enemy_ObjectIndexTables = s.SerializeObject<EnemyObjectIndexTables>(Enemy_ObjectIndexTables, name: nameof(Enemy_ObjectIndexTables)));

            var enemyIndices = Enemy_ObjectIndexTables.IndexTables.SelectMany(x => x).ToArray();
            var enemyObjsCount = enemyIndices.Any() ? enemyIndices.Max() + 1 : 0;
            s.DoAt(Enemy_ObjectsPointer, () => Enemy_Objects = s.SerializeObjectArray<EnemyObject>(Enemy_Objects, enemyObjsCount, x => x.Pre_LevelData2D = this, name: nameof(Enemy_Objects)));

            // Serialize collectibles
            s.DoAt(Collectible_SectorIndicesPointer, () => Collectible_SectorIndices = s.SerializeArray<ushort>(Collectible_SectorIndices, 11, name: nameof(Collectible_SectorIndices)));

            s.DoAt(Collectible_DreamStonesInfosPointer, () => Collectible_DreamStonesInfos = s.SerializeObjectArray<DreamStonesCollectiblesInfo>(Collectible_DreamStonesInfos, 10, name: nameof(Collectible_DreamStonesInfos)));

            var collectibleObjsCount = Collectible_SectorIndices[10];

            s.DoAt(Collectible_ObjectsPointer, () => Collectible_Objects = s.SerializeObjectArray<CollectibleObject>(Collectible_Objects, collectibleObjsCount, name: nameof(Collectible_Objects)));
        }
    }
}