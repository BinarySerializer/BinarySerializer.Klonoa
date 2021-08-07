namespace BinarySerializer.KlonoaDTP
{
    public class CutsceneInstructionData_ChangeSector : BaseCutsceneInstructionData
    {
        public byte SectorIndex { get; set; }
        public int Int_02 { get; set; } // Unused? Padding?

        public override void SerializeImpl(SerializerObject s)
        {
            SectorIndex = s.Serialize<byte>(SectorIndex, name: nameof(SectorIndex));
            s.SerializePadding(1, logIfNotNull: true);
            Int_02 = s.Serialize<int>(Int_02, name: nameof(Int_02));
        }
    }
}