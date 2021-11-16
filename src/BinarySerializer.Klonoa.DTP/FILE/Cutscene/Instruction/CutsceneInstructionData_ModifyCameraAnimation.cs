namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_ModifyCameraAnimation : BaseCutsceneInstructionData
    {
        public byte AnimIndex { get; set; }
        public int Int_02 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            AnimIndex = s.Serialize<byte>(AnimIndex, name: nameof(AnimIndex));
            s.SerializePadding(1, logIfNotNull: false);
            Int_02 = s.Serialize<int>(Int_02, name: nameof(Int_02));
        }
    }
}