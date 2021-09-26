namespace BinarySerializer.Klonoa.DTP
{
    public enum GlobalModifierType
    {
        Unknown,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.TMD, GlobalModifierFileType.Position)]
        [ModifierRotation(ModifierRotationAttribute.RotAxis.Y, 128)]
        WindSwirl,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Transform_WithInfo, GlobalModifierFileType.TIM)]
        [ModifierRotation(ModifierRotationAttribute.RotAxis.Z, -9)]
        BigWindmill,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Transform_WithInfo)]
        [ModifierRotation(ModifierRotationAttribute.RotAxis.Z, -24)]
        SmallWindmill,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Collision, GlobalModifierFileType.MovementPaths, GlobalModifierFileType.Transform_WithoutInfo)]
        MovingPlatform,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Transform_WithInfo)]
        RoadSign,

        // FUN_4_8__8011fff8
        [ModifierFiles(
            GlobalModifierFileType.TMD, 
            GlobalModifierFileType.Collision, 
            GlobalModifierFileType.MovementPaths, 
            GlobalModifierFileType.Transform_WithoutInfo, // Absolute
            GlobalModifierFileType.Unknown)] // TODO: Camera related (parsed at 0x800816e8)
        TiltRock,

        [ModifierFiles(
            GlobalModifierFileType.TMD, 
            GlobalModifierFileType.Collision, 
            GlobalModifierFileType.MovementPaths, 
            GlobalModifierFileType.Transforms_WithInfo, // Absolute
            GlobalModifierFileType.Unknown, // TODO: Camera related (parsed at 0x800816e8)
            GlobalModifierFileType.Transform_WithInfo)] // Local
        Minecart,

        [ModifierFiles(
            GlobalModifierFileType.TMD, 
            GlobalModifierFileType.UnknownArchiveArchive, // TODO: Parts
            GlobalModifierFileType.Unknown, 
            GlobalModifierFileType.Unknown, 
            GlobalModifierFileType.Unknown, 
            GlobalModifierFileType.UnknownArchive, 
            GlobalModifierFileType.UnknownArchive)] // TODO: Palettes
        RongoLango,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Position)]
        Bell,

        // FUN_800790e4
        [ModifierFiles(
            GlobalModifierFileType.TMD, 
            GlobalModifierFileType.Collision, 
            GlobalModifierFileType.Transform_WithoutInfo, // Local
            GlobalModifierFileType.Transform_WithoutInfo, // Absolute
            GlobalModifierFileType.Unknown)] // Unused?
        LockedDoor_0,

        [ModifierFiles(
            GlobalModifierFileType.TMD, 
            GlobalModifierFileType.Collision, 
            GlobalModifierFileType.Transform_WithoutInfo, // Local
            GlobalModifierFileType.Transform_WithoutInfo)] // Absolute
        LockedDoor_1,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Transform_WithoutInfo)]
        [ModifierRotation(ModifierRotationAttribute.RotAxis.Z, -2)]
        WaterWheel,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Collision, GlobalModifierFileType.Transform_WithoutInfo)]
        Crate,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Transform_WithoutInfo)]
        MultiWheel,

        // TODO: Sometimes has two additional transforms - unused?
        // FUN_9_8__80120d24
        [ModifierFiles(
            GlobalModifierFileType.TMD, 
            GlobalModifierFileType.Collision,
            GlobalModifierFileType.MovementPaths,
            GlobalModifierFileType.Transform_WithoutInfo, // Local
            GlobalModifierFileType.Transform_WithoutInfo)] // Absolute
        Gondola,

        // FUN_9_8__80122870
        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Transform_WithoutInfo)]
        FallingTreePart,

        // FUN_9_8__80122cfc
        [ModifierFiles(
            GlobalModifierFileType.TMD,
            GlobalModifierFileType.Collision,
            GlobalModifierFileType.MovementPaths,
            GlobalModifierFileType.Transforms_WithoutInfo, // Absolute
            GlobalModifierFileType.Unknown, // TODO: Camera related (parsed at 0x800816e8)
            GlobalModifierFileType.Transform_WithInfo)] // Local - NOTE: The positions file in here only has one entry. The game parses 3.
        WoodenCart,

        [ModifierFiles(GlobalModifierFileType.LightObject)]
        Light,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Collision)]
        GeyserPlatform,

        [ModifierFiles(GlobalModifierFileType.UVScrollAnimation)]
        ScrollAnimation, // Scrolls the UVs for the level model

        [ModifierFiles]
        VRAMScrollAnimation, // Scrolls regions in VRAM

        [ModifierFiles(GlobalModifierFileType.TMD)]
        Object,

        [ModifierFiles(
            GlobalModifierFileType.TMD, 
            GlobalModifierFileType.UnknownModelObjectsData)] // TODO: Parsed at FUN_8002692c
        LevelModelSection,

        [ModifierFiles(GlobalModifierFileType.ScenerySprites)]
        ScenerySprites,

        [ModifierFiles(GlobalModifierFileType.TextureAnimation)]
        TextureAnimation,

        [ModifierFiles(GlobalModifierFileType.PaletteAnimation)]
        PaletteAnimation,

        [ModifierFiles]
        Special,

        [ModifierFiles]
        WeatherEffect,
    }
}