namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_ChangeSector : BaseCutsceneInstructionData
    {
        public byte SectorIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SectorIndex = s.Serialize<byte>(SectorIndex, name: nameof(SectorIndex));
            s.SerializePadding(5, logIfNotNull: false);
        }
    }
}