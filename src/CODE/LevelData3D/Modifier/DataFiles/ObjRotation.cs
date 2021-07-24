namespace BinarySerializer.KlonoaDTP
{
    public class ObjRotation : BinarySerializable
    {
        public short RotationX { get; set; }
        public short RotationY { get; set; }
        public short RotationZ { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            RotationX = s.Serialize<short>(RotationX, name: nameof(RotationX));
            RotationY = s.Serialize<short>(RotationY, name: nameof(RotationY));
            RotationZ = s.Serialize<short>(RotationZ, name: nameof(RotationZ));
        }
    }
}