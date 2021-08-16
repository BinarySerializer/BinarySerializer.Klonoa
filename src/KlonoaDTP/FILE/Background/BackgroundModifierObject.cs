namespace BinarySerializer.Klonoa.DTP
{
    public class BackgroundModifierObject : BinarySerializable
    {
        public short XPos { get; set; }
        public short YPos { get; set; }
        public BackgroundModifierType Type { get; set; }
        public bool IsLayer => Type == BackgroundModifierType.BackgroundLayer_19 || Type == BackgroundModifierType.BackgroundLayer_22;

        public int BGDIndex { get; set; }
        public int UnknownValues { get; set; }
        public int CELIndex { get; set; }
        public bool UnknownFlag1 { get; set; }
        public bool UnknownFlag2 { get; set; }

        public BackgroundModifierData_20 Data_20 { get; set; }
        public BackgroundModifierData_23 Data_23 { get; set; }
        public byte[] Data_Raw { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            XPos = s.Serialize<short>(XPos, name: nameof(XPos));
            YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            Type = s.Serialize<BackgroundModifierType>(Type, name: nameof(Type));

            s.SerializeBitValues<ushort>(bitFunc =>
            {
                BGDIndex = bitFunc(BGDIndex, 5, name: nameof(BGDIndex));
                UnknownValues = bitFunc(UnknownValues, 7, name: nameof(UnknownValues));
                CELIndex = bitFunc(CELIndex, 2, name: nameof(CELIndex));
                UnknownFlag1 = bitFunc(UnknownFlag1 ? 1 : 0, 1, name: nameof(UnknownFlag1)) == 1;
                UnknownFlag2 = bitFunc(UnknownFlag2 ? 1 : 0, 1, name: nameof(UnknownFlag2)) == 1;
            });

            switch (Type)
            {
                case BackgroundModifierType.HUD:
                case BackgroundModifierType.Reset:
                    // Do nothing
                    break;

                case BackgroundModifierType.Clear:
                    Data_20 = s.SerializeObject<BackgroundModifierData_20>(Data_20, name: nameof(Data_20));
                    break;

                case BackgroundModifierType.PaletteScroll:
                    Data_23 = s.SerializeObject<BackgroundModifierData_23>(Data_23, name: nameof(Data_23));
                    break;

                // TODO: Parse
                case BackgroundModifierType.BackgroundLayer_19:
                case BackgroundModifierType.BackgroundLayer_22:
                    Data_Raw = s.SerializeArray<byte>(Data_Raw, 56, name: nameof(Data_Raw));
                    break;

                default:
                    Data_Raw = s.SerializeArray<byte>(Data_Raw, 56, name: nameof(Data_Raw));
                    s.LogWarning($"Unknown background modifier type {Type}");
                    break;
            }

            s.Goto(Offset + 64);
        }

        public enum BackgroundModifierType : short
        {
            HUD = 11, // The game HUD

            Reset = 18, // Resets some values, seems to always be the first modifier
            BackgroundLayer_19 = 19, // Used when CEL index is 1?
            Clear = 20, // Handles background clearing each frame

            BackgroundLayer_22 = 22,
            PaletteScroll = 23,
        }

        public class BackgroundModifierData_20 : BinarySerializable
        {
            public Entry[] Entries { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Entries = s.SerializeObjectArray<Entry>(Entries, 4, name: nameof(Entries));
            }

            public class Entry : BinarySerializable
            {
                public short XPos { get; set; }
                public short XPos_RelativeObj { get; set; } // An index to a background modifier, can be -1

                public short YPos { get; set; }
                public short YPos_RelativeObj { get; set; } // An index to a background modifier, can be -1

                public RGBA8888Color Color { get; set; }

                public override void SerializeImpl(SerializerObject s)
                {
                    XPos = s.Serialize<short>(XPos, name: nameof(XPos));
                    XPos_RelativeObj = s.Serialize<short>(XPos_RelativeObj, name: nameof(XPos_RelativeObj));
                    YPos = s.Serialize<short>(YPos, name: nameof(YPos));
                    YPos_RelativeObj = s.Serialize<short>(YPos_RelativeObj, name: nameof(YPos_RelativeObj));
                    Color = s.SerializeObject<RGBA8888Color>(Color, name: nameof(Color));
                }
            }
        }
        public class BackgroundModifierData_23 : BinarySerializable
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
}