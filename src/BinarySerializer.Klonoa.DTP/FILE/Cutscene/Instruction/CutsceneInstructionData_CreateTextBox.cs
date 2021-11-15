namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_CreateTextBox : BaseCutsceneInstructionData
    {
        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializePadding(6, logIfNotNull: false);
        }
    }
}