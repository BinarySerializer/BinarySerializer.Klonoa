namespace BinarySerializer.KlonoaDTP
{
    public class ScenerySprites_File : BaseFile
    {
        public short PositionsCount { get; set; }
        public short Short_02 { get; set; }
        public Position[] Positions { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            PositionsCount = s.Serialize<short>(PositionsCount, name: nameof(PositionsCount));
            Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
            Positions = s.SerializeObjectArray<Position>(Positions, PositionsCount, name: nameof(Positions));
        }

        public class Position : BinarySerializable
        {
            public short XPos { get; set; }
            public short ZPos { get; set; }
            public short YPos { get; set; }

            public int ActualXPos => (XPos << 0xC) / 512;
            public int ActualZPos => (ZPos << 0xC) / 512;
            public int ActualYPos => (YPos << 0xC) / 512;

            public override void SerializeImpl(SerializerObject s)
            {
                XPos = s.Serialize<short>(XPos, name: nameof(XPos));
                ZPos = s.Serialize<short>(ZPos, name: nameof(ZPos));
                YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            }
        }
    }
}