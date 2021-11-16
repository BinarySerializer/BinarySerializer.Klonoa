namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_SetCutsceneObjPos : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }

        public short PositionRelativeObjIndex { get; set; } // If less than 0 then it's absolute
        public KlonoaVector16 Position { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: false);
            ParamsBufferOffset = s.Serialize<uint>(ParamsBufferOffset, name: nameof(ParamsBufferOffset));
            DoAtParams(s, () =>
            {
                PositionRelativeObjIndex = s.Serialize<short>(PositionRelativeObjIndex, name: nameof(PositionRelativeObjIndex));
                Position = s.SerializeObject<KlonoaVector16>(Position, name: nameof(Position));
            });
        }
    }
}