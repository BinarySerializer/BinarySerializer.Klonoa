namespace BinarySerializer.Klonoa.LV
{
    public class RT_File : BaseFile
    {
        public uint Count { get; set; }
        public Pointer[] DataOffsets { get; set; }
        public Route[][] Routes { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Count = s.Serialize<uint>(Count, name: nameof(Count));
            DataOffsets = s.SerializePointerArray(DataOffsets, Count, name: nameof(DataOffsets), anchor: Offset);
            Routes ??= new Route[Count][];
            for (int i = 0; i < Count; i++)
                Routes[i] = s.SerializeObjectArrayUntil<Route>(Routes[i], (c) => c.Flag == 0x7FFF, name: $"{nameof(Routes)}[{i}]");

            s.Goto(Offset + Pre_FileSize);
        }
    }
}