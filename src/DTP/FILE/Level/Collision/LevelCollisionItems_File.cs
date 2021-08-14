namespace BinarySerializer.Klonoa.DTP
{
    public class LevelCollisionItems_File : BaseFile
    {
        public LevelCollisionItem[] CollisionItems { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            CollisionItems = s.SerializeObjectArray<LevelCollisionItem>(CollisionItems, Pre_FileSize / 28, name: nameof(CollisionItems));
        }
    }
}