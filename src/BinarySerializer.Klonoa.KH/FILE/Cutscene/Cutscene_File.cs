using System.Linq;

namespace BinarySerializer.Klonoa.KH
{
    public class Cutscene_File : BaseFile
    {
        public CutsceneCommand[] Commands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // We could parse until we reach an end command, but we'll miss conditional code after that (referenced by offsets). So instead we
            // parse it as a linear command array until we reach the text (text is always at the end).
            Pointer firstTextPointer = null;
            Commands = s.SerializeObjectArrayUntil(Commands, x =>
            {
                var textPointer = x.TextCommands?.Offset ?? x.TextCommandsArray?.Offset;

                if (textPointer != null && (firstTextPointer == null || firstTextPointer.FileOffset > textPointer.FileOffset))
                    firstTextPointer = textPointer;

                return firstTextPointer != null && s.CurrentPointer.FileOffset >= firstTextPointer.FileOffset;
            }, name: nameof(Commands));

            // Verify the last command ends it to avoid overflow
            if (Commands.Last().Type != CutsceneCommand.CommandType.End_0 && 
                Commands.Last().Type != CutsceneCommand.CommandType.End_1 &&
                Commands.Last().Type != CutsceneCommand.CommandType.Return)
                throw new BinarySerializableException(this, "Invalid cutscene end command type");

            s.Goto(Offset + Pre_FileSize);
        }
    }
}