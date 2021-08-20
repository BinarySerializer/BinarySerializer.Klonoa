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

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Collision, GlobalModifierFileType.MovementPaths, GlobalModifierFileType.Transform_WithoutInfo, GlobalModifierFileType.Unknown)]
        TiltRock,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.Collision, GlobalModifierFileType.MovementPaths, GlobalModifierFileType.Transforms_WithInfo, GlobalModifierFileType.Unknown, GlobalModifierFileType.Transform_WithInfo)]
        Minecart,

        [ModifierFiles(GlobalModifierFileType.UVScrollAnimation)]
        ScrollAnimation,

        [ModifierFiles(GlobalModifierFileType.TMD)]
        Object,

        [ModifierFiles(GlobalModifierFileType.TMD, GlobalModifierFileType.UnknownModelObjectsData)]
        LevelModelSection,

        [ModifierFiles(GlobalModifierFileType.ScenerySprites)]
        ScenerySprites,

        [ModifierFiles(GlobalModifierFileType.TextureAnimation)]
        TextureAnimation,

        [ModifierFiles(GlobalModifierFileType.PaletteAnimation)]
        PaletteAnimation,

        [ModifierFiles]
        Special,
    }
}