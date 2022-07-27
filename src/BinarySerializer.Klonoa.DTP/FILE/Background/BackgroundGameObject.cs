namespace BinarySerializer.Klonoa.DTP
{
    public class BackgroundGameObject : BinarySerializable
    {
        public short XPos { get; set; }
        public short YPos { get; set; }
        public BackgroundGameObjectType Type { get; set; }
        public bool IsLayer => Type == BackgroundGameObjectType.BackgroundLayer_19 || Type == BackgroundGameObjectType.BackgroundLayer_22;

        public int BGDIndex { get; set; }
        public int UnknownValues { get; set; }
        public int CELIndex { get; set; }
        public bool UnknownFlag1 { get; set; }
        public bool UnknownFlag2 { get; set; }

        public BackgroundGameObjectData_Clear Data_Clear { get; set; }
        public BackgroundGameObjectData_PaletteScroll Data_PaletteScroll { get; set; }
        public BackgroundGameObjectData_SetLightState Data_SetLightState { get; set; }
        public BackgroundGameObjectData_PaletteSwap Data_PaletteSwap { get; set; }
        public byte[] Data_Raw { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            XPos = s.Serialize<short>(XPos, name: nameof(XPos));
            YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            Type = s.Serialize<BackgroundGameObjectType>(Type, name: nameof(Type));

            s.DoBits<ushort>(b =>
            {
                BGDIndex = b.SerializeBits<int>(BGDIndex, 5, name: nameof(BGDIndex));
                UnknownValues = b.SerializeBits<int>(UnknownValues, 7, name: nameof(UnknownValues));
                CELIndex = b.SerializeBits<int>(CELIndex, 2, name: nameof(CELIndex));
                UnknownFlag1 = b.SerializeBits<bool>(UnknownFlag1, 1, name: nameof(UnknownFlag1));
                UnknownFlag2 = b.SerializeBits<bool>(UnknownFlag2, 1, name: nameof(UnknownFlag2));
            });

            switch (Type)
            {
                case BackgroundGameObjectType.Unknown_1:
                    // There is no data to parse
                    break;

                case BackgroundGameObjectType.HUD:
                case BackgroundGameObjectType.Reset:
                    // Do nothing
                    break;

                case BackgroundGameObjectType.Clear:
                case BackgroundGameObjectType.Clear_Gradient:
                    Data_Clear = s.SerializeObject<BackgroundGameObjectData_Clear>(Data_Clear, name: nameof(Data_Clear));
                    break;

                case BackgroundGameObjectType.PaletteScroll:
                    Data_PaletteScroll = s.SerializeObject<BackgroundGameObjectData_PaletteScroll>(Data_PaletteScroll, name: nameof(Data_PaletteScroll));
                    break;

                case BackgroundGameObjectType.SetLightState:
                    Data_SetLightState = s.SerializeObject<BackgroundGameObjectData_SetLightState>(Data_SetLightState, name: nameof(Data_SetLightState));
                    break;

                // TODO: Parse
                case BackgroundGameObjectType.BackgroundLayer_19:
                case BackgroundGameObjectType.BackgroundLayer_22:
                    Data_Raw = s.SerializeArray<byte>(Data_Raw, 56, name: nameof(Data_Raw));
                    break;

                case BackgroundGameObjectType.PaletteSwap:
                    Data_PaletteSwap = s.SerializeObject<BackgroundGameObjectData_PaletteSwap>(Data_PaletteSwap, name: nameof(Data_PaletteSwap));
                    break;

                default:
                    Data_Raw = s.SerializeArray<byte>(Data_Raw, 56, name: nameof(Data_Raw));
                    s.SystemLog?.LogWarning($"Unknown background object type {Type}");
                    break;
            }

            s.Goto(Offset + 64);
        }

        public enum BackgroundGameObjectType : short
        {
            Unknown_1 = 1,

            HUD = 11, // The game HUD

            Reset = 18, // Resets some values, seems to always be the first object
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