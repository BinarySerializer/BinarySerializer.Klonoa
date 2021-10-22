using System.Collections.Generic;
using BinarySerializer.Klonoa.DTP;

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
    }
}