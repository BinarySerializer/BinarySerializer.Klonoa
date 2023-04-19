using System.Collections.Generic;
using System.IO;
using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class VPMSection : BinarySerializable
    {
        public Pointer Pre_Offset { get; set; }

        public ushort SectionSize { get; set; }
        public ushort Ushort_02 { get; set; }
        public Pointer CommandsPointer { get; set; }
        public VIF_Command[] Commands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SectionSize = s.Serialize<ushort>(SectionSize, name: nameof(SectionSize));
            Ushort_02 = s.Serialize<ushort>(Ushort_02, name: nameof(Ushort_02));
            CommandsPointer = s.SerializePointer(CommandsPointer, anchor: Pre_Offset, name: nameof(CommandsPointer));
            s.SerializePadding(8, logIfNotNull: true);
            long lastCommandOffset = CommandsPointer.FileOffset + SectionSize * 0x10 - 0x04;
            VIF_Parser parser = new VIF_Parser() { IsVIF1 = true };
            s.DoAt(CommandsPointer, () => {
                Commands = s.SerializeObjectArrayUntil<VIF_Command>(Commands, c => c.Offset.FileOffset >= lastCommandOffset, 
                    onPreSerialize: (x, _) => x.Pre_Parser = parser, name: nameof(Commands));
            });
        }

        public IEnumerable<VPMMicroMem> ParseBlocks(Context context, string key)
        {
            // Create a parser
            var parser = new VIF_Parser() { IsVIF1 = true, };
            int microProgramIndex = 0;

            VPMMicroMem ExecuteMicroProgram(uint programAddress) {
                parser.HasPendingChanges = false;
                byte[] microProgram = parser.GetCurrentBuffer();

                if (microProgram != null) {
                    string microProgramKey = $"{key}_{microProgramIndex}";
                    var memoryStream = new MemoryStream(microProgram);
                    microProgramIndex++;

                    try {
                        var file = new StreamFile(context, microProgramKey, memoryStream, endianness: Endian.Little);
                        context.AddFile(file);

                        uint tops = parser.TOPS * 16;

                        BinaryDeserializer s = context.Deserializer;
                        s.Goto(file.StartPointer + tops);

                        var block = s.SerializeObject<VPMMicroMem>(default, name: "VIFGeometry_Block", onPreSerialize: x => x.Pre_ProgramAddress = programAddress);

                        return block;
                    } finally {
                        memoryStream.Close();
                        context.RemoveFile(microProgramKey);
                    }
                }
                return null;
            }
            
            if (SectionSize > 2) { // Sections with a size of 2 don't seem to do anything
                // Enumerate every command
                foreach (VIF_Command command in Commands)
                {
                    if (parser.StartsNewMicroProgram(command))
                    {
                        if (parser.HasPendingChanges) {
                            VPMMicroMem block = ExecuteMicroProgram(command.VIFCode.IMMEDIATE);
                            if (block != null) yield return block;
                        }
                    }

                    parser.ExecuteCommand(command, executeFull: true);
                }
            }
        }
    }
}