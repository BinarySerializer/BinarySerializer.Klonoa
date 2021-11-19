using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    // The data for a GameObject in Klonoa. The game defines 24 secondary types by using the type as an index in a function table. This
    // table is located in the code files for each level block and thus will differ. For Vision 1-1 NTSC the function pointer table is
    // at 0x80110808. A GameObject is anything which updates every frame. Most objects have models, but not all.
    public class GameObjectData
    {
        public GlobalGameObjectType GlobalGameObjectType { get; set; }
        public PrimaryObjectType PrimaryType { get; set; }
        public int SecondaryType { get; set; }
        public Pointer DefinitionOffset { get; set; }

        // Data files
        public RawData_File[] UnknownData { get; set; }
        public CollisionTriangles_File Collision { get; set; }
        public MovementPath_File MovementPaths { get; set; }
        public ArchiveFile<MovementPath_File> MovementPathsArchive { get; set; }
        public PS1_TIM TIM { get; set; }
        public TIM_ArchiveFile TIMArchive { get; set; }
        public GameObjectData_Sprites[] Sprites { get; set; }
        public VectorAnimation_File LightPositions { get; set; } // Each light has two positions, source and destination
        public ModelAnimation_ArchiveFile AbsoluteTransform { get; set; }
        public ArchiveFile<ModelAnimation_ArchiveFile> AbsoluteTransforms { get; set; }
        public KlonoaVector16 Position { get; set; }
        public KlonoaVector16 Rotation { get; set; }
        public CameraAnimations_File CameraAnimations { get; set; }
        public TIM_ArchiveFile TextureAnimation { get; set; }
        public PaletteAnimation_ArchiveFile PaletteAnimation { get; set; }
        public PaletteAnimations_ArchiveFile PaletteAnimations { get; set; }
        public UVScrollAnimation_File UVScrollAnimation { get; set; }
        public RGBAnimations_File RGBAnimations { get; set; }
        public VectorAnimation_File ScenerySprites { get; set; }
        public RawData_File RawVRAMData { get; set; }
        public PS1_VRAMRegion RawVRAMDataRegion { get; set; }
        public VectorAnimation_File Positions { get; set; }
        public VectorAnimation_File Rotations { get; set; }

        // Custom
        public GameObjectData_Model[] Models { get; set; }
        public GameObjectData_RawTextureAnimation RawTextureAnimation { get; set; }
        public float AnimatedAbsoluteTransformSpeed { get; set; } = 1;
        public bool DoesAnimatedAbsoluteTransformPingPong { get; set; }
        public KlonoaSettings_DTP.TextureAnimationInfo TextureAnimationInfo { get; set; }
        public KlonoaSettings_DTP.PaletteAnimationInfo PaletteAnimationInfo { get; set; }
        public PS1_VRAMRegion[] PaletteAnimationVRAMRegions { get; set; }
        public uint GeyserPlatformPositionsPointer { get; set; }
        public GameObjectData_GeyserPlatformPosition[] GeyserPlatformPositions { get; set; }
        public KlonoaSettings_DTP.VRAMScrollInfo[] VRAMScrollInfos { get; set; }
        public bool IsMovementPathAbsolute { get; set; }
        public bool IsCollisionAbsolute { get; set; }
        public bool ShowAllModels { get; set; } = true; // If false then only one should show at once
    }
}