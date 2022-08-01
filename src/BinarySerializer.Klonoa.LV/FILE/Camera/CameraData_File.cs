namespace BinarySerializer.Klonoa.LV
{
    public class CameraData_File : BaseFile
    {
        public uint Version { get; set; } // Always 1
        public uint Count { get; set; }
        public uint[] RouteDataOffsets { get; set; }
        public CameraRouteData[][] RouteData { get; set; }
        public NormalCamera[] NormalCameras { get; set; }
        public FixedCamera[] FixedCameras { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Version = s.Serialize<uint>(Version, name: nameof(Version));
            Count = s.Serialize<uint>(Count, name: nameof(Count));
            RouteDataOffsets = s.SerializeArray<uint>(RouteDataOffsets, Count, name: nameof(RouteDataOffsets));
            RouteData ??= new CameraRouteData[Count][];
            for (int i = 0; i < Count; i++)
                RouteData[i] = s.SerializeObjectArrayUntil<CameraRouteData>(RouteData[i], (c) => (c.Flag & 64) != 0, name: $"{nameof(RouteData)}[{i}]");
            NormalCameras = s.SerializeObjectArrayUntil<NormalCamera>(NormalCameras, (c) => (c.Flag & 128) != 0, name: nameof(NormalCameras));
            FixedCameras = s.SerializeObjectArrayUntil<FixedCamera>(FixedCameras, (_) => (s.CurrentPointer + 36).CompareTo(Offset + Pre_FileSize) == 1, name: nameof(FixedCameras)); // Serialize to the end of the file

            s.Goto(Offset + Pre_FileSize);
        }
    }
}