namespace BinarySerializer.Klonoa.DTP
{
    public class IDXLoadCommand : BinarySerializable
    {
        private const uint SectorSize = 2048;

        public int Type { get; set; }

        // Type 1
        public uint BIN_LBA { get; set; } // The LBA offset relative to the LBA of the BIN
        public uint BIN_Offset => BIN_LBA * SectorSize;
        public Pointer BIN_Pointer => new Pointer(BIN_Offset, Context.GetRequiredFile(Context.GetKlonoaSettings<KlonoaSettings_DTP_PS1>().FilePath_BIN));
        public uint BIN_UnknownPointerValue { get; set; }
        public uint BIN_LengthValue { get; set; }
        public uint BIN_Length => BIN_LengthValue * SectorSize;

        // Type 2
        public uint FILE_Length { get; set; }
        public uint FILE_DestinationValue { get; set; }
        public uint FILE_Destination { get; set; }
        public uint FILE_FunctionPointer { get; set; }
        public FileType FILE_Type { get; set; }
        public Pointer FILE_Pointer { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Type = s.Serialize<int>(Type, name: nameof(Type));

            if (Type == 1)
            {
                BIN_LBA = s.Serialize<uint>(BIN_LBA, name: nameof(BIN_LBA));
                s.Log("{0}: {1}", nameof(BIN_Offset), BIN_Offset);

                BIN_UnknownPointerValue = s.Serialize<uint>(BIN_UnknownPointerValue, name: nameof(BIN_UnknownPointerValue));
                s.Log("BIN_UnknownPointer: 0x{0:X8}", BIN_UnknownPointerValue);

                BIN_LengthValue = s.Serialize<uint>(BIN_LengthValue, name: nameof(BIN_LengthValue));
                s.Log("{0}: {1}", nameof(BIN_Length), BIN_Length);
            }
            else
            {
                FILE_Length = s.Serialize<uint>(FILE_Length, name: nameof(FILE_Length));

                FILE_DestinationValue = s.Serialize<uint>(FILE_DestinationValue, name: nameof(FILE_DestinationValue));

                var settings = Context.TryGetKlonoaSettings<KlonoaSettings_DTP_PS1>();

                if (settings != null)
                {
                    FILE_Destination = settings.FileAddresses.TryGetValue(FILE_DestinationValue, out uint value) ? value : FILE_DestinationValue;
                    s.Log("{0}: 0x{1:X8}", nameof(FILE_Destination), FILE_Destination);
                }

                FILE_FunctionPointer = s.Serialize<uint>(FILE_FunctionPointer, name: nameof(FILE_FunctionPointer));
                s.Log("{0}: 0x{1:X8}", nameof(FILE_FunctionPointer), FILE_FunctionPointer);

                // The game parses the files using the supplied function pointer, so we can use that to determine the file type
                if (Type == 2 && settings?.FileTypes.ContainsKey(FILE_FunctionPointer) == true)
                {
                    FILE_Type = settings.FileTypes[FILE_FunctionPointer];
                    s.Log("{0}: {1}", nameof(FILE_Type), FILE_Type);
                }
            }
        }

        public enum FileType
        {
            Unknown,

            // Textures
            Archive_TIM_Generic, // Textures (.TIA)
            Archive_TIM_SongsText, // Songs text, used in the menu
            Archive_TIM_SaveText, // Memory card save text
            Archive_TIM_SpriteSheets, // Sprite sheets, used in levels (.ARC)

            // Sounds
            OA05, // Sound bank (.OAF)
            SEQ, // Sound (music?) (.SEQ)

            // Backgrounds
            Archive_BackgroundPack, // Backgrounds (.BA)
            BackgroundPalettes, // Additional palettes for the background (.CLT)

            // Sprites
            FixedSprites, // Fixed sprite descriptors
            Archive_SpritePack, // Sprites (.ARC)
            Archive_LevelMenuSprites,

            // World map
            Archive_WorldMap, // World map graphics

            // Menu
            Archive_MenuSprites, // Menu graphics (.OFF)
            Proto_Archive_MenuSprites_0, // Menu graphics
            Proto_Archive_MenuSprites_1, // Menu graphics
            Proto_Archive_MenuSprites_2, // Menu graphics
            Font, // Menu font // DAT
            Archive_MenuBackgrounds, // Menu backgrounds
            
            // Level
            Archive_LevelPack, // Level data (.NAK)

            // Other
            Archive_ClipTable,
            
            // Code
            Code, // Compiled code (.BIN)
            CodeNoDest, // Compiled code (with a hard-coded destination)
        }
    }
}