namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// VIF-unpacked V3-8 color
    /// </summary>
    public class VIFGeometry_Color : BaseBytewiseRGBColor
    {
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