using System.Collections.Generic;

namespace BinarySerializer.Klonoa.DTP
{
    public class KlonoaSettings_DTP_US : KlonoaSettings_DTP
    {
        public override KlonoaGameVersion Version => KlonoaGameVersion.DTP;

        public override Dictionary<uint, uint> FileAddresses { get; } = new Dictionary<uint, uint>()
        {
            [0x1000000] = 0x801108f8,
            [0x2000000] = 0x801100b8,
            [0x3000000] = 0x8016a790,
        };
        public override Dictionary<uint, IDXLoadCommand.FileType> FileTypes { get; } = new Dictionary<uint, IDXLoadCommand.FileType>()
        {
            // Textures
            [0x80016CF0] = IDXLoadCommand.FileType.Archive_TIM_Generic,
            [0x80111E80] = IDXLoadCommand.FileType.Archive_TIM_SongsText,
            [0x8001F638] = IDXLoadCommand.FileType.Archive_TIM_SaveText,
            [0x80016F68] = IDXLoadCommand.FileType.Archive_TIM_SpriteSheets,

            // Sounds
            [0x80034A88] = IDXLoadCommand.FileType.OA05,
            [0x80036ECC] = IDXLoadCommand.FileType.SEQ,
            [0x80034EB0] = IDXLoadCommand.FileType.SEQ,
            [0x80036E58] = IDXLoadCommand.FileType.SEQ,

            // Backgrounds
            [0x8002304C] = IDXLoadCommand.FileType.Archive_BackgroundPack,

            // Sprites
            [0x80073930] = IDXLoadCommand.FileType.FixedSprites,
            [0x800737F4] = IDXLoadCommand.FileType.Archive_SpritePack,
            [0x8008B3A4] = IDXLoadCommand.FileType.Archive_LevelMenuSprites,

            // World map
            [0x8003B254] = IDXLoadCommand.FileType.Archive_WorldMap,

            // Menu
            [0x80123D00] = IDXLoadCommand.FileType.Archive_MenuSprites,
            [0x8012311C] = IDXLoadCommand.FileType.Font,
            [0x80122B08] = IDXLoadCommand.FileType.Archive_MenuBackgrounds,

            // Levels
            [0x8001845C] = IDXLoadCommand.FileType.Archive_LevelPack,

            // Unknown
            [0x800264d8] = IDXLoadCommand.FileType.Archive_Unk0,
            [0x80022FD0] = IDXLoadCommand.FileType.Unk1,

            // Code
            [0x8007825C] = IDXLoadCommand.FileType.Code,
            [0x80078274] = IDXLoadCommand.FileType.Code,
            [0x00000000] = IDXLoadCommand.FileType.Code,
            [0x8007820C] = IDXLoadCommand.FileType.CodeNoDest,
        };

        public override uint Address_LevelData3DFunction => 0x80110488;
        public override uint Address_LevelData2DPointerTable => 0x800b6328;

        public override BaseHardCodedObjectsLoader GetHardCodedObjectsLoader(Loader loader) =>
            new HardCodedObjectsLoader(loader);

        public override Dictionary<int, int[]> CutsceneStartSectors => new Dictionary<int, int[]>()
        {
            [3] = new int[] { 0, 1, },
            [5] = new int[] { 0, 0, },
            [6] = new int[] { 3, 2, -1 },
            [7] = new int[] { 0, 5, 5, },
            [8] = new int[] { 0, 0, },
            [9] = new int[] { 0, 0, 3, 3, },
            [10] = new int[] { 0, 4, 9, 9, 8, },
            [11] = new int[] { 0, 1, },
            [13] = new int[] { 7, 7, },
            [14] = new int[] { 0, 0, },
            [15] = new int[] { 0, 4, 4, 5, 2, 2, 6, },
            [16] = new int[] { 0, 8, },
            [17] = new int[] { 0, 0, },
            [18] = new int[] { 0, },
            [19] = new int[] { 0, 7, 7, 7, 4, 5, 6, 0, },
            [20] = new int[] { 0, },
            [21] = new int[] { 0, },
            [22] = new int[] { 0, },
            [23] = new int[] { 0, 0, -1, 2, 2, },
            [24] = new int[] { 0, 8, },
        };
    }
}