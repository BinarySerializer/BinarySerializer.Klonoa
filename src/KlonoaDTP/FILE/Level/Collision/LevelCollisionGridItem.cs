namespace BinarySerializer.Klonoa.DTP
{
    public class LevelCollisionGridItem : BinarySerializable
    {
        public Pointer Pre_CollisionGridItemStructuresPointer { get; set; }
        public Pointer Pre_CollisionIndicesPointer { get; set; }
        public ushort Pre_Depth { get; set; }

        public ushort StructuresOffset { get; set; }

        public LevelCollisionGridItemStructure[] Structures { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            StructuresOffset = s.Serialize<ushort>(StructuresOffset, name: nameof(StructuresOffset));

            s.DoAt(Pre_CollisionGridItemStructuresPointer + StructuresOffset * 4, () =>
            {
                var v = 0;

                Structures = s.SerializeObjectArrayUntil<LevelCollisionGridItemStructure>(Structures, x => (v += x.ZOffset) >= Pre_Depth, onPreSerialize: x => x.Pre_CollisionIndicesPointer = Pre_CollisionIndicesPointer, name: nameof(Structures));
            });
        }
    }
}