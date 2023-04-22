namespace BinarySerializer.Klonoa.LV
{
    public class RouteConnection : BinarySerializable
    {
        public short InRouteNum { get; set; }
        public short InDirection { get; set; }
        public short NextStage { get; set; } // Upper 8 bits are the vision index, lower 8 bits are the sector index
        public short OutRouteNum { get; set; }
        public int InRouteCount { get; set; }
        public int OutRouteCount { get; set; }
        public short OutDirection { get; set; }
        public short Mode { get; set; }
        public int InHeight { get; set; }
        public int OutHeight { get; set; }
        public short Flag { get; set; }
        public short FData { get; set; } // ?

        public override void SerializeImpl(SerializerObject s)
        {
            InRouteNum = s.Serialize<short>(InRouteNum, name: nameof(InRouteNum));
            InDirection = s.Serialize<short>(InDirection, name: nameof(InDirection));
            NextStage = s.Serialize<short>(NextStage, name: nameof(NextStage));
            OutRouteNum = s.Serialize<short>(OutRouteNum, name: nameof(OutRouteNum));
            InRouteCount = s.Serialize<int>(InRouteCount, name: nameof(InRouteCount));
            OutRouteCount = s.Serialize<int>(OutRouteCount, name: nameof(OutRouteCount));
            OutDirection = s.Serialize<short>(OutDirection, name: nameof(OutDirection));
            Mode = s.Serialize<short>(Mode, name: nameof(Mode));
            InHeight = s.Serialize<int>(InHeight, name: nameof(InHeight));
            OutHeight = s.Serialize<int>(OutHeight, name: nameof(OutHeight));
            Flag = s.Serialize<short>(Flag, name: nameof(Flag));
            FData = s.Serialize<short>(FData, name: nameof(FData));
        }
    }
}