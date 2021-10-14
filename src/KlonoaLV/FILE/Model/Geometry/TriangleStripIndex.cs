namespace BinarySerializer.Klonoa.LV
{
    public class TriangleStripIndex : BinarySerializable 
    {
        public ushort NormalIndex { get; set; }
        public ushort UVIndex { get; set; }
        public ushort VertexIndex { get; set; }
        public short Extra { get; set; }
        public bool StartTriangleStrip => Extra > 0;
        public bool EndSection => Extra == -1;

        public override void SerializeImpl(SerializerObject s)
        {
            NormalIndex = s.Serialize<ushort>(NormalIndex, name: nameof(NormalIndex));
            UVIndex = s.Serialize<ushort>(UVIndex, name: nameof(UVIndex));
            VertexIndex = s.Serialize<ushort>(VertexIndex, name: nameof(VertexIndex));
            Extra = s.Serialize<short>(Extra, name: nameof(Extra));
        }
    }
}