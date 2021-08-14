namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_SetCutsceneState : BaseCutsceneInstructionData
    {
        public int State { get; set; } // If 0 then the next frame it'll run the cutscene skip instructions

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializePadding(2, logIfNotNull: false);
            State = s.Serialize<int>(State, name: nameof(State));
        }
    }
}