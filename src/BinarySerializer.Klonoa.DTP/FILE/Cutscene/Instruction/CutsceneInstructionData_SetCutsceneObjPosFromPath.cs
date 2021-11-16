namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_SetCutsceneObjPosFromPath : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }

        public short MovementPathIndex { get; set; } // Movement path index, unless negative
        public short MovementPathDistance { get; set; }
        public short Short_Param04 { get; set; } // Horizontal flip

        public bool IsCutsceneObj => Short_Param04 < 0; // Otherwise it effects some primary object - Klonoa?
        public bool? FlipX => Short_Param04 switch
        {
            -2 => false,
            -3 => true,
            _ => null
        };

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: false);
            ParamsBufferOffset = s.Serialize<uint>(ParamsBufferOffset, name: nameof(ParamsBufferOffset));
            DoAtParams(s, () =>
            {
                MovementPathIndex = s.Serialize<short>(MovementPathIndex, name: nameof(MovementPathIndex));
                MovementPathDistance = s.Serialize<short>(MovementPathDistance, name: nameof(MovementPathDistance));
                Short_Param04 = s.Serialize<short>(Short_Param04, name: nameof(Short_Param04));
            });
        }
    }
}