namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_ModifyCutsceneObj_0 : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }
        public uint ActionType { get; set; } // 0-4

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: false);
            ActionType = s.Serialize<uint>(ActionType, name: nameof(ActionType));
        }
    }
}