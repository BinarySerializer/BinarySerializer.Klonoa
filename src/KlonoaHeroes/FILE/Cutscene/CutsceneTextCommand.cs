namespace BinarySerializer.Klonoa.KH
{
    public class CutsceneTextCommand : BinarySerializable
    {
        public bool IsCommand => FontIndex < 0;

        public short FontIndex { get; set; }
        public CommandType Command => (CommandType)FontIndex;
        public short CommandArgument { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FontIndex = s.Serialize<short>(FontIndex, name: nameof(FontIndex));

            if (HasArgument(Command))
                CommandArgument = s.Serialize<short>(CommandArgument, name: nameof(CommandArgument));
        }

        public static bool HasArgument(CommandType cmd)
        {
            return cmd == CommandType.Speaker || 
                   cmd == CommandType.CMD_07 || 
                   cmd == CommandType.CMD_08 ||
                   cmd == CommandType.CMD_0C || 
                   cmd == CommandType.CMD_0D;
        }

        public enum CommandType
        {
            None = 0,
            End = -1,
            Clear = -2,
            Linebreak = -3,
            Speaker = -4,
            CMD_05 = -5,
            CMD_06 = -6,
            CMD_07 = -7,
            CMD_08 = -8,
            Prompt = -9,
            Pause = -10,
            CMD_0B = -11,
            CMD_0C = -12,
            CMD_0D = -13,
            CMD_0E = -14,
            CMD_0F = -15,
        }
    }
}