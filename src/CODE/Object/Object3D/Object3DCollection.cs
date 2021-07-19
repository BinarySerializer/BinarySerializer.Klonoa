namespace BinarySerializer.KlonoaDTP
{
    public class Object3DCollection : BinarySerializable
    {
        public ArchiveFile Pre_ObjectModelsDataPack { get; set; }

        public Object3D[] Objects { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Objects = s.SerializeObjectArrayUntil<Object3D>(
                obj: Objects,
                conditionCheckFunc: x => x.Short_00 == -1,
                onPreSerialize: x => x.Pre_ObjectModelsDataPack = Pre_ObjectModelsDataPack,
                name: nameof(Objects));
        }
    }
}