namespace BinarySerializer.KlonoaDTP
{
    public class SectorModifiers : BinarySerializable
    {
        public ArchiveFile Pre_AdditionalLevelFilePack { get; set; }

        public ModifierObject[] Modifiers { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Modifiers = s.SerializeObjectArrayUntil<ModifierObject>(
                obj: Modifiers,
                conditionCheckFunc: x => x.Short_00 == -1,
                onPreSerialize: x => x.Pre_AdditionalLevelFilePack = Pre_AdditionalLevelFilePack,
                name: nameof(Modifiers));
        }
    }
}