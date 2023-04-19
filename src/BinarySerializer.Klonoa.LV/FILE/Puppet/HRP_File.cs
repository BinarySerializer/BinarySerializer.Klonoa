namespace BinarySerializer.Klonoa.LV
{
    public class HRP_File : BaseFile
    {
        public PuppetCommand[] Commands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Parse until an invalid command is found (icky solution for now)
            Commands = s.SerializeObjectArrayUntil<PuppetCommand>(Commands, (cmd) => cmd.Type == PuppetCommand.CommandType.Invalid, name: nameof(Commands));

            s.Goto(Offset + Pre_FileSize);
        }
    }
}