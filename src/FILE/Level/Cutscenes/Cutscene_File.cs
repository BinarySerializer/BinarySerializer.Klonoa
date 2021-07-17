namespace BinarySerializer.KlonoaDTP
{
    public class Cutscene_File : BaseFile
    {
        public CutsceneInstruction[] Instructions { get; set; }
        public byte[] ParamsBuffer { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Instructions = s.SerializeObjectArrayUntil(Instructions, x => x.InstructionType == 777, name: nameof(Instructions));
            ParamsBuffer = s.SerializeArray<byte>(ParamsBuffer, Pre_FileSize - (s.CurrentPointer - Offset), name: nameof(ParamsBuffer));
        }
    }
}