namespace BinarySerializer.KlonoaDTP
{
    public class CutsceneInstructionData_CreateObj : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }
        public short Short_02 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: true);
            Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
            s.SerializePadding(2, logIfNotNull: true);
        }
    }
}