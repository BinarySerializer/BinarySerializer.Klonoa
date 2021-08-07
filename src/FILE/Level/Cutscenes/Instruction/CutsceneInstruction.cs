namespace BinarySerializer.KlonoaDTP
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

            switch (Type)
            {
                case InstructionType.DrawText:
                    serializeData<CutsceneInstructionData_DrawText>();
                    break;

                case InstructionType.SetObjAnimation:
                    serializeData<CutsceneInstructionData_SetObjAnimation>();
                    break;

                case InstructionType.CreateObj:
                    serializeData<CutsceneInstructionData_CreateObj>();
                    break;

                case InstructionType.ChangeSector:
                    serializeData<CutsceneInstructionData_ChangeSector>();
                    break;

                case InstructionType.SetObjPos:
                    serializeData<CutsceneInstructionData_SetObjPos>();
                    break;

                case InstructionType.CreateBackgroundObjects:
                case InstructionType.Terminator:
                case InstructionType.Special:
                default:
                    serializeData<CutsceneInstructionData_Default>();
                    break;
            }

            void serializeData<T>()
                where T : BaseCutsceneInstructionData, new()
            {
                Data = s.SerializeObject<T>((T)Data, onPreSerialize: x => x.Pre_ParamsBufferBaseOffset = Pre_ParamsBufferBaseOffset, name: nameof(Data));
            }
        }

        public enum InstructionType : short
        {
            DrawText = 0,

            SetObjAnimation = 7,
            CreateObj = 8,

            CreateBackgroundObjects = 10, // Creates two background objects for the cutscene of type 0xC (text and text border)

            ChangeSector = 16,
            SetObjPos = 17,

            Terminator = 777,
            Special = 999, // TODO: What does this do?
        }
    }
}