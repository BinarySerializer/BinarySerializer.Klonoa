using System;

namespace BinarySerializer.Klonoa.LV
{
    public class BackgroundAnimation_File : BaseFile
    {
        public BackgroundAnimation_Command[] Commands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // All background animation files have a loop
            Commands = s.SerializeObjectArrayUntil<BackgroundAnimation_Command>(Commands, (cmd) => cmd.Type == BackgroundAnimation_CommandType.Jump && cmd.JumpOffset < 0, name: nameof(Commands));

            s.Goto(Offset + Pre_FileSize);
        }
    }
}