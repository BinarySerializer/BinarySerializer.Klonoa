namespace BinarySerializer.Klonoa.KH
{
    public class TextCommands_File : BaseFile
    {
        public TextCommands TextCommands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            TextCommands = s.SerializeObject<TextCommands>(TextCommands, name: nameof(TextCommands));
        }
    }
}