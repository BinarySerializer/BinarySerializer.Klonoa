namespace BinarySerializer.Klonoa.KH
{
    public class Cutscene_File : BaseFile
    {
        public CutsceneCommand[] Commands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // TODO: There are multiple groups of commands. Each group runs on a single frame (some commands tell it to wait a certain number of frames before the next group). How we do we know when we've reached the last group?
            Commands = s.SerializeObjectArrayUntil(Commands, x => x.EndOfFrame, name: nameof(Commands));
        }
    }
}