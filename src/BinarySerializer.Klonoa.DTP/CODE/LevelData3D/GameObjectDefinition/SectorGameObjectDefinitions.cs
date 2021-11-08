namespace BinarySerializer.Klonoa.DTP
{
    public class SectorGameObjectDefinitions : BinarySerializable
    {
        public ArchiveFile Pre_ObjectAssets { get; set; }

        public GameObjectDefinition[] ObjectsDefinitions { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjectsDefinitions = s.SerializeObjectArrayUntil<GameObjectDefinition>(
                obj: ObjectsDefinitions,
                conditionCheckFunc: x => x.Short_00 == -1,
                onPreSerialize: x => x.Pre_ObjectAssets = Pre_ObjectAssets,
                name: nameof(ObjectsDefinitions));
        }
    }
}