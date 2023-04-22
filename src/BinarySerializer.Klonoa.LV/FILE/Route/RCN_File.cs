namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// Defines how routes are linked in and between sectors.
    /// </summary>
    public class RCN_File : BaseFile
    {
        public uint Version { get; set; }
        public uint UInt_04 { get; set; }
        public uint UInt_08 { get; set; }
        public uint UInt_0C { get; set; }
        public RouteConnection[] Connections { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Version = s.Serialize<uint>(Version, name: nameof(Version)); // Always 2
            UInt_04 = s.Serialize<uint>(UInt_04, name: nameof(UInt_04));
            UInt_08 = s.Serialize<uint>(UInt_08, name: nameof(UInt_08));
            UInt_0C = s.Serialize<uint>(UInt_0C, name: nameof(UInt_0C));
            Connections = s.SerializeObjectArray<RouteConnection>(Connections, (Pre_FileSize - 0x10) / 0x20, name: nameof(Connections));
        }
    }
}