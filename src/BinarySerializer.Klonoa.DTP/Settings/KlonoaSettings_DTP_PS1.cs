using BinarySerializer.PS1;
using System.Collections.Generic;

namespace BinarySerializer.Klonoa.DTP
{
    public abstract class KlonoaSettings_DTP_PS1 : KlonoaSettings_DTP
    {
        public virtual string FilePath_BIN => "FILE.BIN";
        public virtual string FilePath_IDX => "FILE.IDX";
        public virtual string FilePath_EXE => "KLONOA.BIN";
        public virtual uint Address_IDX => 0x80010000;
        public virtual uint Address_EXE => 0x80011000;

        public abstract Dictionary<uint, uint> FileAddresses { get; }
        public abstract Dictionary<uint, IDXLoadCommand.FileType> FileTypes { get; }

        public abstract uint Address_LevelData3DFunction { get; }
        public abstract uint Address_LevelData2DPointerTable { get; }

        public abstract BaseHardCodedObjectsLoader GetHardCodedObjectsLoader(Loader loader);
        
        public abstract Dictionary<int, int[]> CutsceneStartSectors { get; }

        public virtual Dictionary<int, Dictionary<int, GlobalGameObjectType>> GlobalGameObjectTypes { get; } = new Dictionary<int, Dictionary<int, GlobalGameObjectType>>()
        {
            [3] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.MovingPlatform,

                [4101] = GlobalGameObjectType.WindSwirl,
                [4103] = GlobalGameObjectType.ScrollAnimation,
                [4105] = GlobalGameObjectType.SmallWindmill,
                [4106] = GlobalGameObjectType.BigWindmill,
                [4107] = GlobalGameObjectType.ScenerySprites,
                [4108] = GlobalGameObjectType.RoadSign,
                [4109] = GlobalGameObjectType.ScenerySprites,
                [4110] = GlobalGameObjectType.TextureAnimation,
                [4120] = GlobalGameObjectType.Special,
                [4121] = GlobalGameObjectType.LevelModelSection,
            },
            [4] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.Minecart,
                [4002] = GlobalGameObjectType.TiltRock,

