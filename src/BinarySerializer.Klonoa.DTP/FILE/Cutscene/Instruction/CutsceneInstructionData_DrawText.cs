namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_DrawText : BaseCutsceneInstructionData
    {
        public byte CharacterName { get; set; }
        
        public TextCommand[] TextCommands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            CharacterName = s.Serialize<byte>(CharacterName, name: nameof(CharacterName));
            s.SerializePadding(1, logIfNotNull: false);
            ParamsBufferOffset = s.Serialize<uint>(ParamsBufferOffset, name: nameof(ParamsBufferOffset));
            DoAtParams(s, () =>
            {
                TextCommands = s.SerializeObjectArrayUntil(TextCommands, x => x.Type == TextCommand.CommandType.End, () => new TextCommand()
                {
                    Command = ~(short)TextCommand.CommandType.End
                }, name: nameof(TextCommands));
            });
        }

        public class TextCommand : BinarySerializable
        {
            public short Command { get; set; }
            public CommandType Type { get; set; }
            public short Param_Generic { get; set; }
            public SoundReference Param_SoundRef { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Command = s.Serialize<short>(Command, name: nameof(Command));

                if (Command >= 0)
                {
                    Type = CommandType.DrawChar;
                    return;
                }

                Type = (CommandType)~Command;
                s.Log($"{nameof(Type)}: {Type}");

                if (Type == CommandType.CMD_0 || Type == CommandType.Blank || Type == CommandType.Delay)
                    Param_Generic = s.Serialize<short>(Param_Generic, name: nameof(Param_Generic));
                else if (Type == CommandType.PlaySound)
                    Param_SoundRef = s.SerializeObject<SoundReference>(Param_SoundRef, name: nameof(Param_SoundRef));
            }

            public enum CommandType
            {
                DrawChar = -1, // Default

                CMD_0 = 0,
                LineBreak = 1,
                Blank = 2,
                Delay = 3, // Param is number of frames to delay
                End = 4,
                WaitForInput = 5, // Waits for input before continuing, then waits a frame
                Clear = 6,
                PlaySound = 7,
            }
        }
    }
}