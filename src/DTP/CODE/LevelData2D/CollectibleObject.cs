namespace BinarySerializer.Klonoa.DTP
{
    public class CollectibleObject : BinarySerializable
    {
        public KlonoaInt20 XPos { get; set; }
        public KlonoaInt20 YPos { get; set; }
        public KlonoaInt20 ZPos { get; set; }

        public PrimaryObjectType PrimaryType => PrimaryObjectType.Collectible_2D;
        public short SecondaryType { get; set; }

        public short Short_0E { get; set; }
        public ushort GlobalSectorIndex { get; set; }
        public ushort Ushort_12 { get; set; }
        public ushort Ushort_14 { get; set; }
        public short Short_16 { get; set; }
        public ushort Ushort_18 { get; set; }
        public ushort Ushort_1A { get; set; }
        public ushort Ushort_1C { get; set; }
        public short MovementPath { get; set; } // If -1 then the positions are absolute
        public int MovementPathPosition { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            XPos = s.SerializeObject<KlonoaInt20>(XPos, name: nameof(XPos));
            YPos = s.SerializeObject<KlonoaInt20>(YPos, name: nameof(YPos));
            ZPos = s.SerializeObject<KlonoaInt20>(ZPos, name: nameof(ZPos));

            SecondaryType = s.Serialize<short>(SecondaryType, name: nameof(SecondaryType));
            Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));
            GlobalSectorIndex = s.Serialize<ushort>(GlobalSectorIndex, name: nameof(GlobalSectorIndex));
            Ushort_12 = s.Serialize<ushort>(Ushort_12, name: nameof(Ushort_12));
            Ushort_14 = s.Serialize<ushort>(Ushort_14, name: nameof(Ushort_14));
            Short_16 = s.Serialize<short>(Short_16, name: nameof(Short_16));
            Ushort_18 = s.Serialize<ushort>(Ushort_18, name: nameof(Ushort_18));
            Ushort_1A = s.Serialize<ushort>(Ushort_1A, name: nameof(Ushort_1A));
            Ushort_1C = s.Serialize<ushort>(Ushort_1C, name: nameof(Ushort_1C));
            MovementPath = s.Serialize<short>(MovementPath, name: nameof(MovementPath));

            MovementPathPosition = s.Serialize<int>(MovementPathPosition, name: nameof(MovementPathPosition));
        }
    }
}