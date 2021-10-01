namespace BinarySerializer.Klonoa.DTP
{
    public class ObjCollisionItems_File : BaseFile
    {
        public uint Count { get; set; }
        public LevelCollisionItem[] CollisionItems { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Count = s.Serialize<uint>(Count, name: nameof(Count));
            // The count doesn't always match for some reason...
            CollisionItems = s.SerializeObjectArray<LevelCollisionItem>(CollisionItems, (Pre_FileSize - 4) / 28, name: nameof(CollisionItems));
        }
    }
}