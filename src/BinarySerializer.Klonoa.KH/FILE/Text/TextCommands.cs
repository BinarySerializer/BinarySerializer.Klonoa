namespace BinarySerializer.Klonoa.KH
{
    public class TextCommands : BinarySerializable
    {
        public int? Pre_MaxLength { get; set; }

        public TextCommand[] Commands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Commands = s.SerializeObjectArrayUntil(Commands, x => x.Command == TextCommand.CommandType.End, name: nameof(Commands));

            if (Pre_MaxLength != null)
                s.SerializePadding((int)(Pre_MaxLength - (s.CurrentFileOffset - Offset.FileOffset) / 2) * 2, logIfNotNull: true);
        }
    }
}