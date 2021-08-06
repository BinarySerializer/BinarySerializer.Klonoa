namespace BinarySerializer.KlonoaDTP
{
    public class CutsceneInstructionData_SetObjAnimation : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }
        public int Short_02 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: true);
            Short_02 = s.Serialize<int>(Short_02, name: nameof(Short_02));
        }
    }
}