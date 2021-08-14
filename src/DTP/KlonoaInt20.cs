namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// A signed 20-bit integer value which the game converts to a fixed point integer during runtime
    /// </summary>
    public class KlonoaInt20 : BinarySerializable
    {
        public int SerializedValue { get; set; }

        public FixedPointInt32 AsFixedPointInt => new FixedPointInt32()
        {
            Value = SerializedValue << 0x0C,
            Pre_PointPosition = 0x0C
        };

        public int Value => (SerializedValue << 0x0C) / 0x1000;

        public override void SerializeImpl(SerializerObject s)
        {
            SerializedValue = s.Serialize<int>(SerializedValue, name: nameof(SerializedValue));
            s.Log($"{nameof(Value)}: {Value}");
        }
    }
}