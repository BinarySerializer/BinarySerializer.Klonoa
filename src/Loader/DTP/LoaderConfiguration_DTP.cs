using System.Collections.Generic;
using BinarySerializer.Klonoa.DTP;
using BinarySerializer.PS1;

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
                [4107] = GlobalModifierType.WeatherEffect,
                [4108] = GlobalModifierType.Object,
            },
            [5] = new Dictionary<int, GlobalModifierType>()
            {
                [4101] = GlobalModifierType.RongoLango,
                [4104] = GlobalModifierType.Bell,
            },
            [6] = new Dictionary<int, GlobalModifierType>()
            {
                [4001] = GlobalModifierType.MovingPlatform,
                [4002] = GlobalModifierType.LockedDoor_0,

                [4102] = GlobalModifierType.Light,
                [4104] = GlobalModifierType.ScenerySprites,
                [4120] = GlobalModifierType.Special,
            },
            [7] = new Dictionary<int, GlobalModifierType>()
            {
                [4101] = GlobalModifierType.GeyserPlatform,
                // TODO: 4102 is archive of movement path files, seems to be for water drops in cave
                [4103] = GlobalModifierType.PaletteAnimation,
                [4104] = GlobalModifierType.TextureAnimation,
                [4116] = GlobalModifierType.VRAMScrollAnimation,
                [4117] = GlobalModifierType.WaterWheel,
                [4120] = GlobalModifierType.Special,
            },
            [8] = new Dictionary<int, GlobalModifierType>()
            {
                [4101] = GlobalModifierType.Object,
                [4102] = GlobalModifierType.Object,
                [4103] = GlobalModifierType.VRAMScrollAnimation,
                [4120] = GlobalModifierType.Special,
            },
            [9] = new Dictionary<int, GlobalModifierType>()
            {
                [4002] = GlobalModifierType.Gondola,
                [4003] = GlobalModifierType.WoodenCart,
                [4004] = GlobalModifierType.Crate,
                [4005] = GlobalModifierType.LockedDoor_1,

                [4102] = GlobalModifierType.MultiWheel,
                [4103] = GlobalModifierType.ScenerySprites,
                [4104] = GlobalModifierType.FallingTreePart,
                [4120] = GlobalModifierType.Special,
                [4121] = GlobalModifierType.LevelModelSection,
            },
            [10] = new Dictionary<int, GlobalModifierType>()
            {
                [4001] = GlobalModifierType.WoodenMallet,
                [4002] = GlobalModifierType.LockedDoor_2,
                [4003] = GlobalModifierType.VerticallyMovingWoodenPlatform,

                [4101] = GlobalModifierType.Cogwheel,
                [4102] = GlobalModifierType.ScenerySprites,
                [4103] = GlobalModifierType.ScenerySprites,
                [4120] = GlobalModifierType.Special,
            },
        };
        public virtual Dictionary<int, TextureAnimationInfo> TextureAnimationInfos { get; } = new Dictionary<int, TextureAnimationInfo>()
        {
            [3] = new TextureAnimationInfo(true, 4),
            [7] = new TextureAnimationInfo(false, 4),
        };

        // TODO: Pointers will differ between each version, so these should be moved to the version specific configurations
        public virtual Dictionary<int, PaletteAnimationInfo> PaletteAnimationInfos { get; } = new Dictionary<int, PaletteAnimationInfo>()
        {
            [4] = new PaletteAnimationInfo(0x80125c58, 8),
            [7] = new PaletteAnimationInfo(0x8012d3c0, 8),
        };
        public virtual Dictionary<int, uint> GeyserPlatformPositionsPointers { get; } = new Dictionary<int, uint>()
        {
            // Block 7
            [8 + (10 * 4) + 0] = 0x8012f110,
            [8 + (10 * 4) + 2] = 0x8012f120,
            [8 + (10 * 4) + 3] = 0x8012f140,
            [8 + (10 * 4) + 4] = 0x8012f150,
        };
        public virtual Dictionary<int, VRAMScrollInfo[]> VRAMScrollInfos { get; } = new Dictionary<int, VRAMScrollInfo[]>()
        {
            // FUN_7_8__80122274 (NTSC)
            [7] = new VRAMScrollInfo[]
            {
                new VRAMScrollInfo(new PS1_VRAMRegion()
                {
                    XPos = 0x1A0,
                    YPos = 0x102,
                    Width = 0x10,
                    Height = 0xFE,
                }, 0x1A0, 0x100, 1),
                new VRAMScrollInfo(new PS1_VRAMRegion()
                {
                    XPos = 0x1A0,
                    YPos = 0x100,
                    Width = 0x10,
                    Height = 0x2,
                }, 0x1A0, 0x1FE, 1),
                new VRAMScrollInfo(new PS1_VRAMRegion()
                {
                    XPos = 0x190,
                    YPos = 0x101,
                    Width = 0x8,
                    Height = 0x7F,
                }, 0x190, 0x100, 1),
                new VRAMScrollInfo(new PS1_VRAMRegion()
                {
                    XPos = 0x190,
                    YPos = 0x100,
                    Width = 0x8,
                    Height = 0x1,
                }, 0x190, 0x17F, 1),
            },
            // FUN_8_7__80116f48 (NTSC)
            [8] = new VRAMScrollInfo[]
            {
                new VRAMScrollInfo(new PS1_VRAMRegion()
                {
                    XPos = 0x1F0,
                    YPos = 0x2,
                    Width = 0x10,
                    Height = 0xFE,
                }, 0x1F0, 0, 1),
                new VRAMScrollInfo(new PS1_VRAMRegion()
                {
                    XPos = 0x1F0,
                    YPos = 0x0,
                    Width = 0x10,
                    Height = 0x2,
                }, 0x1F0, 0xFE, 1),
                new VRAMScrollInfo(new PS1_VRAMRegion()
                {
                    XPos = 0x1D8,
                    YPos = 0x1,
                    Width = 0x8,
                    Height = 0x7F,
                }, 0x1D8, 0x0, 1),
                new VRAMScrollInfo(new PS1_VRAMRegion()
                {
                    XPos = 0x1D8,
                    YPos = 0x0,
                    Width = 0x8,
                    Height = 0x1,
                }, 0x1D8, 0x7F, 1),
            },
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

        public class TextureAnimationInfo
        {
            public TextureAnimationInfo(bool pingPong, int animSpeed)
            {
                PingPong = pingPong;
                AnimSpeed = animSpeed;
            }

            public bool PingPong { get; }
            public int AnimSpeed { get; }
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

        public class VRAMScrollInfo
        {
            public VRAMScrollInfo(PS1_VRAMRegion region, int destinationX, int destinationY, int animSpeed)
            {
                Region = region;
                DestinationX = destinationX;
                DestinationY = destinationY;
                AnimSpeed = animSpeed;
            }

            public PS1_VRAMRegion Region { get; }
            public int DestinationX { get; }
            public int DestinationY { get; }
            public int AnimSpeed { get; }
        }
    }
}