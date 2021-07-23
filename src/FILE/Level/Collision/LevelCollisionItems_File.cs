namespace BinarySerializer.KlonoaDTP
{
    public class LevelCollisionItems_File : BaseFile
    {
        public LevelCollisionItem[] Items { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Items = s.SerializeObjectArray<LevelCollisionItem>(Items, Pre_FileSize / 28, name: nameof(Items));
        }
    }
}