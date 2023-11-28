namespace BinarySerializer.Klonoa.BV
{
    /// <summary>
    /// Defines UVs and the CLUT to use for each vertex in the triangle that uses this texcoord.<br/>
    /// The CLUT is contained in the image data in the TIM associated with this model.
    /// <c>ClutX</c> and <c>ClutY</c> defines the coordinates of the CLUT within the image.
    /// </summary>
    public class VMDG_Texcoord : BinarySerializable
    {
        public VMD_UV UV0 { get; set; }
        public ushort ClutX { get; set; }
        public ushort ClutY { get; set; }
        public VMD_UV UV1 { get; set; }
        public VMD_UV UV2 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            UV0 = s.SerializeObject<VMD_UV>(UV0, name: nameof(UV0));
            s.DoBits<short>((b) => {
                ClutX = b.SerializeBits<ushort>(ClutX, 6);
                ClutY = b.SerializeBits<ushort>(ClutY, 9);
                b.SerializePadding(1, logIfNotNull: true);
            });
            UV1 = s.SerializeObject<VMD_UV>(UV1, name: nameof(UV1));
            UV2 = s.SerializeObject<VMD_UV>(UV2, name: nameof(UV2));

        }
    }
}