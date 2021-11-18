namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_DestroyCutsceneObj : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }
        public uint ActionType { get; set; } // 0 == Destroy now, 1 == Destroy some data, 2-4 == set some value which might destroy it on the next update

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: false);
            ActionType = s.Serialize<uint>(ActionType, name: nameof(ActionType));
        }
    }
}