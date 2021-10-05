namespace BinarySerializer.Klonoa.DTP
{
    public class CollisionItems_File : BaseFile
    {
        public CollisionItem[] CollisionItems { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            CollisionItems = s.SerializeObjectArray<CollisionItem>(CollisionItems, Pre_FileSize / 28, name: nameof(CollisionItems));
        }
    }
}