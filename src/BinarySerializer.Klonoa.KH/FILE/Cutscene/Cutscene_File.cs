namespace BinarySerializer.Klonoa.KH
{
    public class Cutscene_File : BaseFile
    {
        public CutsceneCommand[] Commands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Commands = s.SerializeObjectArrayUntil(Commands, x => x.Type == CutsceneCommand.CommandType.End_0 || x.Type == CutsceneCommand.CommandType.End_1, name: nameof(Commands));

            s.Goto(Offset + Pre_FileSize);
        }
    }
}