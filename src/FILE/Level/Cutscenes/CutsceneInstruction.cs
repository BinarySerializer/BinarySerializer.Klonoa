namespace BinarySerializer.KlonoaDTP
{
    public class CutsceneInstruction : BinarySerializable
    {
        public uint FrameIndex { get; set; } // The index this instruction should be applied for
        public short InstructionType { get; set; } // 777 is terminator, 999 is special
        public byte Byte_06 { get; set; }
        public byte Byte_07 { get; set; } // Bool?
        public int Param { get; set; } // Depends on the type, can be an offset to more data in the file (the ParamsBuffer)

        public override void SerializeImpl(SerializerObject s)
        {
            FrameIndex = s.Serialize<uint>(FrameIndex, name: nameof(FrameIndex));
            InstructionType = s.Serialize<short>(InstructionType, name: nameof(InstructionType));
            Byte_06 = s.Serialize<byte>(Byte_06, name: nameof(Byte_06));
            Byte_07 = s.Serialize<byte>(Byte_07, name: nameof(Byte_07));
            Param = s.Serialize<int>(Param, name: nameof(Param));
        }
    }
}