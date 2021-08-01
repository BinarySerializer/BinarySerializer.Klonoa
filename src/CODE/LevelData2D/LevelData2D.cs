using System.Linq;

namespace BinarySerializer.KlonoaDTP
{
    public class LevelData2D : BinarySerializable
    {
        public Pointer[] DataPointers { get; set; }

        // Enemies
        public EnemyObjectIndexTables EnemyObjectIndexTables { get; set; }
        public EnemyObject[] EnemyObjects { get; set; }

        // Collectibles
        public ushort[] CollectibleSectorIndices { get; set; }
        public DreamStonesCollectiblesInfo[] DreamStonesCollectibleInfos { get; set; }
        public CollectibleObject[] CollectibleObjects { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize pointer table
            DataPointers = s.SerializePointerArrayUntil(DataPointers, x => x == null, name: nameof(DataPointers));

            // Serialize enemies
            s.DoAt(DataPointers[0], () => EnemyObjectIndexTables = s.SerializeObject<EnemyObjectIndexTables>(EnemyObjectIndexTables, name: nameof(EnemyObjectIndexTables)));

            var enemyIndices = EnemyObjectIndexTables.IndexTables.SelectMany(x => x).ToArray();
            var enemyObjsCount = enemyIndices.Any() ? enemyIndices.Max() + 1 : 0;
            s.DoAt(DataPointers[40], () => EnemyObjects = s.SerializeObjectArray<EnemyObject>(EnemyObjects, enemyObjsCount, x => x.Pre_DataPointers = DataPointers, name: nameof(EnemyObjects)));

            // Serialize collectibles
            s.DoAt(DataPointers[43], () => CollectibleSectorIndices = s.SerializeArray<ushort>(CollectibleSectorIndices, 11, name: nameof(CollectibleSectorIndices)));

            s.DoAt(DataPointers[44], () => DreamStonesCollectibleInfos = s.SerializeObjectArray<DreamStonesCollectiblesInfo>(DreamStonesCollectibleInfos, 10, name: nameof(DreamStonesCollectibleInfos)));

            var collectibleObjsCount = CollectibleSectorIndices[10];

            s.DoAt(DataPointers[42], () => CollectibleObjects = s.SerializeObjectArray<CollectibleObject>(CollectibleObjects, collectibleObjsCount, name: nameof(CollectibleObjects)));
        }
    }
}