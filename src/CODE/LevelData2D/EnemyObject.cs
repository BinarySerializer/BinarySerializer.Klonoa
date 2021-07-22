namespace BinarySerializer.KlonoaDTP
{
    public class EnemyObject : BinarySerializable
    {
        // Both of these should shift like the positions
        public int Int_00 { get; set; } // Game only initialized the object if some value is within 0x19 of this - perhaps how far along the path the object is? The path can be -1 though...
        public int Int_04 { get; set; } // Usually -1 when enemy comes from background/foreground

        public short SecondaryType { get; set; } // Primary type is 1 for enemies
        public ushort Ushort_0A { get; set; }
        public int XPos { get; set; }
        public int ZPos { get; set; }
        public int YPos { get; set; }
        public int ActualXPos => (XPos << 0xC) / 512;
        public int ActualZPos => (ZPos << 0xC) / 512;
        public int ActualYPos => (YPos << 0xC) / 512;
        public short Short_18 { get; set; }
        public short Short_1A { get; set; } // Movement path?
        public short SectorIndex { get; set; }
        public ushort Ushort_1E { get; set; }
        public ushort Ushort_20 { get; set; }
        public ushort Ushort_22 { get; set; } // Flags (flip flags)?
        public short Short_24 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Int_00 = s.Serialize<int>(Int_00, name: nameof(Int_00));
            Int_04 = s.Serialize<int>(Int_04, name: nameof(Int_04));
            SecondaryType = s.Serialize<short>(SecondaryType, name: nameof(SecondaryType));
            Ushort_0A = s.Serialize<ushort>(Ushort_0A, name: nameof(Ushort_0A));

            XPos = s.Serialize<int>(XPos, name: nameof(XPos));
            s.Log($"{nameof(ActualXPos)}: {ActualXPos}");

            ZPos = s.Serialize<int>(ZPos, name: nameof(ZPos));
            s.Log($"{nameof(ActualZPos)}: {ActualZPos}");

            YPos = s.Serialize<int>(YPos, name: nameof(YPos));
            s.Log($"{nameof(ActualYPos)}: {ActualYPos}");

            Short_18 = s.Serialize<short>(Short_18, name: nameof(Short_18));
            Short_1A = s.Serialize<short>(Short_1A, name: nameof(Short_1A));
            SectorIndex = s.Serialize<short>(SectorIndex, name: nameof(SectorIndex));
            Ushort_1E = s.Serialize<ushort>(Ushort_1E, name: nameof(Ushort_1E));
            Ushort_20 = s.Serialize<ushort>(Ushort_20, name: nameof(Ushort_20));
            Ushort_22 = s.Serialize<ushort>(Ushort_22, name: nameof(Ushort_22));
            Short_24 = s.Serialize<short>(Short_24, name: nameof(Short_24));
            s.SerializePadding(2, logIfNotNull: true);
        }
    }
}