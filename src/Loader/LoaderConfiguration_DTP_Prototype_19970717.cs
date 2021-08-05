using System.Collections.Generic;

namespace BinarySerializer.KlonoaDTP
{
    public class LoaderConfiguration_DTP_Prototype_19970717 : LoaderConfiguration
    {
        public override string FilePath_EXE => "PSX.EXE";
        public override uint Address_EXE => 0x80010C00;

        public override Dictionary<uint, uint> FileAddresses { get; } = new Dictionary<uint, uint>()
        {
            [0x1000000] = 0x801121a8,
            [0x2000000] = 0x801119b8,
            [0x3000000] = 0x80146a98,
        };
        // TODO: Set remaining type pointers
        public override Dictionary<uint, IDXLoadCommand.FileType> FileTypes { get; } = new Dictionary<uint, IDXLoadCommand.FileType>()
        {
            // Textures
            [0x800172b4] = IDXLoadCommand.FileType.Archive_TIM_Generic,
            //[0x80111E80] = IDXLoadCommand.FileType.Archive_TIM_SongsText,
            //[0x8001F638] = IDXLoadCommand.FileType.Archive_TIM_SaveText,
            //[] = IDXLoadCommand.FileType.Archive_TIM_SpriteSheets, // Game doesn't have a function for this, uses Archive_TIM_Generic instead

            // Sounds
            [0x80034d64] = IDXLoadCommand.FileType.OA05,
            [0x800372E4] = IDXLoadCommand.FileType.SEQ,
            //[0x80034EB0] = IDXLoadCommand.FileType.SEQ,
            //[0x80036E58] = IDXLoadCommand.FileType.SEQ,

            // Backgrounds
            [0x80022960] = IDXLoadCommand.FileType.Archive_BackgroundPack,

            // Sprites
            [0x8005afac] = IDXLoadCommand.FileType.FixedSprites,
            [0x8005ae70] = IDXLoadCommand.FileType.Archive_SpritePack,
            //[0x8008B3A4] = IDXLoadCommand.FileType.Archive_LevelMenuSprites,

            // World map
            [0x8003ae3c] = IDXLoadCommand.FileType.Archive_WorldMap,

            // Menu
            //[0x80123D00] = IDXLoadCommand.FileType.Archive_MenuSprites,
            //[0x8012311C] = IDXLoadCommand.FileType.Font,
            //[0x80122B08] = IDXLoadCommand.FileType.Archive_MenuBackgrounds,

            // Levels
            [0x800189D8] = IDXLoadCommand.FileType.Archive_LevelPack,

            // Unknown
            //[0x800264d8] = IDXLoadCommand.FileType.Archive_Unk0,
            //[0x80022FD0] = IDXLoadCommand.FileType.Unk1,

            // Code
            [0x800654C4] = IDXLoadCommand.FileType.Code,
            [0x800654DC] = IDXLoadCommand.FileType.Code,
            [0x00000000] = IDXLoadCommand.FileType.Code,
            //[0x8007820C] = IDXLoadCommand.FileType.CodeNoDest,
        };

        public override uint Address_LevelData3DFunction => 0x80111d50;
        public override uint Address_LevelData2DPointerTable => 0x800b3c94;
    }
}