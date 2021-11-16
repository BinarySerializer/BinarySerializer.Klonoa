namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_28 : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }
        public int Int_02 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: false);
            Int_02 = s.Serialize<int>(Int_02, name: nameof(Int_02));
        }
    }
}