namespace BinarySerializer.Klonoa.DTP
{
    public class BackgroundGameObjects_File : BaseFile
    {
        public int ObjectsCount { get; set; }
        public BackgroundGameObject[] Objects { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjectsCount = s.Serialize<int>(ObjectsCount, name: nameof(ObjectsCount));
            Objects = s.SerializeObjectArray<BackgroundGameObject>(Objects, ObjectsCount, name: nameof(Objects));
        }
    }
}