namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_MoveCutsceneObjTowardsPos : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }
        public byte Byte_01 { get; set; } // Unused?

        public KlonoaVector16 Position { get; set; }
        public short Short_Param06 { get; set; }
        public short Short_Param08 { get; set; } // Value to divide position differences by

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            Byte_01 = s.Serialize<byte>(Byte_01, name: nameof(Byte_01));
            ParamsBufferOffset = s.Serialize<uint>(ParamsBufferOffset, name: nameof(ParamsBufferOffset));
            DoAtParams(s, () =>
            {
                Position = s.SerializeObject<KlonoaVector16>(Position, name: nameof(Position));
                Short_Param06 = s.Serialize<short>(Short_Param06, name: nameof(Short_Param06));
                Short_Param08 = s.Serialize<short>(Short_Param08, name: nameof(Short_Param08));
            });
        }
    }
}