namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_ModifyCutsceneObjRendering : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }
        public int ActionType { get; set; } // 0-4, 5-8, -1

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: false);
            ActionType = s.Serialize<int>(ActionType, name: nameof(ActionType));
        }
    }
}