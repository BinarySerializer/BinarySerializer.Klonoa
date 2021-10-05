namespace BinarySerializer.Klonoa.DTP
{
    public class LevelCollisionGridItem : BinarySerializable
    {
        public Pointer Pre_CollisionGridItemStructuresPointer { get; set; }
        public Pointer Pre_CollisionIndicesPointer { get; set; }
        public ushort Pre_Depth { get; set; }

        public ushort CollisionGridItemStructuresOffset { get; set; }

        public LevelCollisionGridItemStructure[] Structure { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            CollisionGridItemStructuresOffset = s.Serialize<ushort>(CollisionGridItemStructuresOffset, name: nameof(CollisionGridItemStructuresOffset));

            s.DoAt(Pre_CollisionGridItemStructuresPointer + CollisionGridItemStructuresOffset * 4, () =>
            {
                var v = 0;

                Structure = s.SerializeObjectArrayUntil<LevelCollisionGridItemStructure>(Structure, x => (v += x.ZOffset) >= Pre_Depth, onPreSerialize: x => x.Pre_CollisionIndicesPointer = Pre_CollisionIndicesPointer, name: nameof(Structure));
            });
        }
    }
}