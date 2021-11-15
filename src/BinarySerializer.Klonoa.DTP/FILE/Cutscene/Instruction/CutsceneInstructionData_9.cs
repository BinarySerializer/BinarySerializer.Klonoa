namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_9 : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(5, logIfNotNull: false);
        }
    }
}