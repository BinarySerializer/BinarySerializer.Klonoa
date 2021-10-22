namespace BinarySerializer.Klonoa.DTP
{
    public class SectorModifiers : BinarySerializable
    {
        public ArchiveFile Pre_ObjectAssets { get; set; }

        public ModifierObject[] Modifiers { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Modifiers = s.SerializeObjectArrayUntil<ModifierObject>(
                obj: Modifiers,
                conditionCheckFunc: x => x.Short_00 == -1,
                onPreSerialize: x => x.Pre_ObjectAssets = Pre_ObjectAssets,
                name: nameof(Modifiers));
        }
    }
}