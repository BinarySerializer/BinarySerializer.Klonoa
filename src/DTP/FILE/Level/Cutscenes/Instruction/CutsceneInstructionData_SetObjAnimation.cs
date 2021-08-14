namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_SetObjAnimation : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }
        public int AnimIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: false);
            AnimIndex = s.Serialize<int>(AnimIndex, name: nameof(AnimIndex));
        }
    }
}