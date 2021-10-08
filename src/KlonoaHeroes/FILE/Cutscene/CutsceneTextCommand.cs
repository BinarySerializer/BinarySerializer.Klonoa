namespace BinarySerializer.Klonoa.KH
{
    public class CutsceneTextCommand : BinarySerializable
    {
        public bool IsCommand => (FontIndex & 0x8000) != 0;

        public ushort FontIndex { get; set; }
        public CommandType Command { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FontIndex = s.Serialize<ushort>(FontIndex, name: nameof(FontIndex));

            if (IsCommand)
            {
                // TODO: Set command type and parse additional arguments if required
            }
        }

        public enum CommandType
        {
            CMD_00 = 0x0,
            CMD_01 = 0x1,
            CMD_02 = 0x2,
            CMD_03 = 0x3,
            CMD_04 = 0x4,
            CMD_05 = 0x5,
            CMD_06 = 0x6,
            CMD_07 = 0x7,
            CMD_08 = 0x8,
            CMD_09 = 0x9,
            CMD_0A = 0xA,
            CMD_0B = 0xB,
            CMD_0C = 0xC,
        }
    }
}