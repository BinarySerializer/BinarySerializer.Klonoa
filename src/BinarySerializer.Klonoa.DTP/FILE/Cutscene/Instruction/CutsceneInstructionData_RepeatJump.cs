namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_RepeatJump : BaseCutsceneInstructionData
    {
        public byte RepeatCount { get; set; }
        public int JumpOffset { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            RepeatCount = s.Serialize<byte>(RepeatCount, name: nameof(RepeatCount));
            s.SerializePadding(1, logIfNotNull: false);
            JumpOffset = s.Serialize<int>(JumpOffset, name: nameof(JumpOffset));
        }
    }
}