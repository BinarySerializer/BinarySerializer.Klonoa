namespace BinarySerializer.Klonoa.BV
{
    /// <summary>
    /// Vector with 10:11:11 values.
    /// </summary>
    public class VMDM_Vector : VMD_Vector
    {
        public short XBits { get; set; }
        public short YBits { get; set; }
        public short ZBits { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.DoBits<short>((b) => {
                XBits = b.SerializeBits<short>(XBits, 10, SignedNumberRepresentation.TwosComplement, name: nameof(XBits));
                YBits = b.SerializeBits<short>(YBits, 11, SignedNumberRepresentation.TwosComplement, name: nameof(YBits));
                ZBits = b.SerializeBits<short>(ZBits, 11, SignedNumberRepresentation.TwosComplement, name: nameof(ZBits));
            });

            X = (short)((int)XBits << 2);
            Y = (short)((int)YBits << 1);
            Z = (short)((int)ZBits << 1);
        }
    }
}