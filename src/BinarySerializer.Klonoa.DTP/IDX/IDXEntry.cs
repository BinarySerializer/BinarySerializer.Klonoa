namespace BinarySerializer.Klonoa.DTP
{
    public class IDXEntry : BinarySerializable
    {
        public uint DestinationPointer { get; set; } // The game copies the load commands pointer to this location
        public Pointer LoadCommandsPointer { get; set; }

        public IDXLoadCommand[] LoadCommands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            DestinationPointer = s.Serialize<uint>(DestinationPointer, name: nameof(DestinationPointer));
            s.Log("{0}: 0x{1:X8}", nameof(DestinationPointer), DestinationPointer);
            LoadCommandsPointer = s.SerializePointer(LoadCommandsPointer, name: nameof(LoadCommandsPointer));

            s.DoAt(LoadCommandsPointer, () =>
            {
                // Serialize load commands
                LoadCommands = s.SerializeObjectArrayUntil(LoadCommands, x => x.Type == 0, name: nameof(LoadCommands));
            });
        }
    }
}