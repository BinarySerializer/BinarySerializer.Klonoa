using System.Collections.Generic;
using System.IO;
using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class VPMSection : BinarySerializable
    {
        public Pointer Pre_Offset { get; set; }

        public Chain_DMAtag DMAtag;
        public VIF_Command[] Commands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            DMAtag = s.SerializeObject<Chain_DMAtag>(DMAtag, name: nameof(DMAtag));
            s.SerializePadding(8, logIfNotNull: true);
            
            VIF_Parser parser = new VIF_Parser() { IsVIF1 = true };
            long commandsSize = DMAtag.QWC * 0x10;
            long currentSize = 0;
            s.DoAt(Pre_Offset + DMAtag.ADDR, () => {
                Commands = s.SerializeObjectArrayUntil<VIF_Command>(Commands, c => {
                    currentSize += c.SerializedSize;
                    return currentSize >= commandsSize;
                }, onPreSerialize: (x, _) => x.Pre_Parser = parser, name: nameof(Commands));
            });
        }

        public IEnumerable<VPMMicroMem> ParseMicroMemory(Context context, string key)
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

                        var block = s.SerializeObject<VPMMicroMem>(default, name: "VPM_MicroMem", onPreSerialize: x => x.Pre_ProgramAddress = programAddress);

                        return block;
                    } finally {
                        memoryStream.Close();
                        context.RemoveFile(microProgramKey);
                    }
                }
                return null;
            }
            
            if (DMAtag.QWC > 2) { // Sections with a size of 2 don't seem to do anything
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