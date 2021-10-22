namespace BinarySerializer.Klonoa.DTP
{
    public class CameraAnimationFrame : BinarySerializable
    {
        public bool Pre_IsRelative { get; set; }

        public int AbsolutePositionX { get; set; }
        public int AbsolutePositionY { get; set; }
        public int AbsolutePositionZ { get; set; }
        public short AbsoluteRotationX { get; set; }
        public short AbsoluteRotationY { get; set; }

        public sbyte RelativePositionX { get; set; }
        public sbyte RelativePositionY { get; set; }
        public sbyte RelativePositionZ { get; set; }
        public sbyte RelativeRotationX { get; set; }
        public sbyte RelativeRotationY { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            if (!Pre_IsRelative)
            {
                AbsolutePositionX = s.Serialize<int>(AbsolutePositionX, name: nameof(AbsolutePositionX));
                AbsolutePositionY = s.Serialize<int>(AbsolutePositionY, name: nameof(AbsolutePositionY));
                AbsolutePositionZ = s.Serialize<int>(AbsolutePositionZ, name: nameof(AbsolutePositionZ));
                AbsoluteRotationX = s.Serialize<short>(AbsoluteRotationX, name: nameof(AbsoluteRotationX));
                AbsoluteRotationY = s.Serialize<short>(AbsoluteRotationY, name: nameof(AbsoluteRotationY));
            }
            else
            {
                RelativePositionX = s.Serialize<sbyte>(RelativePositionX, name: nameof(RelativePositionX));
                RelativePositionY = s.Serialize<sbyte>(RelativePositionY, name: nameof(RelativePositionY));
                RelativePositionZ = s.Serialize<sbyte>(RelativePositionZ, name: nameof(RelativePositionZ));
                RelativeRotationX = s.Serialize<sbyte>(RelativeRotationX, name: nameof(RelativeRotationX));
                RelativeRotationY = s.Serialize<sbyte>(RelativeRotationY, name: nameof(RelativeRotationY));
            }
        }
    }
}