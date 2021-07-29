namespace BinarySerializer.KlonoaDTP
{
    public class LevelModelObjectMap_File : BaseFile
    {
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public ushort Depth { get; set; }

        // Pivot?
        public short Short_08 { get; set; }
        public short Short_0A { get; set; }
        public short Short_0C { get; set; }
        public short Short_0E { get; set; } // Padding?

        public short[] ObjIndices { get; set; } // Indices to level model objects

        public override void SerializeImpl(SerializerObject s)
        {
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));
            Depth = s.Serialize<ushort>(Depth, name: nameof(Depth));
            s.SerializePadding(2, logIfNotNull: true);

            Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
            Short_0A = s.Serialize<short>(Short_0A, name: nameof(Short_0A));
            Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
            Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));

            ObjIndices = s.SerializeArray<short>(ObjIndices, Width * Height * Depth, name: nameof(ObjIndices));
        }
    }
}