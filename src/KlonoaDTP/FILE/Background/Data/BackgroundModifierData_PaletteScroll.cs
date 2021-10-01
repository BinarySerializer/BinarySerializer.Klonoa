namespace BinarySerializer.Klonoa.DTP
{
    public class BackgroundModifierData_PaletteScroll : BinarySerializable
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public int Speed { get; set; } // In frames

        public override void SerializeImpl(SerializerObject s)
        {
            XPosition = s.Serialize<int>(XPosition, name: nameof(XPosition));
            YPosition = s.Serialize<int>(YPosition, name: nameof(YPosition));
            StartIndex = s.Serialize<int>(StartIndex, name: nameof(StartIndex));
            Length = s.Serialize<int>(Length, name: nameof(Length));
            Speed = s.Serialize<int>(Speed, name: nameof(Speed));
        }
    }
}