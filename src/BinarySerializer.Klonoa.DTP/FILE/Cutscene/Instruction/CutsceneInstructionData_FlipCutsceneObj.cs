namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_FlipCutsceneObj : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }
        public int FlipX { get; set; } // Flip if not 0

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: false);
            FlipX = s.Serialize<int>(FlipX, name: nameof(FlipX));
        }
    }
}