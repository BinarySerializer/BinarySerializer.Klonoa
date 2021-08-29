namespace BinarySerializer.Klonoa.DTP
{
    public enum GlobalModifierFileType
    {
        None,
        
        Unknown,
        UnknownArchive,
        UnknownArchiveArchive,

        TMD,
        Collision,
        MovementPaths,
        UnknownModelObjectsData,
        TIM,
        LightObject,

        Transform_WithInfo,
        Transform_WithoutInfo,
        Transforms_WithInfo,
        Position,
        
        TextureAnimation,
        PaletteAnimation,
        UVScrollAnimation,
        
        ScenerySprites,
    }
}