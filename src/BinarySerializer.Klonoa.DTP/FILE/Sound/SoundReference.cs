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
            s.SerializeBitValues<ushort>(bitFunc =>
            {
                Value_00 = bitFunc(Value_00, 4, name: nameof(Value_00));
                Value_04 = bitFunc(Value_04, 7, name: nameof(Value_04));
                Value_11 = bitFunc(Value_11 ? 1 : 0, 1, name: nameof(Value_11)) != 0;
                Value_12 = bitFunc(Value_12, 3, name: nameof(Value_12));
                Value_15 = bitFunc(Value_15 ? 1 : 0, 1, name: nameof(Value_15)) != 0;
            });
        }
    }
}