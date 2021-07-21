namespace BinarySerializer.KlonoaDTP
{
    public class EnemyObjectIndexTables : BinarySerializable
    {
        // One for each movement path. Determines which objects get loaded when on each path.
        public Pointer[] TablePointers { get; set; }
        
        // Serialized from pointers
        public short[][] IndexTables { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            TablePointers = s.SerializePointerArray(TablePointers, 64, name: nameof(TablePointers));

            IndexTables ??= new short[TablePointers.Length][];

            for (int i = 0; i < IndexTables.Length; i++)
                s.DoAt(TablePointers[i], () => IndexTables[i] = s.SerializeArrayUntil<short>(IndexTables[i], x => x == -1, name: $"{nameof(IndexTables)}[{i}]"));
        }
    }
}