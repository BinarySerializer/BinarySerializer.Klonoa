﻿namespace BinarySerializer.Klonoa.DTP
{
    public class MovementPathBlock : BinarySerializable
    {
        public FixedPointInt16 DirectionX { get; set; }
        public FixedPointInt16 DirectionY { get; set; }
        public FixedPointInt16 DirectionZ { get; set; }
        public short Short_06 { get; set; }
        public FixedPointInt32 XPos { get; set; }
        public FixedPointInt32 YPos { get; set; }
        public FixedPointInt32 ZPos { get; set; }
        public short Short_14 { get; set; }
        public short Short_16 { get; set; }
        public short BlockLength { get; set; } // 0x7ffe is a special case
        public bool IsEnd => BlockLength == 0x7fff;
        public short Short_1A { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            DirectionX = s.SerializeObject<FixedPointInt16>(DirectionX, x => x.Pre_PointPosition = 0x0C, name: nameof(DirectionX));
            DirectionY = s.SerializeObject<FixedPointInt16>(DirectionY, x => x.Pre_PointPosition = 0x0C, name: nameof(DirectionY));
            DirectionZ = s.SerializeObject<FixedPointInt16>(DirectionZ, x => x.Pre_PointPosition = 0x0C, name: nameof(DirectionZ));
            Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
            XPos = s.SerializeObject<FixedPointInt32>(XPos, x => x.Pre_PointPosition = 0x0C, name: nameof(XPos));
            YPos = s.SerializeObject<FixedPointInt32>(YPos, x => x.Pre_PointPosition = 0x0C, name: nameof(YPos));
            ZPos = s.SerializeObject<FixedPointInt32>(ZPos, x => x.Pre_PointPosition = 0x0C, name: nameof(ZPos));
            Short_14 = s.Serialize<short>(Short_14, name: nameof(Short_14));
            Short_16 = s.Serialize<short>(Short_16, name: nameof(Short_16));
            BlockLength = s.Serialize<short>(BlockLength, name: nameof(BlockLength));
            Short_1A = s.Serialize<short>(Short_1A, name: nameof(Short_1A));
        }
    }
}