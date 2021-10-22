namespace BinarySerializer.Klonoa.LV
{
    public class GeometryTriangleStrip : BinarySerializable
    {
        public byte TriangleCount { get; set; }
        public byte LastTriangleStrip { get; set; } // 0x80 = true

        public override void SerializeImpl(SerializerObject s)
        {
            TriangleCount = s.Serialize<byte>(TriangleCount, name: nameof(TriangleCount));
            LastTriangleStrip = s.Serialize<byte>(LastTriangleStrip, name: nameof(LastTriangleStrip));
            s.SerializePadding(14); // Not sure what the other bytes are supposed to do
        }
    }
}