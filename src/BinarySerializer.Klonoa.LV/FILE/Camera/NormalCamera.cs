namespace BinarySerializer.Klonoa.LV
{
    public class NormalCamera : BinarySerializable
    {
        public int Zoom { get; set; }
        public int AngleX { get; set; }
        public int AngleY { get; set; }
        public int AngleZ { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Proj { get; set; }
        public int Flag { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Zoom = s.Serialize<int>(Zoom, name: nameof(Zoom));
            AngleX = s.Serialize<int>(AngleX, name: nameof(AngleX));
            AngleY = s.Serialize<int>(AngleY, name: nameof(AngleY));
            AngleZ = s.Serialize<int>(AngleZ, name: nameof(AngleZ));
            PositionX = s.Serialize<int>(PositionX, name: nameof(PositionX));
            PositionY = s.Serialize<int>(PositionY, name: nameof(PositionY));
            Proj = s.Serialize<int>(Proj, name: nameof(Proj));
            Flag = s.Serialize<int>(Flag, name: nameof(Flag));
        }
    }
}