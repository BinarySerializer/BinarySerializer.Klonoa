using System.Linq;

namespace BinarySerializer.Klonoa.DTP
{
    public class LevelData2D : BinarySerializable
    {
        public Pointer Enemy_ObjectIndexTablesPointer { get; set; }
        public Pointer Pointer_04 { get; set; }
        public Pointer Pointer_08 { get; set; }
        public Pointer Enemy_Data_01_Pointer { get; set; }
        public Pointer Pointer_10 { get; set; }
        public Pointer Pointer_14 { get; set; }
        public Pointer Pointer_18 { get; set; }
        public Pointer Pointer_1C { get; set; }
        public Pointer Pointer_20 { get; set; }
        public Pointer Pointer_24 { get; set; }
        public Pointer Pointer_28 { get; set; }
        public Pointer Pointer_2C { get; set; }
        public Pointer Pointer_30 { get; set; }
        public Pointer Enemy_Data_02_Pointer { get; set; }
        public Pointer Pointer_38 { get; set; }
        public Pointer Pointer_3C { get; set; }
        public Pointer Pointer_40 { get; set; }
        public Pointer Pointer_44 { get; set; }
        public Pointer Pointer_48 { get; set; }
        public Pointer Pointer_4C { get; set; }
        public Pointer Pointer_50 { get; set; }
        public Pointer Pointer_54 { get; set; }
        public Pointer Pointer_58 { get; set; }
        public Pointer Pointer_5C { get; set; }
        public Pointer Pointer_60 { get; set; }
        public Pointer Pointer_64 { get; set; }
        public Pointer Pointer_68 { get; set; }
        public Pointer Pointer_6C { get; set; }
        public Pointer Pointer_70 { get; set; }
        public Pointer Pointer_74 { get; set; }
        public Pointer Pointer_78 { get; set; }
        public Pointer Pointer_7C { get; set; }
        public Pointer Pointer_80 { get; set; }
        public Pointer Pointer_84 { get; set; }
        public Pointer Pointer_88 { get; set; }
        public Pointer Pointer_8C { get; set; }
        public Pointer Pointer_90 { get; set; }
        public Pointer Pointer_94 { get; set; }
        public Pointer Pointer_98 { get; set; }
        public Pointer Pointer_9C { get; set; }
        public Pointer Enemy_ObjectsPointer { get; set; }
        public Pointer Enemy_SpawnObjectsPointer { get; set; }
        public Pointer Collectible_ObjectsPointer { get; set; }
        public Pointer Collectible_SectorIndicesPointer { get; set; }
        public Pointer Collectible_DreamStonesInfosPointer { get; set; }
        public Pointer Pointer_B4 { get; set; }
        public Pointer Pointer_B8 { get; set; }
        public Pointer Pointer_BC { get; set; }
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
            Pointer_08 = s.SerializePointer(Pointer_08, name: nameof(Pointer_08));
            Enemy_Data_01_Pointer = s.SerializePointer(Enemy_Data_01_Pointer, name: nameof(Enemy_Data_01_Pointer));
            Pointer_10 = s.SerializePointer(Pointer_10, name: nameof(Pointer_10));
            Pointer_14 = s.SerializePointer(Pointer_14, name: nameof(Pointer_14));
            Pointer_18 = s.SerializePointer(Pointer_18, name: nameof(Pointer_18));
            Pointer_1C = s.SerializePointer(Pointer_1C, name: nameof(Pointer_1C));
            Pointer_20 = s.SerializePointer(Pointer_20, name: nameof(Pointer_20));
            Pointer_24 = s.SerializePointer(Pointer_24, name: nameof(Pointer_24));
            Pointer_28 = s.SerializePointer(Pointer_28, name: nameof(Pointer_28));
            Pointer_2C = s.SerializePointer(Pointer_2C, name: nameof(Pointer_2C));
            Pointer_30 = s.SerializePointer(Pointer_30, name: nameof(Pointer_30));
            Enemy_Data_02_Pointer = s.SerializePointer(Enemy_Data_02_Pointer, name: nameof(Enemy_Data_02_Pointer));
            Pointer_38 = s.SerializePointer(Pointer_38, name: nameof(Pointer_38));
            Pointer_3C = s.SerializePointer(Pointer_3C, name: nameof(Pointer_3C));
            Pointer_40 = s.SerializePointer(Pointer_40, name: nameof(Pointer_40));
            Pointer_44 = s.SerializePointer(Pointer_44, name: nameof(Pointer_44));
            Pointer_48 = s.SerializePointer(Pointer_48, name: nameof(Pointer_48));
            Pointer_4C = s.SerializePointer(Pointer_4C, name: nameof(Pointer_4C));
            Pointer_50 = s.SerializePointer(Pointer_50, name: nameof(Pointer_50));
            Pointer_54 = s.SerializePointer(Pointer_54, name: nameof(Pointer_54));
            Pointer_58 = s.SerializePointer(Pointer_58, name: nameof(Pointer_58));
            Pointer_5C = s.SerializePointer(Pointer_5C, name: nameof(Pointer_5C));
            Pointer_60 = s.SerializePointer(Pointer_60, name: nameof(Pointer_60));
            Pointer_64 = s.SerializePointer(Pointer_64, name: nameof(Pointer_64));
            Pointer_68 = s.SerializePointer(Pointer_68, name: nameof(Pointer_68));
            Pointer_6C = s.SerializePointer(Pointer_6C, name: nameof(Pointer_6C));
            Pointer_70 = s.SerializePointer(Pointer_70, name: nameof(Pointer_70));
            Pointer_74 = s.SerializePointer(Pointer_74, name: nameof(Pointer_74));
            Pointer_78 = s.SerializePointer(Pointer_78, name: nameof(Pointer_78));
            Pointer_7C = s.SerializePointer(Pointer_7C, name: nameof(Pointer_7C));
            Pointer_80 = s.SerializePointer(Pointer_80, name: nameof(Pointer_80));
            Pointer_84 = s.SerializePointer(Pointer_84, name: nameof(Pointer_84));
            Pointer_88 = s.SerializePointer(Pointer_88, name: nameof(Pointer_88));
            Pointer_8C = s.SerializePointer(Pointer_8C, name: nameof(Pointer_8C));
            Pointer_90 = s.SerializePointer(Pointer_90, name: nameof(Pointer_90));
            Pointer_94 = s.SerializePointer(Pointer_94, name: nameof(Pointer_94));
            Pointer_98 = s.SerializePointer(Pointer_98, name: nameof(Pointer_98));
            Pointer_9C = s.SerializePointer(Pointer_9C, name: nameof(Pointer_9C));
            Enemy_ObjectsPointer = s.SerializePointer(Enemy_ObjectsPointer, name: nameof(Enemy_ObjectsPointer));
            Enemy_SpawnObjectsPointer = s.SerializePointer(Enemy_SpawnObjectsPointer, name: nameof(Enemy_SpawnObjectsPointer));
            Collectible_ObjectsPointer = s.SerializePointer(Collectible_ObjectsPointer, name: nameof(Collectible_ObjectsPointer));
            Collectible_SectorIndicesPointer = s.SerializePointer(Collectible_SectorIndicesPointer, name: nameof(Collectible_SectorIndicesPointer));
            Collectible_DreamStonesInfosPointer = s.SerializePointer(Collectible_DreamStonesInfosPointer, name: nameof(Collectible_DreamStonesInfosPointer));
            Pointer_B4 = s.SerializePointer(Pointer_B4, name: nameof(Pointer_B4));
            Pointer_B8 = s.SerializePointer(Pointer_B8, name: nameof(Pointer_B8));
            Pointer_BC = s.SerializePointer(Pointer_BC, name: nameof(Pointer_BC));
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