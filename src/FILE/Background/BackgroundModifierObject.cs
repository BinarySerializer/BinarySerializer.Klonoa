namespace BinarySerializer.KlonoaDTP
{
    public class BackgroundModifierObject : BinarySerializable
    {
        public short XPos { get; set; }
        public short YPos { get; set; }
        public BackgroundModifierType Type { get; set; }

        public int BGDIndex { get; set; }
        public int UnknownValues { get; set; }
        public int CELIndex { get; set; }
        public bool UnknownFlag1 { get; set; }
        public bool UnknownFlag2 { get; set; }

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
                //case BackgroundModifierType.BackgroundLayer_19:
                //    break;

                //case BackgroundModifierType.BackgroundLayer_22:
                //    break;

                case BackgroundModifierType.PaletteScroll:
                    Data_23 = s.SerializeObject<BackgroundModifierData_23>(Data_23, name: nameof(Data_23));
                    break;

                default:
                    Data_Raw = s.SerializeArray<byte>(Data_Raw, 56, name: nameof(Data_Raw));
                    break;
            }

            s.Goto(Offset + 64);
        }

        public enum BackgroundModifierType : short
        {
            BackgroundLayer_19 = 19, // Used when CEL index is 1?
            
            BackgroundLayer_22 = 22,
            PaletteScroll = 23,
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