namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_MoveCutsceneObjTowardsRelativeObj : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }

        public short RelativeObjIndex { get; set; } // Klonoa if -1, do something else if 100
        public short Short_Param02 { get; set; }
        public short Short_Param04 { get; set; } // Value to divide position differences by

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: false);
            ParamsBufferOffset = s.Serialize<uint>(ParamsBufferOffset, name: nameof(ParamsBufferOffset));
            DoAtParams(s, () =>
            {
                RelativeObjIndex = s.Serialize<short>(RelativeObjIndex, name: nameof(RelativeObjIndex));
                Short_Param02 = s.Serialize<short>(Short_Param02, name: nameof(Short_Param02));
                Short_Param04 = s.Serialize<short>(Short_Param04, name: nameof(Short_Param04));
            });
        }
    }
}