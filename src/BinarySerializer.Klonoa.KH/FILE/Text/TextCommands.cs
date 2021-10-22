namespace BinarySerializer.Klonoa.KH
{
    public class TextCommands : BinarySerializable
    {
        public TextCommand[] Commands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Commands = s.SerializeObjectArrayUntil(Commands, x => x.Command == TextCommand.CommandType.End, name: nameof(Commands));
        }
    }
}