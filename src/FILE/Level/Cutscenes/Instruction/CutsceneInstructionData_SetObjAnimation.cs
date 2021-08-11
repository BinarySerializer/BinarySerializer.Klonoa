namespace BinarySerializer.KlonoaDTP
{
    public class CutsceneInstructionData_SetObjAnimation : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }
        public byte Byte_01 { get; set; }
        public int AnimIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            Byte_01 = s.Serialize<byte>(Byte_01, name: nameof(Byte_01));
            AnimIndex = s.Serialize<int>(AnimIndex, name: nameof(AnimIndex));
        }
    }
}