namespace BinarySerializer.Klonoa.LV
{
    public class FixedCamera : BinarySerializable
    {
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int PositionZ { get; set; }
        public int AngleX { get; set; }
        public int AngleY { get; set; }
        public int AngleZ { get; set; }
        public int Proj { get; set; }
        public int Time { get; set; }
        public int Flag { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            PositionX = s.Serialize<int>(PositionX, name: nameof(PositionX));
            PositionY = s.Serialize<int>(PositionY, name: nameof(PositionY));
            PositionZ = s.Serialize<int>(PositionZ, name: nameof(PositionZ));
            AngleX = s.Serialize<int>(AngleX, name: nameof(AngleX));
            AngleY = s.Serialize<int>(AngleY, name: nameof(AngleY));
            AngleZ = s.Serialize<int>(AngleZ, name: nameof(AngleZ));
            Proj = s.Serialize<int>(Proj, name: nameof(Proj));
            Time = s.Serialize<int>(Time, name: nameof(Time));
            Flag = s.Serialize<int>(Flag, name: nameof(Flag));
        }
    }
}