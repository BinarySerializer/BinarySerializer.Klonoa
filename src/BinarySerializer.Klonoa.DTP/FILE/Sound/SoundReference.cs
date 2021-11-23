namespace BinarySerializer.Klonoa.DTP
{
    public class SoundReference : BinarySerializable
    {
        public int Value_00 { get; set; }
        public int Value_04 { get; set; }
        public bool Value_11 { get; set; }
        public int Value_12 { get; set; }
        public bool Value_15 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.DoBits<ushort>(b =>
            {
                Value_00 = b.SerializeBits<int>(Value_00, 4, name: nameof(Value_00));
                Value_04 = b.SerializeBits<int>(Value_04, 7, name: nameof(Value_04));
                Value_11 = b.SerializeBits<int>(Value_11 ? 1 : 0, 1, name: nameof(Value_11)) != 0;
                Value_12 = b.SerializeBits<int>(Value_12, 3, name: nameof(Value_12));
                Value_15 = b.SerializeBits<int>(Value_15 ? 1 : 0, 1, name: nameof(Value_15)) != 0;
            });
        }
    }
}