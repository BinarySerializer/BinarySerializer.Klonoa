namespace BinarySerializer.KlonoaDTP
{
    public class CutsceneInstructionData_Default : BaseCutsceneInstructionData
    {
        public byte[] RawBytes { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            RawBytes = s.SerializeArray<byte>(RawBytes, 6, name: nameof(RawBytes));
        }
    }
}