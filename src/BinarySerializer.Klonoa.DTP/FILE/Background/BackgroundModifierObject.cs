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

        public BackgroundModifierData_Clear Data_Clear { get; set; }
        public BackgroundModifierData_PaletteScroll Data_PaletteScroll { get; set; }
        public BackgroundModifierData_SetLightState Data_SetLightState { get; set; }
        public BackgroundModifierData_PaletteSwap Data_PaletteSwap { get; set; }
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
                case BackgroundModifierType.Unknown_1:
                    // There is no data to parse
                    break;

                case BackgroundModifierType.HUD:
                case BackgroundModifierType.Reset:
                    // Do nothing
                    break;

                case BackgroundModifierType.Clear:
                case BackgroundModifierType.Clear_Gradient:
                    Data_Clear = s.SerializeObject<BackgroundModifierData_Clear>(Data_Clear, name: nameof(Data_Clear));
                    break;

                case BackgroundModifierType.PaletteScroll:
                    Data_PaletteScroll = s.SerializeObject<BackgroundModifierData_PaletteScroll>(Data_PaletteScroll, name: nameof(Data_PaletteScroll));
                    break;

                case BackgroundModifierType.SetLightState:
                    Data_SetLightState = s.SerializeObject<BackgroundModifierData_SetLightState>(Data_SetLightState, name: nameof(Data_SetLightState));
                    break;

                // TODO: Parse
                case BackgroundModifierType.BackgroundLayer_19:
                case BackgroundModifierType.BackgroundLayer_22:
                    Data_Raw = s.SerializeArray<byte>(Data_Raw, 56, name: nameof(Data_Raw));
                    break;

                case BackgroundModifierType.PaletteSwap:
                    Data_PaletteSwap = s.SerializeObject<BackgroundModifierData_PaletteSwap>(Data_PaletteSwap, name: nameof(Data_PaletteSwap));
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
            Unknown_1 = 1,

            HUD = 11, // The game HUD

            Reset = 18, // Resets some values, seems to always be the first modifier
            BackgroundLayer_19 = 19, // Used when CEL index is 1?
            Clear_Gradient = 20, // Handles background clearing each frame
            Clear = 21,
            BackgroundLayer_22 = 22,
            PaletteScroll = 23,
            SetLightState = 24,
            PaletteSwap = 25,
        }
    }
}