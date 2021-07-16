namespace BinarySerializer.KlonoaDTP
{
    public class ModelAnimationFrame : BinarySerializable
    {
        // TODO: Parse this. Contains rotation matrix values, used for light matrix and sprite clipping.

        public int Int_00 { get; set; }
        public int Int_04 { get; set; }
        public int Int_08 { get; set; }
        public int Int_0C { get; set; }
        public short Short_10 { get; set; }
        public short Short_12 { get; set; }
        public short Short_14 { get; set; }
        public short Short_16 { get; set; }
        public byte[] Bytes_18 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Int_00 = s.Serialize<int>(Int_00, name: nameof(Int_00));
            Int_04 = s.Serialize<int>(Int_04, name: nameof(Int_04));
            Int_08 = s.Serialize<int>(Int_08, name: nameof(Int_08));
            Int_0C = s.Serialize<int>(Int_0C, name: nameof(Int_0C));
            Short_10 = s.Serialize<short>(Short_10, name: nameof(Short_10));
            Short_12 = s.Serialize<short>(Short_12, name: nameof(Short_12));
            Short_14 = s.Serialize<short>(Short_14, name: nameof(Short_14));
            Short_16 = s.Serialize<short>(Short_16, name: nameof(Short_16));
            Bytes_18 = s.SerializeArray<byte>(Bytes_18, 4, name: nameof(Bytes_18));
        }
    }
}