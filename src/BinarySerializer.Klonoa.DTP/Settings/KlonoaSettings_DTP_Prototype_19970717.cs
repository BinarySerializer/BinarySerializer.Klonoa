using System.Collections.Generic;

namespace BinarySerializer.Klonoa.DTP
{
    public class KlonoaSettings_DTP_Prototype_19970717 : KlonoaSettings_DTP
    {
        public override KlonoaGameVersion Version => KlonoaGameVersion.DTP_Prototype_19970717;

        public override string FilePath_EXE => "PSX.EXE";
        public override uint Address_EXE => 0x80010C00;

        public override Dictionary<uint, uint> FileAddresses { get; } = new Dictionary<uint, uint>()
        {
            [0x1000000] = 0x801121a8,
            [0x2000000] = 0x801119b8,
            [0x3000000] = 0x80146a98,
        };
        public override Dictionary<uint, IDXLoadCommand.FileType> FileTypes { get; } = new Dictionary<uint, IDXLoadCommand.FileType>()
        {
            // Textures
            [0x800172b4] = IDXLoadCommand.FileType.Archive_TIM_Generic,
            //[] = IDXLoadCommand.FileType.Archive_TIM_SongsText, // Doesn't exist
            [0x8001EF4C] = IDXLoadCommand.FileType.Archive_TIM_SaveText,
            //[] = IDXLoadCommand.FileType.Archive_TIM_SpriteSheets, // Replaced by Archive_TIM_Generic

            // Sounds
            [0x80034d64] = IDXLoadCommand.FileType.OA05,
            [0x800372E4] = IDXLoadCommand.FileType.SEQ,
            [0x80035378] = IDXLoadCommand.FileType.SEQ,
            //[] = IDXLoadCommand.FileType.SEQ, // Doesn't exist

            // Backgrounds
            [0x80022960] = IDXLoadCommand.FileType.Archive_BackgroundPack,

            // Sprites
            [0x8005afac] = IDXLoadCommand.FileType.FixedSprites,
            [0x8005ae70] = IDXLoadCommand.FileType.Archive_SpritePack,
            //[] = IDXLoadCommand.FileType.Archive_LevelMenuSprites, // Doesn't exist

            // World map
            [0x8003ae3c] = IDXLoadCommand.FileType.Archive_WorldMap,

            // Menu
            //[] = IDXLoadCommand.FileType.Archive_MenuSprites, // Replaced by Proto_Archive_MenuSprites_X
            [0x8008AD6C] = IDXLoadCommand.FileType.Proto_Archive_MenuSprites_0,
            [0x8008AD78] = IDXLoadCommand.FileType.Proto_Archive_MenuSprites_1,
            [0x8008AD84] = IDXLoadCommand.FileType.Proto_Archive_MenuSprites_2,
            [0x8011A3F8] = IDXLoadCommand.FileType.Font,
            [0x80119E48] = IDXLoadCommand.FileType.Archive_MenuBackgrounds,

            // Levels
            [0x800189D8] = IDXLoadCommand.FileType.Archive_LevelPack,

            // Unknown
            [0x80025C7C] = IDXLoadCommand.FileType.Archive_Unk0,
            [0x800228E4] = IDXLoadCommand.FileType.Unk1,

            // Code
            [0x800654C4] = IDXLoadCommand.FileType.Code,
            [0x800654DC] = IDXLoadCommand.FileType.Code,
            [0x00000000] = IDXLoadCommand.FileType.Code,
            [0x80065474] = IDXLoadCommand.FileType.CodeNoDest,
        };

        public override uint Address_LevelData3DFunction => 0x80111d50;
        public override uint Address_LevelData2DPointerTable => 0x800b3c94;

        public override BaseHardCodedObjectsLoader GetHardCodedObjectsLoader(Loader loader) => 
            new HardCodedObjectsLoader_Prototype_19970717(loader);

        public override Dictionary<int, int[]> CutsceneStartSectors => new Dictionary<int, int[]>()
        {
            [3] = new int[] { 0, 0, 1, },
            [5] = new int[] { 0, 0, },
            [6] = new int[] { 1, 2, },
            [7] = new int[] { 0, 5, 5, },
            [8] = new int[] { 0, 0, },
            [9] = new int[] { 0, 0, 3, },
            [10] = new int[] { 0, 4, 9, 9, 8, },
            [11] = new int[] { 0, 1, },
            [13] = new int[] { 7, 7, },
            [14] = new int[] { 0, 0, },
            //[15] = new int[] { 0, 4, 4, 5, 2, 2, 6, },
            //[16] = new int[] { 0, 8, },
            //[17] = new int[] { 0, 0, },
            //[18] = new int[] { 0, },
            //[19] = new int[] { 0, 7, 7, 7, 4, 5, 6, 0, },
            //[20] = new int[] { 0, },
            //[21] = new int[] { 0, },
            //[22] = new int[] { 0, },
            //[23] = new int[] { 0, 0, -1, 2, 2, },
            //[24] = new int[] { 0, 8, },
        };

        public override Dictionary<int, Dictionary<int, PaletteAnimationInfo>> PaletteAnimationInfos { get; } = new Dictionary<int, Dictionary<int, PaletteAnimationInfo>>()
        {
            [4] = new Dictionary<int, PaletteAnimationInfo>()
            {
                [5] = new PaletteAnimationInfo(0x80127d44, 8),
            },
            [7] = new Dictionary<int, PaletteAnimationInfo>()
            {
                [3] = new PaletteAnimationInfo(0x8012e6c4, 8),
            },
            [13] = new Dictionary<int, PaletteAnimationInfo>()
            {
                [3] = new PaletteAnimationInfo(0x80112310, 8),
            },
            //[15] = new Dictionary<int, PaletteAnimationInfo>()
            //{
            //    [6] = new PaletteAnimationInfo(0x801261a8, 8),
            //},
            //[16] = new Dictionary<int, PaletteAnimationInfo>()
            //{
            //    [5] = new PaletteAnimationInfo(0x80110a8c, 12, blocksCount: 6),
            //},
            //[18] = new Dictionary<int, PaletteAnimationInfo>()
            //{
            //    [1] = new PaletteAnimationInfo(0xFFFFFFFF, 8), // Need to hard-code this...
            //    [2] = new PaletteAnimationInfo(0x80110b04, 16),
            //},
            //[23] = new Dictionary<int, PaletteAnimationInfo>()
            //{
            //    [4] = new PaletteAnimationInfo(0x8014f158, 8),
            //},
            //[24] = new Dictionary<int, PaletteAnimationInfo>()
            //{
            //    [6] = new PaletteAnimationInfo(0x80127c24, 8),
            //},
        };
        public override Dictionary<int, PaletteAnimationInfo> ObjectWithPaletteAnimationInfos { get; } = new Dictionary<int, PaletteAnimationInfo>()
        {
            //[18] = new PaletteAnimationInfo(0x80110b0c, 8),
            //[19] = new PaletteAnimationInfo(0x80110a3c, 8),
        };
        public override Dictionary<int, uint> GeyserPlatformPositionsPointers { get; } = new Dictionary<int, uint>()
        {
            // Block 7
            [8 + (10 * 4) + 0] = 0x8012cb78,
            [8 + (10 * 4) + 2] = 0x8012cb88,
            [8 + (10 * 4) + 3] = 0x8012cba8,
            [8 + (10 * 4) + 4] = 0x8012cbb8,
        };
    }
}