namespace BinarySerializer.KlonoaDTP
{
    public class Cutscene_File : BaseFile
    {
        // TODO: First instruction seems to be something else? The game parses it differently.
        public CutsceneInstruction[] Instructions { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Instructions = s.SerializeObjectArrayUntil(Instructions, x => x.Type == CutsceneInstruction.InstructionType.Terminator, onPreSerialize: x => x.Pre_ParamsBufferBaseOffset = Offset, name: nameof(Instructions));

            s.Goto(Offset + Pre_FileSize);
        }
    }
}