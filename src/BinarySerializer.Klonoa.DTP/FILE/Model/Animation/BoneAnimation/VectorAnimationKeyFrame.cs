namespace BinarySerializer.Klonoa.DTP
{
    public class VectorAnimationKeyFrame
    {
        public CommandType Type { get; set; }

        public bool Flag0 { get; set; }
        public bool Flag1 { get; set; }
        public bool Flag2 { get; set; }

        public byte ChangeBy1Count { get; set; }
        public byte RepeatCount { get; set; }
        public int ValueChange { get; set; }
        public bool Sign { get; set; }

        public int ActualValueChange { get; set; }

        public void Serialize(SerializerObject.SerializeBits64 bitFunc)
        {
            Flag0 = bitFunc(Flag0 ? 1 : 0, 1, name: nameof(Flag0)) != 0;

            if (!Flag0)
            {
                ValueChange = (int)bitFunc(ValueChange, 7, name: nameof(ValueChange));
                ActualValueChange = (ValueChange << (0x18 + 1)) >> 0x17;
                Type = CommandType.Relative;
                return;
            }

            Flag1 = bitFunc(Flag1 ? 1 : 0, 1, name: nameof(Flag1)) != 0;

            if (!Flag1)
            {
                ValueChange = (int)bitFunc(ValueChange, 10, name: nameof(ValueChange));
                ActualValueChange = ValueChange << 2;
                Type = CommandType.Absolute;
                return;
            }

            Flag2 = bitFunc(Flag2 ? 1 : 0, 1, name: nameof(Flag2)) != 0;

            if (!Flag2)
            {
                RepeatCount = (byte)bitFunc(RepeatCount, 5, name: nameof(RepeatCount));
                ValueChange = (int)bitFunc(ValueChange, 8, name: nameof(ValueChange));
                ActualValueChange = (ValueChange << 0x18) >> 0x18;
                Type = CommandType.RelativeRepeat;
                return;
            }

            ChangeBy1Count = (byte)bitFunc(ChangeBy1Count, 5, name: nameof(ChangeBy1Count));
            RepeatCount = (byte)bitFunc(RepeatCount, 5, name: nameof(RepeatCount));
            ValueChange = (int)bitFunc(ValueChange, 7, name: nameof(ValueChange));
            Sign = bitFunc(Sign ? 1 : 0, 1, name: nameof(Sign)) != 0;

            ActualValueChange = ValueChange * (Sign ? -1 : 1);
            Type = CommandType.RelativeRepeatWithChangeBy1;
        }

        public enum CommandType
        {
            Relative,
            Absolute,
            RelativeRepeat,
            RelativeRepeatWithChangeBy1,
        }
    }
}