namespace BinarySerializer.KlonoaDTP
{
    public class ObjRotation_File : BaseFile
    {
        public short RotationX { get; set; }
        public short RotationY { get; set; }
        public short RotationZ { get; set; }
        public short Short_06 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            RotationX = s.Serialize<short>(RotationX, name: nameof(RotationX));
            RotationY = s.Serialize<short>(RotationY, name: nameof(RotationY));
            RotationZ = s.Serialize<short>(RotationZ, name: nameof(RotationZ));
            Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
        }
    }
}