namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_CreateObj3D : BaseCutsceneInstructionData
    {
        public byte SecondaryType { get; set; }
        public int Int_02 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SecondaryType = s.Serialize<byte>(SecondaryType, name: nameof(SecondaryType));
            s.SerializePadding(1, logIfNotNull: false);
            Int_02 = s.Serialize<int>(Int_02, name: nameof(Int_02));
        }
    }
}