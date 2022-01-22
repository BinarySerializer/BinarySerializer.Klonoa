namespace BinarySerializer.Klonoa.KH
{
    public class TextCommand : BinarySerializable
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
                   cmd == CommandType.BlankSpace ||
                   cmd == CommandType.Speed || 
                   cmd == CommandType.CMD_0D;
        }

        public enum CommandType
        {
            None = 0,
            End = -1,
            Clear = -2,
            Linebreak = -3, // Increment y by 0x10 and reset x
            Speaker = -4, // Draw speaker as two sprites and increment x by 0x20 + 8
            CMD_05 = -5,
            CMD_06 = -6,
            CMD_07 = -7,
            BlankSpace = -8, // Pixels specified by argument, if arg is 0 then default to 0x28
            Prompt = -9,
            Pause = -10,
            CMD_0B = -11,
            Speed = -12, // Set speed for each text character, in frames
            CMD_0D = -13,
            CMD_0E = -14,
            CMD_0F = -15,
        }
    }
}