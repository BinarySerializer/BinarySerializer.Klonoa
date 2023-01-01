namespace BinarySerializer.Klonoa.LV
{
    public class CutsceneFile : BaseFile
    {
        public CutsceneCommand[] Commands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Parse until an invalid command is found (icky solution for now)
            Commands = s.SerializeObjectArrayUntil<CutsceneCommand>(Commands, (cmd) => cmd.Type == CutsceneCommand.CommandType.Invalid, name: nameof(Commands));

            s.Goto(Offset + Pre_FileSize);
        }
    }
}