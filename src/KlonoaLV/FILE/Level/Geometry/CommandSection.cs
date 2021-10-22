namespace BinarySerializer.Klonoa.LV
{
    public class CommandSection : BinarySerializable
    {
        public ushort SectionSize { get; set; }
        public ushort Ushort_02 { get; set; }
        public Pointer CommandsPointer { get; set; }
        public GeometryCommand[] Commands { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SectionSize = s.Serialize<ushort>(SectionSize, name: nameof(SectionSize));
            Ushort_02 = s.Serialize<ushort>(Ushort_02, name: nameof(Ushort_02));
            CommandsPointer = s.SerializePointer(CommandsPointer, name: nameof(CommandsPointer));
            s.SerializePadding(8, logIfNotNull: true);
            int ActualSectionSize = SectionSize * 0x10;
            long LastCommandOffset = CommandsPointer.FileOffset + ActualSectionSize - 0x10;
            if (SectionSize > 0x02)
                s.DoAt(CommandsPointer, () => Commands = s.SerializeObjectArrayUntil<GeometryCommand>(Commands, x => x.VIFCode.CMD == 0x11 && x.Offset.FileOffset >= LastCommandOffset, name: nameof(Commands)));
        }
    }
}