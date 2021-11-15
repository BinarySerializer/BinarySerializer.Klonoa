namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstruction : BinarySerializable
    {
        public Pointer Pre_ParamsBufferBaseOffset { get; set; }

        public uint FrameIndex { get; set; } // The index this instruction should be applied for
        public InstructionType Type { get; set; }
        public BaseCutsceneInstructionData Data { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FrameIndex = s.Serialize<uint>(FrameIndex, name: nameof(FrameIndex));
            Type = s.Serialize<InstructionType>(Type, name: nameof(Type));

            Data = Type switch
            {
                InstructionType.DrawText => serializeData<CutsceneInstructionData_DrawText>(),
                InstructionType.Instruction_2 => serializeData<CutsceneInstructionData_2>(),
                InstructionType.Instruction_3 => serializeData<CutsceneInstructionData_3>(),
                InstructionType.CreateObj3D => serializeData<CutsceneInstructionData_CreateObj3D>(),
                InstructionType.Instruction_5 => serializeData<CutsceneInstructionData_5>(),
                InstructionType.SetObjPosFromPath => serializeData<CutsceneInstructionData_SetObjPosFromPath>(),
                InstructionType.SetObj2DAnimation => serializeData<CutsceneInstructionData_SetObjAnimation>(),
                InstructionType.CreateObj2D => serializeData<CutsceneInstructionData_CreateObj>(),
                InstructionType.Instruction_9 => serializeData<CutsceneInstructionData_9>(),
                InstructionType.CreateTextBox => serializeData<CutsceneInstructionData_CreateTextBox>(),
                InstructionType.ChangeSector => serializeData<CutsceneInstructionData_ChangeSector>(),
                InstructionType.SetObjPos => serializeData<CutsceneInstructionData_SetObjPos>(),
                InstructionType.ClearVRAM => serializeData<CutsceneInstructionData_ClearVRAM>(),
                InstructionType.SetCutsceneState => serializeData<CutsceneInstructionData_SetCutsceneState>(),

                _ => serializeData<CutsceneInstructionData_Default>()
            };

            BaseCutsceneInstructionData serializeData<T>()
                where T : BaseCutsceneInstructionData, new()
            {
                return s.SerializeObject<T>((T)Data, onPreSerialize: x => x.Pre_ParamsBufferBaseOffset = Pre_ParamsBufferBaseOffset, name: nameof(Data));
            }
        }

        public enum InstructionType : short
        {
            DrawText = 0,
            // TODO: 1 is block specific
            Instruction_2 = 2,
            Instruction_3 = 3,
            CreateObj3D = 4,
            Instruction_5 = 5,
            SetObjPosFromPath = 6,
            SetObj2DAnimation = 7,
            CreateObj2D = 8,
            Instruction_9 = 9,
            CreateTextBox = 10, // Creates two background objects for the cutscene of type 0xC (text and text border)
            // TODO: 11 is block specific

            ChangeSector = 16,
            SetObjPos = 17,

            MoveObj = 23, // TODO: Parse, creates an animation where the object moves to a new position

            ClearVRAM = 27,

            SetCutsceneState = 31,

            Terminator = 777,
            Special = 999, // TODO: What does this do?
        }
    }
}