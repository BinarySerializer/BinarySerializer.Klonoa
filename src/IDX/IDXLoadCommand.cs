using System.Collections.Generic;

namespace BinarySerializer.KlonoaDTP
{
    public class IDXLoadCommand : BinarySerializable
    {
        private const uint SectorSize = 2048;

        public int Type { get; set; }

        // Type 1
        public uint BIN_LBA { get; set; } // The LBA offset relative to the LBA of the BIN
        public uint BIN_Offset => BIN_LBA * SectorSize;
        public Pointer BIN_Pointer => new Pointer(BIN_Offset, Context.GetFile(Loader.FilePath_BIN));
        public uint BIN_UnknownPointerValue { get; set; }
        public uint BIN_LengthValue { get; set; }
        public uint BIN_Length => BIN_LengthValue * SectorSize;

        // Type 2
        public uint FILE_Length { get; set; }
        public uint FILE_DestinationValue { get; set; }
        public uint FILE_FunctionPointer { get; set; }
        public FileType FILE_Type { get; set; }
        public Pointer FILE_Pointer { get; set; }

        public uint GetFileDestinationAddress(Loader loader)
        {
            var fileAddr = loader.Config.FileAddresses;

            if (fileAddr.ContainsKey(FILE_DestinationValue))
                return fileAddr[FILE_DestinationValue];
            else
                return FILE_DestinationValue;
        }

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
                s.Log($"{nameof(FILE_DestinationValue)}: 0x{FILE_DestinationValue:X8}");

                FILE_FunctionPointer = s.Serialize<uint>(FILE_FunctionPointer, name: nameof(FILE_FunctionPointer));
                s.Log($"{nameof(FILE_FunctionPointer)}: 0x{FILE_FunctionPointer:X8}");

                if (FileTypes.ContainsKey(FILE_FunctionPointer))
                {
                    FILE_Type = FileTypes[FILE_FunctionPointer];
                    s.Log($"{nameof(FILE_Type)}: {FILE_Type}");
                }
            }
        }

        // The game parses the files using the supplied function pointer, so we can use that to determine the file type
        public static Dictionary<uint, FileType> FileTypes { get; } = new Dictionary<uint, FileType>()
        {
            // Textures
            [0x80016CF0] = FileType.Archive_TIM_Generic,
            [0x80111E80] = FileType.Archive_TIM_SongsText,
            [0x8001F638] = FileType.Archive_TIM_SaveText,
            [0x80016F68] = FileType.Archive_TIM_SpriteSheets,

            // Sounds
            [0x80034A88] = FileType.OA05,
            [0x80036ECC] = FileType.SEQ,
            [0x80034EB0] = FileType.SEQ,

            // Backgrounds
            [0x8002304C] = FileType.Archive_BackgroundPack,

            // Sprites
            [0x80073930] = FileType.FixedSprites,
            [0x800737F4] = FileType.Archive_SpritePack,
            
            // World map
            [0x8003B254] = FileType.Archive_WorldMap,

            // Menu
            [0x80123D00] = FileType.Archive_MenuSprites,
            [0x8012311C] = FileType.Font,
            [0x80122B08] = FileType.Archive_MenuBackgrounds,

            // Levels
            [0x8001845C] = FileType.Archive_LevelPack,
            
            // Unknown
            [0x800264d8] = FileType.Archive_Unk0,

            // Code
            [0x8007825C] = FileType.Code,
            [0x80078274] = FileType.Code,
            [0x00000000] = FileType.Code,
        };

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
            
            // World map
            Archive_WorldMap, // World map graphics

            // Menu
            Archive_MenuSprites, // Menu graphics
            Font, // Menu font
            Archive_MenuBackgrounds, // Menu backgrounds
            
            // Level
            Archive_LevelPack, // Level data

            // Unknown
            Archive_Unk0,
            
            // Code
            Code, // Compiled code
        }
    }
}