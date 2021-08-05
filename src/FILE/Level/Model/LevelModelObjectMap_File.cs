namespace BinarySerializer.KlonoaDTP
{
    public class LevelModelObjectMap_File : BaseFile
    {
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public ushort Depth { get; set; }

        public short PivotX { get; set; }
        public short PivotY { get; set; }
        public short PivotZ { get; set; }

        public short[] ObjIndices { get; set; } // Indices to level model objects

        public override void SerializeImpl(SerializerObject s)
        {
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));
            Depth = s.Serialize<ushort>(Depth, name: nameof(Depth));
            s.SerializePadding(2, logIfNotNull: true);

            PivotX = s.Serialize<short>(PivotX, name: nameof(PivotX));
            PivotY = s.Serialize<short>(PivotY, name: nameof(PivotY));
            PivotZ = s.Serialize<short>(PivotZ, name: nameof(PivotZ));
            s.SerializePadding(2, logIfNotNull: true);

            ObjIndices = s.SerializeArray<short>(ObjIndices, Width * Height * Depth, name: nameof(ObjIndices));
        }
    }
}