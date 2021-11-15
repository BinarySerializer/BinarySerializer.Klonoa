namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_ClearVRAM : BaseCutsceneInstructionData
    {
        public byte Byte_00 { get; set; }
        public int Int_02 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Byte_00 = s.Serialize<byte>(Byte_00, name: nameof(Byte_00));
            s.SerializePadding(1, logIfNotNull: false);
            Int_02 = s.Serialize<int>(Int_02, name: nameof(Int_02));
        }
    }
}