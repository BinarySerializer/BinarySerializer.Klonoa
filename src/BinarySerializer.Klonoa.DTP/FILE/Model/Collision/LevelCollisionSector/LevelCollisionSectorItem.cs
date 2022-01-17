namespace BinarySerializer.Klonoa.DTP
{
    public class LevelCollisionSectorItem : BinarySerializable
    {
        public Pointer Pre_LevelCollisionSectorItemStructuresPointer { get; set; }
        public Pointer Pre_CollisionIndicesPointer { get; set; }
        public ushort Pre_Depth { get; set; }

        public ushort StructuresOffset { get; set; }

        public LevelCollisionSectorItemStructure[] Structures { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            StructuresOffset = s.Serialize<ushort>(StructuresOffset, name: nameof(StructuresOffset));

            s.DoAt(Pre_LevelCollisionSectorItemStructuresPointer + StructuresOffset * 4, () =>
            {
                var v = 0;

                Structures = s.SerializeObjectArrayUntil<LevelCollisionSectorItemStructure>(Structures, x => (v += x.ZOffset) >= Pre_Depth, onPreSerialize: (x, _) => x.Pre_CollisionIndicesPointer = Pre_CollisionIndicesPointer, name: nameof(Structures));
            });
        }
    }
}