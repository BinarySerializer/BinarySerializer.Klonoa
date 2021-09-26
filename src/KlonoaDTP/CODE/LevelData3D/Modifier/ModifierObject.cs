using System;
using System.Linq;
using System.Reflection;
using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    // A GameObject for Klonoa. We call it a modifier here since it's not necessarily an in-game object, but rather anything which updates
    // every frame. Most objects will draw an object on screen, while others can do other things like modify the VRAM.

    // The game defines 24 secondary types by using the type as an index in a function table. This table is located in the code files
    // for each level block and thus will differ. For Vision 1-1 NTSC the function pointer table is at 0x80110808. A lot of the pointers
    // are nulled out, so you would believe the actual indices themselves will be globally the same, with each level only implementing
    // functions for the used ones, but oddly enough the indices differ between levels.

    public class ModifierObject : BinarySerializable
    {
        public ArchiveFile Pre_AdditionalLevelFilePack { get; set; }

        public short Short_00 { get; set; }
        public short Short_02 { get; set; }
        public int Int_04 { get; set; }
        public PrimaryObjectType PrimaryType { get; set; }
        public short SecondaryType { get; set; }
        public short Short_0C { get; set; }
        public short Short_0E { get; set; }
        public Pointer ParametersPointer { get; set; } // Pointer to object parameters. The data is different based on the type.
        public Pointer DataFileIndicesPointer { get; set; }
        public short Short_18 { get; set; }
        public short Short_1A { get; set; } // Seems to be used in memory to indicate if it's been loaded

        public bool IsInvalid => PrimaryType == PrimaryObjectType.Invalid || PrimaryType == PrimaryObjectType.None ||
                                 SecondaryType == -1 || SecondaryType == 0;

        // Serialized from pointers
        public ushort[] DataFileIndices { get; set; }

        // Parameters
        public ModifierObjectParams_MovingPlatform Params_MovingPlatform { get; set; }

        // Data files
        public RawData_File[] Data_Unknown { get; set; }
        public PS1_TMD Data_TMD { get; set; }
        public PS1_TMD Data_TMD_Secondary { get; set; }
        public ObjCollisionItems_File Data_Collision { get; set; }
        public MovementPath_File Data_MovementPaths { get; set; }
        public UnknownModelObjectsData_File Data_UnknownModelObjectsData { get; set; }
        public PS1_TIM Data_TIM { get; set; }
        public ObjPositions_File Data_LightPositions { get; set; } // Each light has two positions, source and destination
        public ObjTransform_ArchiveFile Data_LocalTransform { get; set; }
        public ObjTransform_ArchiveFile Data_AbsoluteTransform { get; set; }
        public ArchiveFile<ObjTransform_ArchiveFile> Data_LocalTransforms { get; set; }
        public ArchiveFile<ObjTransform_ArchiveFile> Data_AbsoluteTransforms { get; set; }
        public KlonoaVector16 Data_Position { get; set; }
        public TIM_ArchiveFile Data_TextureAnimation { get; set; }
        public PaletteAnimation_ArchiveFile Data_PaletteAnimation { get; set; }
        public UVScrollAnimation_File Data_UVScrollAnimation { get; set; }
        public ObjPositions_File Data_ScenerySprites { get; set; }

        // Custom
        public GlobalModifierType GlobalModifierType { get; set; }
        public float? ConstantRotationX { get; set; }
        public float? ConstantRotationY { get; set; }
        public float? ConstantRotationZ { get; set; }
        public float AnimatedTransformSpeed { get; set; } = 1;
        public bool DoesAnimatedTransformPingPong { get; set; }
        public LoaderConfiguration_DTP.TextureAnimationInfo TextureAnimationInfo { get; set; }
        public LoaderConfiguration_DTP.PaletteAnimationInfo PaletteAnimationInfo { get; set; }
        public PS1_VRAMRegion[] PaletteAnimationVRAMRegions { get; set; }
        public uint GeyserPlatformPositionsPointer { get; set; }
        public GeyserPlatformPosition[] GeyserPlatformPositions { get; set; }
        public LoaderConfiguration_DTP.VRAMScrollInfo[] VRAMScrollInfos { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
            Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
            Int_04 = s.Serialize<int>(Int_04, name: nameof(Int_04));
            PrimaryType = s.Serialize<PrimaryObjectType>(PrimaryType, name: nameof(PrimaryType));
            SecondaryType = s.Serialize<short>(SecondaryType, name: nameof(SecondaryType));
            Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
            Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));
            ParametersPointer = s.SerializePointer(ParametersPointer, name: nameof(ParametersPointer));
            DataFileIndicesPointer = s.SerializePointer(DataFileIndicesPointer, name: nameof(DataFileIndicesPointer));
            Short_18 = s.Serialize<short>(Short_18, name: nameof(Short_18));
            Short_1A = s.Serialize<short>(Short_1A, name: nameof(Short_1A));

            s.DoAt(DataFileIndicesPointer, () => DataFileIndices = s.SerializeArray<ushort>(DataFileIndices, 8, name: nameof(DataFileIndices)));

            if (IsInvalid) 
                return;
            
            // Determine the type
            var loader = Loader_DTP.GetLoader(s.Context);
            GlobalModifierType = loader.Config.GetGlobalModifierType(loader.BINBlock, (int)PrimaryType, SecondaryType);

            // Serialize the parameters
            switch (GlobalModifierType)
            {
                case GlobalModifierType.Gondola:
                case GlobalModifierType.VerticallyMovingWoodenPlatform:
                    s.DoAt(ParametersPointer, () => Params_MovingPlatform = s.SerializeObject<ModifierObjectParams_MovingPlatform>(Params_MovingPlatform, name: nameof(Params_MovingPlatform)));
                    break;
            }
        }

        public void SerializeDataFiles(SerializerObject s)
        {
            if (IsInvalid)
                return;

            if (GlobalModifierType == GlobalModifierType.Unknown)
            {
                var count = DataFileIndices?.Select((x, i) => new { x, i }).ToList().FindIndex(x => x.x == 0 && x.i > 0);
                s.LogWarning($"Unknown modifier at {Offset} with {count ?? 0} data files");

                Data_Unknown = new RawData_File[count ?? 0];

                if (count == null)
                    return;

                for (int i = 0; i < count; i++)
                    Data_Unknown[i] = SerializeDataFile<RawData_File>(s, Data_Unknown[i], name: $"{nameof(Data_Unknown)}[{i}]");

                return;
            }

            var loader = Loader_DTP.GetLoader(s.Context);

            switch (GlobalModifierType)
            {
                case GlobalModifierType.Unknown:
                    break;
                
                case GlobalModifierType.WindSwirl:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_TMD_Secondary = SerializeDataFile<PS1_TMD>(s, Data_TMD_Secondary, name: nameof(Data_TMD_Secondary));
                    Data_Position = SerializeDataFile<KlonoaVector16>(s, Data_Position, name: nameof(Data_Position));
                    ConstantRotationY = 128;
                    break;
                
                case GlobalModifierType.BigWindmill:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform, 
                        onPreSerialize: x => x.Pre_UsesTransformInfo = true, name: nameof(Data_AbsoluteTransform));
                    Data_TIM = SerializeDataFile<PS1_TIM>(s, Data_TIM, name: nameof(Data_TIM));
                    ConstantRotationZ = -9;
                    break;

                case GlobalModifierType.SmallWindmill:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = true, name: nameof(Data_AbsoluteTransform));
                    ConstantRotationZ = -24;
                    break;

                case GlobalModifierType.MovingPlatform:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    AnimatedTransformSpeed = 1;
                    break;

                case GlobalModifierType.RoadSign:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = true, name: nameof(Data_AbsoluteTransform));
                    break;

                case GlobalModifierType.TiltRock: // FUN_4_8__8011fff8
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    SkipDataFile<RawData_File>(s); // TODO: Camera block (parsed at 0x800816e8)

                    AnimatedTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.Minecart:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_AbsoluteTransforms = SerializeDataFile<ArchiveFile<ObjTransform_ArchiveFile>>(s, Data_AbsoluteTransforms,
                        onPreSerialize: x => x.Pre_OnPreSerializeAction = t => t.Pre_UsesTransformInfo = true, name: nameof(Data_AbsoluteTransforms));
                    SkipDataFile<RawData_File>(s); // TODO: Camera block (parsed at 0x800816e8)
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = true, name: nameof(Data_LocalTransform));

                    AnimatedTransformSpeed = 1;
                    break;

                case GlobalModifierType.RongoLango:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    SkipDataFile<ArchiveFile<RawData_ArchiveFile>>(s); // TODO: Parts
                    SkipDataFile<RawData_File>(s); // TODO: Unknown data
                    SkipDataFile<RawData_File>(s); // TODO: Unknown data
                    SkipDataFile<RawData_File>(s); // TODO: Unknown data
                    SkipDataFile<RawData_ArchiveFile>(s); // TODO: Unknown data
                    SkipDataFile<RawData_ArchiveFile>(s); // TODO: Palettes
                    break;

                case GlobalModifierType.Bell:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Position = SerializeDataFile<KlonoaVector16>(s, Data_Position, name: nameof(Data_Position));
                    break;

                case GlobalModifierType.LockedDoor_0: // FUN_800790e4
                case GlobalModifierType.LockedDoor_1:
                case GlobalModifierType.LockedDoor_2:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));

                    if (GlobalModifierType != GlobalModifierType.LockedDoor_2)
                        Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                            onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform));

                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    if (GlobalModifierType == GlobalModifierType.LockedDoor_0)
                        SkipDataFile<RawData_File>(s); // TODO: Unused?

                    AnimatedTransformSpeed = 1;
                    break;

                case GlobalModifierType.WaterWheel:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    ConstantRotationZ = -2;
                    break;

                case GlobalModifierType.Crate:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    AnimatedTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.MultiWheel:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    break;

                case GlobalModifierType.Gondola: // FUN_9_8__80120d24
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    // TODO: Sometimes has two additional transforms - unused?

                    AnimatedTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    DoesAnimatedTransformPingPong = true;
                    break;

                case GlobalModifierType.FallingTreePart: // FUN_9_8__80122870
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    AnimatedTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.WoodenCart: // FUN_9_8__80122cfc
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_AbsoluteTransforms = SerializeDataFile<ArchiveFile<ObjTransform_ArchiveFile>>(s, Data_AbsoluteTransforms,
                        onPreSerialize: x => x.Pre_OnPreSerializeAction = t => t.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransforms));
                    SkipDataFile<RawData_File>(s); // TODO: Camera block (parsed at 0x800816e8)
                    // NOTE: The positions file in here only has one entry. The game parses 3.
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = true, name: nameof(Data_LocalTransform));

                    // Correct overflow values
                    foreach (var pos in Data_LocalTransform.Positions.Positions.SelectMany(x => x))
                    {
                        pos.X = 0;
                        pos.Y = 0;
                        pos.Z = 0;
                    }

                    AnimatedTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.WoodenMallet:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_LocalTransforms = SerializeDataFile<ArchiveFile<ObjTransform_ArchiveFile>>(s, Data_LocalTransforms,
                        onPreSerialize: x => x.Pre_OnPreSerializeAction = f => f.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransforms));
                    SkipDataFile<RawData_File>(s); // TODO: Camera block (parsed at 0x800816e8)

                    AnimatedTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.VerticallyMovingWoodenPlatform:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform));

                    AnimatedTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    DoesAnimatedTransformPingPong = true;
                    break;

                case GlobalModifierType.Cogwheel:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    // TODO: Constant rotation
                    break;

                case GlobalModifierType.Light:
                    if (Short_00 == 0x11)
                        Data_LightPositions = SerializeDataFile<ObjPositions_File>(s, Data_LightPositions, name: nameof(Data_LightPositions));
                    else
                        Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    break;

                case GlobalModifierType.GeyserPlatform:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));

                    GeyserPlatformPositionsPointer = loader.Config.GeyserPlatformPositionsPointers[loader.GlobalSectorIndex];
                    s.DoAt(new Pointer(GeyserPlatformPositionsPointer, loader.FindCodeFile(GeyserPlatformPositionsPointer)), () =>
                    {
                        GeyserPlatformPositions = s.SerializeObjectArrayUntil(GeyserPlatformPositions, x => x.Ushort_06 == 0, () => new GeyserPlatformPosition()
                        {
                            Position = new KlonoaVector16()
                        }, name: nameof(GeyserPlatformPositions));
                    });
                    break;

                case GlobalModifierType.ScrollAnimation:
                    Data_UVScrollAnimation = SerializeDataFile<UVScrollAnimation_File>(s, Data_UVScrollAnimation, name: nameof(Data_UVScrollAnimation));
                    break;

                case GlobalModifierType.VRAMScrollAnimation:
                    VRAMScrollInfos = loader.Config.VRAMScrollInfos[loader.BINBlock];
                    break;

                case GlobalModifierType.Object:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    break;

                case GlobalModifierType.LevelModelSection:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_UnknownModelObjectsData = SerializeDataFile<UnknownModelObjectsData_File>(s, Data_UnknownModelObjectsData, 
                        onPreSerialize: x => x.Pre_ObjsCount = Data_TMD.ObjectsCount, name: nameof(Data_UnknownModelObjectsData)); // TODO: Parsed at FUN_8002692c
                    break;

                case GlobalModifierType.ScenerySprites:
                    Data_ScenerySprites = SerializeDataFile<ObjPositions_File>(s, Data_ScenerySprites, name: nameof(Data_ScenerySprites));
                    break;

                case GlobalModifierType.TextureAnimation:
                    Data_TextureAnimation = SerializeDataFile<TIM_ArchiveFile>(s, Data_TextureAnimation, name: nameof(Data_TextureAnimation));
                    
                    TextureAnimationInfo = loader.Config.TextureAnimationInfos[loader.BINBlock];
                    break;

                case GlobalModifierType.PaletteAnimation:
                    Data_PaletteAnimation = SerializeDataFile<PaletteAnimation_ArchiveFile>(s, Data_PaletteAnimation, name: nameof(Data_PaletteAnimation));
                    
                    PaletteAnimationInfo = loader.Config.PaletteAnimationInfos[loader.BINBlock];
                    s.DoAt(new Pointer(PaletteAnimationInfo.Address_Regions, loader.FindCodeFile(PaletteAnimationInfo.Address_Regions)), () =>
                    {
                        var count = Data_PaletteAnimation.OffsetTable.FilesCount;
                        PaletteAnimationVRAMRegions = s.SerializeObjectArray<PS1_VRAMRegion>(PaletteAnimationVRAMRegions, count, name: nameof(PaletteAnimationVRAMRegions));
                    });
                    break;

                case GlobalModifierType.Special:
                    break;

                case GlobalModifierType.WeatherEffect:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (DataFileIndices != null && DataFileIndices[_dataFileIndex] != 0)
                s.LogWarning($"Modifier of type {GlobalModifierType} has unparsed data file(s)");
        }

        private int _dataFileIndex;
        private T SerializeDataFile<T>(SerializerObject s, T obj, Action<T> onPreSerialize = null, bool logIfNotFullyParsed = true, string name = null)
            where T : BinarySerializable, new()
        {
            if (typeof(T) == typeof(PS1_TMD))
                logIfNotFullyParsed = false;

            return Pre_AdditionalLevelFilePack.SerializeFile<T>(
                s: s, 
                obj: obj, 
                index: DataFileIndices[_dataFileIndex++], 
                onPreSerialize: onPreSerialize, 
                logIfNotFullyParsed: logIfNotFullyParsed, 
                name: name);
        }
        private void SkipDataFile<T>(SerializerObject s)
            where T : BinarySerializable, new()
        {
            s.LogWarning($"Data file skipped at index {_dataFileIndex} for object of type {GlobalModifierType}");
            SerializeDataFile<T>(s, null, name: "Unknown");
        }

        public class GeyserPlatformPosition : BinarySerializable
        {
            public KlonoaVector16 Position { get; set; }
            public ushort Ushort_06 { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Position = s.SerializeObject<KlonoaVector16>(Position, name: nameof(Position));
                Ushort_06 = s.Serialize<ushort>(Ushort_06, name: nameof(Ushort_06));
            }
        }
    }
}