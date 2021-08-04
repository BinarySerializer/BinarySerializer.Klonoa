namespace BinarySerializer.KlonoaDTP
{
    public class BackgroundModifiers_File : BaseFile
    {
        public int ModifiersCount { get; set; }
        public BackgroundModifierObject[] Modifiers { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ModifiersCount = s.Serialize<int>(ModifiersCount, name: nameof(ModifiersCount));
            Modifiers = s.SerializeObjectArray<BackgroundModifierObject>(Modifiers, ModifiersCount, name: nameof(Modifiers));
        }
    }
}