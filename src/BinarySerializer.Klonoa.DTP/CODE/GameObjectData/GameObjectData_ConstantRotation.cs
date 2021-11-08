namespace BinarySerializer.Klonoa.DTP
{
    public class GameObjectData_ConstantRotation
    {
        public float? RotX { get; set; }
        public float? RotY { get; set; }
        public float? RotZ { get; set; }
        public float Min { get; set; } = -0x800;
        public float Length { get; set; } = 0x1000;
    }
}