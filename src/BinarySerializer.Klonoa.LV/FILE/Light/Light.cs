namespace BinarySerializer.Klonoa.LV
{
    public class Light : BinarySerializable
    {
        public short RotationX { get; set; }
        public short RotationY { get; set; }
        public SerializableColor Color { get; set; }
        public byte Flag { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            RotationX = s.Serialize<short>(RotationX, name: nameof(RotationX));
            RotationY = s.Serialize<short>(RotationY, name: nameof(RotationY));
            Color = s.SerializeInto<SerializableColor>(Color, BytewiseColor.RGB888, name: nameof(Color));
            Flag = s.Serialize<byte>(Flag, name: nameof(Flag));
            s.SerializePadding(2);
        }
    }
}