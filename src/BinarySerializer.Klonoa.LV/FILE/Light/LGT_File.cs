namespace BinarySerializer.Klonoa.LV
{
    public class LGT_File : BaseFile
    {
        public uint Version { get; set; } // Always 1
        public uint Count { get; set; }
        public uint[] RouteDataOffsets { get; set; }
        public LightRouteData[][] RouteData { get; set; }
        public LightInfo[] Lights { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Version = s.Serialize<uint>(Version, name: nameof(Version));
            Count = s.Serialize<uint>(Count, name: nameof(Count));
            RouteDataOffsets = s.SerializeArray<uint>(RouteDataOffsets, Count, name: nameof(RouteDataOffsets));
            RouteData ??= new LightRouteData[Count][];
            for (int i = 0; i < Count; i++)
                RouteData[i] = s.SerializeObjectArrayUntil<LightRouteData>(RouteData[i], (c) => (c.Flag & 64) != 0, name: $"{nameof(RouteData)}[{i}]");
            Lights = s.SerializeObjectArrayUntil<LightInfo>(Lights, (_) => (s.CurrentPointer + 36).CompareTo(Offset + Pre_FileSize) == 1, name: nameof(Lights)); // Serialize to the end of the file

            s.Goto(Offset + Pre_FileSize);
        }
    }
}