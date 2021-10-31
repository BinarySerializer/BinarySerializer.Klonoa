namespace BinarySerializer.Klonoa.DTP
{
    public class SectorGameObjects3D : BinarySerializable
    {
        public ArchiveFile Pre_ObjectAssets { get; set; }

        public GameObject3D[] Objects { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Objects = s.SerializeObjectArrayUntil<GameObject3D>(
                obj: Objects,
                conditionCheckFunc: x => x.Short_00 == -1,
                onPreSerialize: x => x.Pre_ObjectAssets = Pre_ObjectAssets,
                name: nameof(Objects));
        }
    }
}