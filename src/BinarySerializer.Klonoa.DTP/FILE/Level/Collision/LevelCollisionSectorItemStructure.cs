namespace BinarySerializer.Klonoa.DTP
{
    public class LevelCollisionSectorItemStructure : BinarySerializable
    {
        public Pointer Pre_CollisionIndicesPointer { get; set; }

        public ushort CollisionIndicesOffset { get; set; }
        public ushort ZOffset { get; set; }

        public ushort[] CollisionIndices { get; set; } // To collision item files

        public override void SerializeImpl(SerializerObject s)
        {
            CollisionIndicesOffset = s.Serialize<ushort>(CollisionIndicesOffset, name: nameof(CollisionIndicesOffset));
            ZOffset = s.Serialize<ushort>(ZOffset, name: nameof(ZOffset));

            if (CollisionIndicesOffset != 0xFFFF)
                s.DoAt(Pre_CollisionIndicesPointer + CollisionIndicesOffset * 2, () => CollisionIndices = s.SerializeArrayUntil<ushort>(CollisionIndices, x => x == 0xFFFF, () => 0xFFFF, name: nameof(CollisionIndices)));
        }
    }
}