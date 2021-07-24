namespace BinarySerializer.KlonoaDTP
{
    public class ObjCollisionItems_File : BaseFile
    {
        public uint Count { get; set; }
        public LevelCollisionItem[] CollisionItems { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Count = s.Serialize<uint>(Count, name: nameof(Count));
            CollisionItems = s.SerializeObjectArray<LevelCollisionItem>(CollisionItems, Count, name: nameof(CollisionItems));
        }
    }
}