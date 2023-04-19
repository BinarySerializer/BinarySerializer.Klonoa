namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// VIF-unpacked V3-8 color
    /// </summary>
    public class VPMMicroMemColor : BaseBytewiseRGBColor
    {
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
            R = s.Serialize<byte>(R, name: nameof(R));
            s.SerializePadding(3);
            G = s.Serialize<byte>(G, name: nameof(G));
            s.SerializePadding(3);
            B = s.Serialize<byte>(B, name: nameof(B));
            s.SerializePadding(3);
            s.SerializePadding(4); // Indeterminate
        }
    }
}