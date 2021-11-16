namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_SetCameraAnimation : BaseCutsceneInstructionData
    {
        public byte AnimIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            AnimIndex = s.Serialize<byte>(AnimIndex, name: nameof(AnimIndex));
            s.SerializePadding(5, logIfNotNull: false);
        }
    }
}