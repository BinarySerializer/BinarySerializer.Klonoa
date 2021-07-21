using System.Linq;

namespace BinarySerializer.KlonoaDTP
{
    public class LevelData2D : BinarySerializable
    {
        public Pointer[] DataPointers { get; set; }

        // Serialized from pointers
        public EnemyObjectIndexTables EnemyObjectIndexTables { get; set; }
        public EnemyObject[] EnemyObjects { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            DataPointers = s.SerializePointerArrayUntil(DataPointers, x => x == null, name: nameof(DataPointers));

            s.DoAt(DataPointers[0], () => EnemyObjectIndexTables = s.SerializeObject<EnemyObjectIndexTables>(EnemyObjectIndexTables, name: nameof(EnemyObjectIndexTables)));

            var enemyObjsCount = EnemyObjectIndexTables.IndexTables.SelectMany(x => x).Max() + 1;
            s.DoAt(DataPointers[40], () => EnemyObjects = s.SerializeObjectArray<EnemyObject>(EnemyObjects, enemyObjsCount, name: nameof(EnemyObjects)));
        }
    }
}