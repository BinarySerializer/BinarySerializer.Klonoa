namespace BinarySerializer.Klonoa.DTP
{
    public class IDXLoadCommand : BinarySerializable
    {
        private const uint SectorSize = 2048;

        public LoaderConfiguration_DTP Pre_LoaderConfig { get; set; } // Pass in the config as a pre-serialize value as the loader will not have been created yet

        public int Type { get; set; }

        // Type 1
        public uint BIN_LBA { get; set; } // The LBA offset relative to the LBA of the BIN
        public uint BIN_Offset => BIN_LBA * SectorSize;
        public Pointer BIN_Pointer => new Pointer(BIN_Offset, Context.GetFile(Pre_LoaderConfig.FilePath_BIN));
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
                s.Log($"{nameof(BIN_Offset)}: {BIN_Offset}");

                BIN_UnknownPointerValue = s.Serialize<uint>(BIN_UnknownPointerValue, name: nameof(BIN_UnknownPointerValue));
                s.Log($"BIN_UnknownPointer: 0x{BIN_UnknownPointerValue:X8}");

                BIN_LengthValue = s.Serialize<uint>(BIN_LengthValue, name: nameof(BIN_LengthValue));
                s.Log($"{nameof(BIN_Length)}: {BIN_Length}");
            }
            else
            {
                FILE_Length = s.Serialize<uint>(FILE_Length, name: nameof(FILE_Length));

                FILE_DestinationValue = s.Serialize<uint>(FILE_DestinationValue, name: nameof(FILE_DestinationValue));

                if (Pre_LoaderConfig != null)
                {
                    FILE_Destination = Pre_LoaderConfig.FileAddresses.TryGetValue(FILE_DestinationValue, out uint value) ? value : FILE_DestinationValue;
                    s.Log($"{nameof(FILE_Destination)}: 0x{FILE_Destination:X8}");
                }

                FILE_FunctionPointer = s.Serialize<uint>(FILE_FunctionPointer, name: nameof(FILE_FunctionPointer));
                s.Log($"{nameof(FILE_FunctionPointer)}: 0x{FILE_FunctionPointer:X8}");

                // The game parses the files using the supplied function pointer, so we can use that to determine the file type
                if (Type == 2 && Pre_LoaderConfig?.FileTypes.ContainsKey(FILE_FunctionPointer) == true)
                {
                    FILE_Type = Pre_LoaderConfig.FileTypes[FILE_FunctionPointer];
                    s.Log($"{nameof(FILE_Type)}: {FILE_Type}");
                }
            }
        }

        public enum FileType
        {
            Unknown,

            // Textures
            Archive_TIM_Generic, // Textures
            Archive_TIM_SongsText, // Songs text, used in the menu
            Archive_TIM_SaveText, // Memory card save text
            Archive_TIM_SpriteSheets, // Sprite sheets, used in levels

            // Sounds
            OA05, // Sound bank
            SEQ, // Sound (music?)

            // Backgrounds
            Archive_BackgroundPack, // Backgrounds

            // Sprites
            FixedSprites, // Fixed sprite descriptors
            Archive_SpritePack, // Sprites
            Archive_LevelMenuSprites,

            // World map
            Archive_WorldMap, // World map graphics

            // Menu
            Archive_MenuSprites, // Menu graphics
            Proto_Archive_MenuSprites_0, // Menu graphics
            Proto_Archive_MenuSprites_1, // Menu graphics
            Proto_Archive_MenuSprites_2, // Menu graphics
            Font, // Menu font
            Archive_MenuBackgrounds, // Menu backgrounds
            
            // Level
            Archive_LevelPack, // Level data

            // Unknown
            Archive_Unk0,
            Unk1,
            
            // Code
            Code, // Compiled code
            CodeNoDest, // Compiled code (with a hard-coded destination)
        }
    }
}