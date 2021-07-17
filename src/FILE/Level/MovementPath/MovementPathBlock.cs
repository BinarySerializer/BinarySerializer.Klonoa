namespace BinarySerializer.KlonoaDTP
{
    public class MovementPathBlock : BinarySerializable
    {
        public short Short_00 { get; set; }
        public short Short_02 { get; set; }
        public short Short_04 { get; set; }
        public short Short_06 { get; set; }
        public FixedPointInt32 XPos { get; set; }
        public FixedPointInt32 ZPos { get; set; }
        public FixedPointInt32 YPos { get; set; }
        public short Short_14 { get; set; }
        public short Short_16 { get; set; }
        public short Short_18 { get; set; }
        public short Short_1A { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
            Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
            Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
            Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
            XPos = s.SerializeObject<FixedPointInt32>(XPos, x => x.Pre_PointPosition = 9, name: nameof(XPos));
            ZPos = s.SerializeObject<FixedPointInt32>(ZPos, x => x.Pre_PointPosition = 9, name: nameof(ZPos));
            YPos = s.SerializeObject<FixedPointInt32>(YPos, x => x.Pre_PointPosition = 9, name: nameof(YPos));
            Short_14 = s.Serialize<short>(Short_14, name: nameof(Short_14));
            Short_16 = s.Serialize<short>(Short_16, name: nameof(Short_16));
            Short_18 = s.Serialize<short>(Short_18, name: nameof(Short_18));
            Short_1A = s.Serialize<short>(Short_1A, name: nameof(Short_1A));
        }
    }
}