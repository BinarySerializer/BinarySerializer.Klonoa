namespace BinarySerializer.Klonoa.DTP
{
    public class ModelAnimationKeyFrame
    {
        public bool Flag0 { get; set; }
        public bool Flag1 { get; set; }
        public bool Flag2 { get; set; }

        public byte DecrementCount { get; set; }
        public byte RepeatCount { get; set; }
        public int ValueChange { get; set; }
        public bool Sign { get; set; }

        public void Serialize(SerializerObject.SerializeBits64 bitFunc)
        {
            Flag0 = bitFunc(Flag0 ? 1 : 0, 1, name: nameof(Flag0)) != 0;

            if (!Flag0)
            {
                ValueChange = (int)bitFunc(ValueChange, 7, name: nameof(ValueChange)); // Shift left by 1
                return;
            }

            Flag1 = bitFunc(Flag1 ? 1 : 0, 1, name: nameof(Flag1)) != 0;

            if (!Flag1)
            {
                ValueChange = (int)bitFunc(ValueChange, 10, name: nameof(ValueChange));
                return;
            }

            Flag2 = bitFunc(Flag2 ? 1 : 0, 1, name: nameof(Flag2)) != 0;

            if (!Flag2)
            {
                RepeatCount = (byte)bitFunc(RepeatCount, 5, name: nameof(RepeatCount));
                ValueChange = (int)bitFunc(ValueChange, 8, name: nameof(ValueChange));
                return;
            }

            DecrementCount = (byte)bitFunc(DecrementCount, 5, name: nameof(DecrementCount));
            RepeatCount = (byte)bitFunc(RepeatCount, 5, name: nameof(RepeatCount));
            ValueChange = (int)bitFunc(ValueChange, 7, name: nameof(ValueChange));
            Sign = bitFunc(Sign ? 1 : 0, 1, name: nameof(Sign)) != 0;
        }
    }
}