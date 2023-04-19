namespace BinarySerializer.Klonoa.LV
{
    public class SFXTriangleStripIndex : BinarySerializable 
    {
        public ushort NormalIndex { get; set; }
        public ushort UVIndex { get; set; }
        public ushort VertexIndex { get; set; }
        public short Flag { get; set; }
        public bool StartTriangleStrip => Flag > 0;
        public bool EndSection => Flag == -1;

        public override void SerializeImpl(SerializerObject s)
        {
            NormalIndex = s.Serialize<ushort>(NormalIndex, name: nameof(NormalIndex));
            UVIndex = s.Serialize<ushort>(UVIndex, name: nameof(UVIndex));
            VertexIndex = s.Serialize<ushort>(VertexIndex, name: nameof(VertexIndex));
            Flag = s.Serialize<short>(Flag, name: nameof(Flag));
        }
    }
}