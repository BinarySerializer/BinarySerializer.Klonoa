namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_20 : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }

        public short Short_Param00 { get; set; }
        public short Short_Param02 { get; set; }
        public short Short_Param04 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: false);
            ParamsBufferOffset = s.Serialize<uint>(ParamsBufferOffset, name: nameof(ParamsBufferOffset));
            DoAtParams(s, () =>
            {
                Short_Param00 = s.Serialize<short>(Short_Param00, name: nameof(Short_Param00));
                Short_Param02 = s.Serialize<short>(Short_Param02, name: nameof(Short_Param02));
                Short_Param04 = s.Serialize<short>(Short_Param04, name: nameof(Short_Param04));
            });
        }
    }
}