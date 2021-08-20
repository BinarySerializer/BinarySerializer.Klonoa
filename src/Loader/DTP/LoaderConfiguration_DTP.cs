using System.Collections.Generic;
using BinarySerializer.Klonoa.DTP;

namespace BinarySerializer.Klonoa
{
    public abstract class LoaderConfiguration_DTP : LoaderConfiguration
    {
        public virtual int BLOCK_Fix => 0;
        public virtual int BLOCK_Menu => 1;
        public virtual int BLOCK_FirstLevel => 3;

        public virtual string FilePath_BIN => "FILE.BIN";
        public virtual string FilePath_IDX => "FILE.IDX";
        public virtual string FilePath_EXE => "KLONOA.BIN";
        public virtual uint Address_IDX => 0x80010000;
        public virtual uint Address_EXE => 0x80011000;

        public abstract Dictionary<uint, uint> FileAddresses { get; }
        public abstract Dictionary<uint, IDXLoadCommand.FileType> FileTypes { get; }

        public abstract uint Address_LevelData3DFunction { get; }
        public abstract uint Address_LevelData2DPointerTable { get; }

        public virtual Dictionary<int, Dictionary<int, GlobalModifierType>> GlobalModifierTypes { get; } = new Dictionary<int, Dictionary<int, GlobalModifierType>>()
        {
            [3] = new Dictionary<int, GlobalModifierType>()
            {
                [4001] = GlobalModifierType.MovingPlatform,

                [4101] = GlobalModifierType.WindSwirl,
                [4103] = GlobalModifierType.ScrollAnimation,
                [4105] = GlobalModifierType.SmallWindmill,
                [4106] = GlobalModifierType.BigWindmill,
                [4107] = GlobalModifierType.ScenerySprites,
                [4108] = GlobalModifierType.RoadSign,
                [4109] = GlobalModifierType.ScenerySprites,
                [4110] = GlobalModifierType.TextureAnimation,
                [4120] = GlobalModifierType.Special,
                [4121] = GlobalModifierType.LevelModelSection,
            },
            [4] = new Dictionary<int, GlobalModifierType>()
            {
                [4001] = GlobalModifierType.Minecart,
                [4002] = GlobalModifierType.TiltRock,

                [4101] = GlobalModifierType.WindSwirl,
                [4103] = GlobalModifierType.ScrollAnimation,
                [4104] = GlobalModifierType.ScenerySprites,
                [4105] = GlobalModifierType.PaletteAnimation,
                [4107] = GlobalModifierType.Special, // Rain?
                [4108] = GlobalModifierType.Object,
            },
        };
        public virtual Dictionary<int, int> TextureAnimationSpeeds { get; } = new Dictionary<int, int>()
        {
            [3] = 4,
        };
        public virtual Dictionary<int, PaletteAnimationInfo> PaletteAnimationInfos { get; } = new Dictionary<int, PaletteAnimationInfo>()
        {
            [4] = new PaletteAnimationInfo(0x80125c58, 8),
        };
        public GlobalModifierType GetGlobalModifierType(int binBlock, int primaryType, int secondaryType)
        {
            if (!GlobalModifierTypes.ContainsKey(binBlock))
                return GlobalModifierType.Unknown;

            var typeKey = primaryType * 100 + secondaryType;

            if (!GlobalModifierTypes[binBlock].ContainsKey(typeKey))
                return GlobalModifierType.Unknown;

            return GlobalModifierTypes[binBlock][typeKey];
        }

        public class PaletteAnimationInfo
        {
            public PaletteAnimationInfo(uint addressRegions, int animSpeed)
            {
                Address_Regions = addressRegions;
                AnimSpeed = animSpeed;
            }

            public uint Address_Regions { get; }
            public int AnimSpeed { get; }
        }
    }
}