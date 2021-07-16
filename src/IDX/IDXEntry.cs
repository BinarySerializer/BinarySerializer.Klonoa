using System;

namespace BinarySerializer.KlonoaDTP
{
    public class IDXEntry : BinarySerializable
    {
        public int Pre_BlockIndex { get; set; }

        public uint DestinationPointer { get; set; } // The game copies the load commands pointer to this location
        public Pointer LoadCommandsPointer { get; set; }

        public IDXLoadCommand[] LoadCommands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            DestinationPointer = s.Serialize<uint>(DestinationPointer, name: nameof(DestinationPointer));
            s.Log($"{nameof(DestinationPointer)}: 0x{DestinationPointer:X8}");
            LoadCommandsPointer = s.SerializePointer(LoadCommandsPointer, name: nameof(LoadCommandsPointer));

            s.DoAt(LoadCommandsPointer, () =>
            {
                // Serialize load commands
                LoadCommands = s.SerializeObjectArrayUntil(LoadCommands, x => x.Type == 0, name: nameof(LoadCommands));

                // Set the file pointers
                Pointer p = null;
                var binFile = s.Context.GetFile(Loader.FilePath_BIN);

                for (var i = 0; i < LoadCommands.Length; i++)
                {
                    var cmd = LoadCommands[i];

                    // Seek
                    if (cmd.Type == 1)
                    {
                        p = cmd.BIN_Pointer;
                    }
                    // File
                    else if (cmd.Type == 2)
                    {
                        if (p == null)
                            throw new Exception($"File load command can not appear before a seek commands");

                        // Set the pointer
                        cmd.FILE_Pointer = p;

                        // Add a region for nicer pointer logging
                        binFile.AddRegion(p.FileOffset, cmd.FILE_Length, $"File_{Pre_BlockIndex}_{i}");

                        // Increment pointer by the file size
                        p += cmd.FILE_Length;
                    }
                }
            });
        }
    }
}