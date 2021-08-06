namespace BinarySerializer.KlonoaDTP
{
    public class CutsceneInstructionData_SetObjPos : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }

        public short PositionRelativeObjIndex { get; set; } // If less than 0 then it's absolute
        public short XPos { get; set; }
        public short YPos { get; set; }
        public short ZPos { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: true);
            ParamsBufferOffset = s.Serialize<uint>(ParamsBufferOffset, name: nameof(ParamsBufferOffset));
            DoAtParams(s, () =>
            {
                PositionRelativeObjIndex = s.Serialize<short>(PositionRelativeObjIndex, name: nameof(PositionRelativeObjIndex));
                XPos = s.Serialize<short>(XPos, name: nameof(XPos));
                YPos = s.Serialize<short>(YPos, name: nameof(YPos));
                ZPos = s.Serialize<short>(ZPos, name: nameof(ZPos));
            });
        }
    }
}