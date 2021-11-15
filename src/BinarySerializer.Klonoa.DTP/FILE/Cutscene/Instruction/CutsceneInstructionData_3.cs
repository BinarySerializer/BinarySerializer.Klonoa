namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_3 : BaseCutsceneInstructionData
    {
        public byte Flags { get; set; }
        public int Int_02 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
            s.SerializePadding(1, logIfNotNull: false);
            Int_02 = s.Serialize<int>(Int_02, name: nameof(Int_02));
        }
    }
}