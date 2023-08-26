namespace BinarySerializer.Klonoa.LV
{
    public class CurveMirrorHeader : BinarySerializable
    {
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float Length { get; set; }
        
        public override void SerializeImpl(SerializerObject s)
        {
            RotationX = s.Serialize<float>(RotationX, name: nameof(RotationX));
            RotationY = s.Serialize<float>(RotationY, name: nameof(RotationY));
            Length = s.Serialize<float>(Length, name: nameof(Length));
        }
    }
}