                [4101] = GlobalGameObjectType.WindSwirl,
                [4103] = GlobalGameObjectType.ScrollAnimation,
                [4104] = GlobalGameObjectType.ScenerySprites,
                [4105] = GlobalGameObjectType.PaletteAnimations,
                [4107] = GlobalGameObjectType.WeatherEffect,
                [4108] = GlobalGameObjectType.Object,
            },
            [5] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4101] = GlobalGameObjectType.RongoLango,
                [4104] = GlobalGameObjectType.Bell,
            },
            [6] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.MovingPlatform,
                [4002] = GlobalGameObjectType.LockedDoor_0,

                [4102] = GlobalGameObjectType.Light,
                [4104] = GlobalGameObjectType.ScenerySprites,
                [4120] = GlobalGameObjectType.Special,
            },
            [7] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4101] = GlobalGameObjectType.GeyserPlatform,
                [4102] = GlobalGameObjectType.MovementPaths,
                [4103] = GlobalGameObjectType.PaletteAnimations,
                [4104] = GlobalGameObjectType.TextureAnimation,
                [4116] = GlobalGameObjectType.VRAMScrollAnimation,
                [4117] = GlobalGameObjectType.WaterWheel,
                [4120] = GlobalGameObjectType.Special,
            },
            [8] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4101] = GlobalGameObjectType.Object,
                [4102] = GlobalGameObjectType.Object,
                [4103] = GlobalGameObjectType.VRAMScrollAnimation,
                [4120] = GlobalGameObjectType.Special,
            },
            [9] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4002] = GlobalGameObjectType.Gondola,
                [4003] = GlobalGameObjectType.WoodenCart,
                [4004] = GlobalGameObjectType.Crate,
                [4005] = GlobalGameObjectType.LockedDoor_1,
                [4006] = GlobalGameObjectType.FallingTreePart, // July prototype

                [4102] = GlobalGameObjectType.MultiWheel,
                [4103] = GlobalGameObjectType.ScenerySprites,
                [4104] = GlobalGameObjectType.FallingTreePart,
                [4120] = GlobalGameObjectType.Special,
                [4121] = GlobalGameObjectType.LevelModelSection,
            },
            [10] = new Dictionary<int, GlobalGameObjectType>() // Note: This level scroll the VRAM using an object of primary type 12 (FUN_10_8__80110c78)
            {
                [4001] = GlobalGameObjectType.WoodenMallet,
                [4002] = GlobalGameObjectType.LockedDoor_2,
                [4003] = GlobalGameObjectType.VerticallyMovingWoodenPlatform,
                [4004] = GlobalGameObjectType.SpinningWoodAttachedPlatform,
                [4005] = GlobalGameObjectType.SpinningWood,
                [4006] = GlobalGameObjectType.MovingLedge,

                [4101] = GlobalGameObjectType.Cogwheel,
                [4102] = GlobalGameObjectType.ScenerySprites,
                [4103] = GlobalGameObjectType.ScenerySprites,
                [4120] = GlobalGameObjectType.Special,
            },
            [11] = new Dictionary<int, GlobalGameObjectType>()
            {
                // Nothing
            },
            [12] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.UnstablePlatform,
                [4002] = GlobalGameObjectType.SwingingPlatform,
                [4003] = GlobalGameObjectType.Ledge,

                [4121] = GlobalGameObjectType.LevelModelSection,
            },
            [13] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.Bone,
                [4002] = GlobalGameObjectType.RedBoulder,
                [4003] = GlobalGameObjectType.GreenBoulder,
                [4004] = GlobalGameObjectType.BlockingBoulder,

                [4101] = GlobalGameObjectType.VRAMScrollAnimationWithTexture,
                [4103] = GlobalGameObjectType.PaletteAnimation,
                [4104] = GlobalGameObjectType.TextureAnimation,
                [4105] = GlobalGameObjectType.DestroyedHouse,
                [4107] = GlobalGameObjectType.ScenerySprites,
                [4120] = GlobalGameObjectType.Special,
            },
            [14] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.SwingingWoodPlank,
                [4002] = GlobalGameObjectType.Collision,

                [4101] = GlobalGameObjectType.Rocks,
                [4120] = GlobalGameObjectType.Special,
            },
            [15] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.MovingPlatformOnTrack,
                [4002] = GlobalGameObjectType.SpinningWheel,
                [4003] = GlobalGameObjectType.FallingTargetPlatform,
                [4004] = GlobalGameObjectType.BlockingLedge,
                [4005] = GlobalGameObjectType.OrbDoorPart,
                [4007] = GlobalGameObjectType.LockedDoor_3,

                [4101] = GlobalGameObjectType.ScenerySprites,
                [4102] = GlobalGameObjectType.ScenerySprites,
                [4103] = GlobalGameObjectType.VRAMScrollAnimation,
                [4104] = GlobalGameObjectType.RGBAnimation,
                [4105] = GlobalGameObjectType.Object,
                [4106] = GlobalGameObjectType.PaletteAnimations,
                [4120] = GlobalGameObjectType.Special,
            },
            [16] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.MovingPlatformWithOptionalLocal,
                [4002] = GlobalGameObjectType.MovingWallPillars,
                [4003] = GlobalGameObjectType.IronGate,
                [4004] = GlobalGameObjectType.DarkLightPlatform,

                [4101] = GlobalGameObjectType.DarkLightSwitcher,
                [4102] = GlobalGameObjectType.ScenerySprites,
                [4103] = GlobalGameObjectType.ScenerySprites,
                [4104] = GlobalGameObjectType.ScenerySprites,
                [4105] = GlobalGameObjectType.PaletteAnimation,
                [4121] = GlobalGameObjectType.LevelModelSection,
            },
            [17] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.JokaBossArenaCollision,

                [4101] = GlobalGameObjectType.JokaSpinningCore,
            },
            [18] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.TransparentGemPlatform,
                [4002] = GlobalGameObjectType.MovingCavePlatform,
                [4003] = GlobalGameObjectType.BirdStatueWithSwitch,
                [4004] = GlobalGameObjectType.ObjectWithPaletteAnimation,
                [4005] = GlobalGameObjectType.LightField,

                [4101] = GlobalGameObjectType.PaletteAnimation,
                [4102] = GlobalGameObjectType.PaletteAnimation,
                [4103] = GlobalGameObjectType.LightField, // July prototype
                [4120] = GlobalGameObjectType.Special,
                [4121] = GlobalGameObjectType.LevelModelSection,
            },
            [19] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.TransparentGemPlatform,
                [4002] = GlobalGameObjectType.MovingCavePlatform,
                [4003] = GlobalGameObjectType.OnWayMovingWallPillar,
                [4004] = GlobalGameObjectType.ColoredPillar,
                [4006] = GlobalGameObjectType.DoorWithPillar,
                [4007] = GlobalGameObjectType.ObjectWithPaletteAnimation,

                [4101] = GlobalGameObjectType.ColoredStatue,
                [4102] = GlobalGameObjectType.ColoredDoor,
                [4103] = GlobalGameObjectType.VRAMScrollAnimation,
                [4104] = GlobalGameObjectType.Object,
                [4120] = GlobalGameObjectType.Special,
                [4121] = GlobalGameObjectType.LevelModelSection,
            },
            [20] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.GhadiusCirclePlatform,
                [4002] = GlobalGameObjectType.JokaBossArenaCollision,

                [4101] = GlobalGameObjectType.CutsceneCrystal,
            },
            [21] = new Dictionary<int, GlobalGameObjectType>()
            {
                // Nothing
            },
            [22] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4102] = GlobalGameObjectType.NahatombSphere,
                [4103] = GlobalGameObjectType.NahatombEscaping,
                [4104] = GlobalGameObjectType.Special,
                [4105] = GlobalGameObjectType.CutsceneCrystal,
                [4120] = GlobalGameObjectType.Special,
            },
            [23] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4001] = GlobalGameObjectType.NahatombBluePlatformAndGem,

                [4103] = GlobalGameObjectType.Special,
                [4104] = GlobalGameObjectType.NahatombPaletteAnimation,
                [4105] = GlobalGameObjectType.Special,
            },
            [24] = new Dictionary<int, GlobalGameObjectType>()
            {
                [4101] = GlobalGameObjectType.VRAMScrollAnimation,
                [4102] = GlobalGameObjectType.LevelTimer,
                [4104] = GlobalGameObjectType.Fireworks,
                [4105] = GlobalGameObjectType.Textures,
                [4106] = GlobalGameObjectType.PaletteAnimations,
                [4121] = GlobalGameObjectType.LevelModelSection,
            },
        };
        public virtual Dictionary<int, TextureAnimationInfo> TextureAnimationInfos { get; } = new Dictionary<int, TextureAnimationInfo>()
        {
            [3] = new TextureAnimationInfo(true, 4),
            [7] = new TextureAnimationInfo(false, 4),
            [13] = new TextureAnimationInfo(false, 4),
        };

        public abstract Dictionary<int, Dictionary<int, PaletteAnimationInfo>> PaletteAnimationInfos { get; }
        public abstract Dictionary<int, PaletteAnimationInfo> ObjectWithPaletteAnimationInfos { get; }
        public abstract Dictionary<int, uint> GeyserPlatformPositionsPointers { get; }
        public virtual Dictionary<int, VRAMScrollInfo[]> VRAMScrollInfos { get; } = new Dictionary<int, VRAMScrollInfo[]>()
        {
            // FUN_7_8__80122274
            [7] = new VRAMScrollInfo[]
            {
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1A0,
                    YPos = 0x102,
                    Width = 0x10,
                    Height = 0xFE,
                }, 0x1A0, 0x100, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1A0,
                    YPos = 0x100,
                    Width = 0x10,
                    Height = 0x2,
                }, 0x1A0, 0x1FE, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x190,
                    YPos = 0x101,
                    Width = 0x8,
                    Height = 0x7F,
                }, 0x190, 0x100, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x190,
                    YPos = 0x100,
                    Width = 0x8,
                    Height = 0x1,
                }, 0x190, 0x17F, 1),
            },
            // FUN_8_7__80116f48
            [8] = new VRAMScrollInfo[]
            {
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1F0,
                    YPos = 0x2,
                    Width = 0x10,
                    Height = 0xFE,
                }, 0x1F0, 0, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1F0,
                    YPos = 0x0,
                    Width = 0x10,
                    Height = 0x2,
                }, 0x1F0, 0xFE, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1D8,
                    YPos = 0x1,
                    Width = 0x8,
                    Height = 0x7F,
                }, 0x1D8, 0x0, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1D8,
                    YPos = 0x0,
                    Width = 0x8,
                    Height = 0x1,
                }, 0x1D8, 0x7F, 1),
            },
            // FUN_13_8__801205a4
            [13] = new VRAMScrollInfo[]
            {
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1B0,
                    YPos = 0x102,
                    Width = 0x10,
                    Height = 0xFE,
                }, 0x1b0, 0x100, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1B0,
                    YPos = 0x100,
                    Width = 0x10,
                    Height = 0x2,
                }, 0x1b0, 0x1fe, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x198,
                    YPos = 0x101,
                    Width = 0x8,
                    Height = 0x7F,
                }, 0x198, 0x100, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x198,
                    YPos = 0x100,
                    Width = 0x8,
                    Height = 0x1,
                }, 0x198, 0x17f, 1),
            },
            // FUN_15_8__8011b3c8
            [15] = new VRAMScrollInfo[]
            {
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x160,
                    YPos = 0x141,
                    Width = 0x10,
                    Height = 0xBF,
                }, 0x160, 0x140, 4),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x160,
                    YPos = 0x140,
                    Width = 0x10,
                    Height = 0x1,
                }, 0x160, 0x1ff, 4),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x150,
                    YPos = 0x121,
                    Width = 0x8,
                    Height = 0x5F,
                }, 0x150, 0x120, 8),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x150,
                    YPos = 0x120,
                    Width = 0x8,
                    Height = 0x1,
                }, 0x150, 0x17f, 8),
            },
            // FUN_19_8__801208d8
            [19] = new VRAMScrollInfo[]
            {
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x160,
                    YPos = 0x182,
                    Width = 0x10,
                    Height = 0x7E,
                }, 0x160, 0x180, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x160,
                    YPos = 0x180,
                    Width = 0x10,
                    Height = 0x2,
                }, 0x160, 0x1FE, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x150,
                    YPos = 0x141,
                    Width = 0x8,
                    Height = 0x3F,
                }, 0x150, 0x140, 1),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x150,
                    YPos = 0x140,
                    Width = 0x8,
                    Height = 0x1,
                }, 0x150, 0x17F, 1),
            },
            // FUN_24_7__8011d3c8
            [24] = new VRAMScrollInfo[]
            {
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1F0,
                    YPos = 0x1,
                    Width = 0x10,
                    Height = 0xBF,
                }, 0x1F0, 0x00, 4),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1F0,
                    YPos = 0x0,
                    Width = 0x10,
                    Height = 0x1,
                }, 0x1F0, 0xBF, 4),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1D8,
                    YPos = 0x1,
                    Width = 0x8,
                    Height = 0x5F,
                }, 0x1D8, 0x00, 8),
                new VRAMScrollInfo(new Rect()
                {
                    XPos = 0x1D8,
                    YPos = 0x20,
                    Width = 0x8,
                    Height = 0x1,
                }, 0x1D8, 0x5F, 8),
            },
        };
        public GlobalGameObjectType GetGlobalGameObjectType(int binBlock, int primaryType, int secondaryType)
        {
            if (!GlobalGameObjectTypes.ContainsKey(binBlock))
                return GlobalGameObjectType.Unknown;

            var typeKey = primaryType * 100 + secondaryType;

            if (!GlobalGameObjectTypes[binBlock].ContainsKey(typeKey))
                return GlobalGameObjectType.Unknown;

            return GlobalGameObjectTypes[binBlock][typeKey];
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
            public PaletteAnimationInfo(uint addressRegions, int animSpeed, int blocksCount = -1)
            {
                Address_Regions = addressRegions;
                AnimSpeed = animSpeed;
                BlocksCount = blocksCount;
            }

            public uint Address_Regions { get; }
            public int AnimSpeed { get; }
            public int BlocksCount { get; }
        }

        public class VRAMScrollInfo
        {
            public VRAMScrollInfo(Rect region, int destinationX, int destinationY, int animSpeed)
            {
                Region = region;
                DestinationX = destinationX;
                DestinationY = destinationY;
                AnimSpeed = animSpeed;
            }

            public Rect Region { get; }
            public int DestinationX { get; }
            public int DestinationY { get; }
            public int AnimSpeed { get; }
        }
    }
}