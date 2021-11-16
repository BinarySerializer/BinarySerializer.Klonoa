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
                InstructionType.SetCameraAnimation => serializeData<CutsceneInstructionData_SetCameraAnimation>(),
                InstructionType.Instruction_2 => serializeData<CutsceneInstructionData_2>(),
                InstructionType.Instruction_3 => serializeData<CutsceneInstructionData_3>(),
                InstructionType.CreateGameObject_12 => serializeData<CutsceneInstructionData_CreateGameObject_12>(),
                InstructionType.Instruction_5 => serializeData<CutsceneInstructionData_5>(),
                InstructionType.SetCutsceneObjPosFromPath => serializeData<CutsceneInstructionData_SetCutsceneObjPosFromPath>(),
                InstructionType.SetCutsceneObjAnimation => serializeData<CutsceneInstructionData_SetCutsceneObjAnimation>(),
                InstructionType.CreateCutsceneObj => serializeData<CutsceneInstructionData_CreateCutsceneObj>(),
                InstructionType.Instruction_9 => serializeData<CutsceneInstructionData_9>(),
                InstructionType.CreateTextBox => serializeData<CutsceneInstructionData_CreateTextBox>(),
                InstructionType.ModifyCameraAnimation => serializeData<CutsceneInstructionData_ModifyCameraAnimation>(),
                InstructionType.Instruction_12 => serializeData<CutsceneInstructionData_12>(),
                InstructionType.ModifyCutsceneObj_0 => serializeData<CutsceneInstructionData_ModifyCutsceneObj_0>(),
                InstructionType.CreateGameObject_4 => serializeData<CutsceneInstructionData_CreateGameObject_4>(),
                InstructionType.ModifyCutsceneObj_1 => serializeData<CutsceneInstructionData_ModifyCutsceneObj_1>(),
                InstructionType.ChangeSector => serializeData<CutsceneInstructionData_ChangeSector>(),
                InstructionType.SetCutsceneObjPos => serializeData<CutsceneInstructionData_SetCutsceneObjPos>(),
                InstructionType.Instruction_18 => serializeData<CutsceneInstructionData_18>(),
                InstructionType.MoveCutsceneObjTowardsRelativeObj => serializeData<CutsceneInstructionData_MoveCutsceneObjTowardsRelativeObj>(),
                InstructionType.Instruction_20 => serializeData<CutsceneInstructionData_20>(),
                InstructionType.FlipCutsceneObj => serializeData<CutsceneInstructionData_FlipCutsceneObj>(),
                InstructionType.Instruction_22 => serializeData<CutsceneInstructionData_22>(),
                InstructionType.MoveCutsceneObjTowardsPos => serializeData<CutsceneInstructionData_MoveCutsceneObjTowardsPos>(),
                InstructionType.RepeatJump => serializeData<CutsceneInstructionData_RepeatJump>(),
                InstructionType.MoveCamera => serializeData<CutsceneInstructionData_MoveCamera>(),
                InstructionType.Instruction_26 => serializeData<CutsceneInstructionData_26>(),
                InstructionType.ModifyVRAM => serializeData<CutsceneInstructionData_ModifyVRAM>(),
                InstructionType.Instruction_28 => serializeData<CutsceneInstructionData_28>(),
                InstructionType.Instruction_29 => serializeData<CutsceneInstructionData_29>(),
                InstructionType.Instruction_30 => serializeData<CutsceneInstructionData_30>(),
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
            SetCameraAnimation = 1, // Only used in block 3 in final game
            Instruction_2 = 2,
            Instruction_3 = 3,
            CreateGameObject_12 = 4, // Starts from secondary type 12
            Instruction_5 = 5,
            SetCutsceneObjPosFromPath = 6,
            SetCutsceneObjAnimation = 7,
            CreateCutsceneObj = 8,
            Instruction_9 = 9,
            CreateTextBox = 10, // Creates two background objects for the cutscene of type 0xC (text and text border)
            ModifyCameraAnimation = 11, // Only used in block 3 in final game
            Instruction_12 = 12, // Creates a background object and does something related to the camera and Klonoa
            ModifyCutsceneObj_0 = 13,
            CreateGameObject_4 = 14, // Starts from secondary type 4
            ModifyCutsceneObj_1 = 15,
            ChangeSector = 16,
            SetCutsceneObjPos = 17,
            Instruction_18 = 18,
            MoveCutsceneObjTowardsRelativeObj = 19,
            Instruction_20 = 20,
            FlipCutsceneObj = 21,
            Instruction_22 = 22,
            MoveCutsceneObjTowardsPos = 23,
            RepeatJump = 24, // Goes back to a previous buffer position x number of times, basically a for loop
            MoveCamera = 25,
            Instruction_26 = 26,
            ModifyVRAM = 27,
            Instruction_28 = 28,
            Instruction_29 = 29,
            Instruction_30 = 30,
            SetCutsceneState = 31,

            Terminator = 777,
            Special = 999, // TODO: What does this do?
        }
    }
}