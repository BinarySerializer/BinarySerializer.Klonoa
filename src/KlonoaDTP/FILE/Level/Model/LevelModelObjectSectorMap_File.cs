namespace BinarySerializer.Klonoa.DTP
{
    public class LevelModelObjectSectorMap_File : BaseFile
    {
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public ushort Depth { get; set; }

        public KlonoaVector16 Pivot { get; set; }

        public short[] ObjIndices { get; set; } // Indices to level model objects in each sector

        public override void SerializeImpl(SerializerObject s)
        {
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));
            Depth = s.Serialize<ushort>(Depth, name: nameof(Depth));
            s.SerializePadding(2, logIfNotNull: true);

            Pivot = s.SerializeObject<KlonoaVector16>(Pivot, name: nameof(Pivot));
            s.SerializePadding(2, logIfNotNull: true);

            ObjIndices = s.SerializeArray<short>(ObjIndices, Width * Height * Depth, name: nameof(ObjIndices));
        }
    }
}