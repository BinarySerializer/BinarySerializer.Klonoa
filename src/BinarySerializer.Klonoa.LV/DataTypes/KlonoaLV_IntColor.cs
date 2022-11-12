namespace BinarySerializer.Klonoa.LV
{
    public class KlonoaLV_IntColor : BaseColor
    {
        public KlonoaLV_IntColor() { }
        public KlonoaLV_IntColor(float r, float g, float b, float a = 1f) : base(r, g, b, a) { }

        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public int A { get; set; }

        public override float Red
        {
            get => R / 255f;
            set => R = (byte)(value * 255);
        }
        public override float Green
        {
            get => G / 255f;
            set => G = (byte)(value * 255);
        }
        public override float Blue
        {
            get => B / 255f;
            set => B = (byte)(value * 255);
        }

        public override void SerializeImpl(SerializerObject s)
        {
            R = s.Serialize<int>(R, name: nameof(R));
            G = s.Serialize<int>(G, name: nameof(G));
            B = s.Serialize<int>(B, name: nameof(B));
            s.SerializePadding(4, logIfNotNull: true);
        }
    }
}