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
        public ObjTransform_ArchiveFile Data_LocalTransform_Secondary { get; set; } // TODO: Get rid of and use fake archive
        public ObjTransform_ArchiveFile Data_AbsoluteTransform { get; set; }
        public ArchiveFile<ObjTransform_ArchiveFile> Data_LocalTransforms { get; set; }
        public ArchiveFile<ObjTransform_ArchiveFile> Data_AbsoluteTransforms { get; set; }
        public KlonoaVector16 Data_Position { get; set; }
        public TIM_ArchiveFile Data_TextureAnimation { get; set; }
        public PaletteAnimation_ArchiveFile Data_PaletteAnimation { get; set; }
        public PaletteAnimations_ArchiveFile Data_PaletteAnimations { get; set; }
        public UVScrollAnimation_File Data_UVScrollAnimation { get; set; }
        public RGBAnimations_File Data_RGBAnimations { get; set; }
        public ObjPositions_File Data_ScenerySprites { get; set; }

        // Custom
        public GlobalModifierType GlobalModifierType { get; set; }
        public float? ConstantRotationX { get; set; }
        public float? ConstantRotationY { get; set; }
        public float? ConstantRotationZ { get; set; }
        public float ConstantRotationMin { get; set; } = -0x800;
        public float ConstantRotationLength { get; set; } = 0x1000;
        public float AnimatedLocalTransformSpeed { get; set; } = 1;
        public bool DoesAnimatedLocalTransformPingPong { get; set; }
        public float AnimatedAbsoluteTransformSpeed { get; set; } = 1;
        public bool DoesAnimatedAbsoluteTransformPingPong { get; set; }
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
                case GlobalModifierType.MovingPlatformOnTrack:
                case GlobalModifierType.MovingPlatformWithOptionalLocal:
                    s.DoAt(ParametersPointer, () => Params_MovingPlatform = s.SerializeObject<ModifierObjectParams_MovingPlatform>(Params_MovingPlatform, name: nameof(Params_MovingPlatform)));
                    break;
            }
        }

        public void SerializeDataFiles(SerializerObject s)
        {
            if (IsInvalid)
                return;

            s.Log($"Serializing data files for type {GlobalModifierType}");

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

                    AnimatedAbsoluteTransformSpeed = 1;
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

                    AnimatedAbsoluteTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.Minecart: // FUN_4_8__8011a638
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_AbsoluteTransforms = SerializeDataFile<ArchiveFile<ObjTransform_ArchiveFile>>(s, Data_AbsoluteTransforms,
                        onPreSerialize: x => x.Pre_OnPreSerializeAction = t => t.Pre_UsesTransformInfo = true, name: nameof(Data_AbsoluteTransforms));
                    SkipDataFile<RawData_File>(s); // TODO: Camera block (parsed at 0x800816e8)
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = true, name: nameof(Data_LocalTransform));

                    AnimatedAbsoluteTransformSpeed = 1;
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

                    if (GlobalModifierType != GlobalModifierType.LockedDoor_2)
                        AnimatedLocalTransformSpeed = 1;
                    else
                        AnimatedAbsoluteTransformSpeed = 1;

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

                    AnimatedAbsoluteTransformSpeed = 0.5f;
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

                    AnimatedLocalTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    DoesAnimatedLocalTransformPingPong = true;
                    break;

                case GlobalModifierType.FallingTreePart: // FUN_9_8__80122870
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    AnimatedAbsoluteTransformSpeed = 0.5f;
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

                    AnimatedAbsoluteTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.WoodenMallet:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_LocalTransforms = SerializeDataFile<ArchiveFile<ObjTransform_ArchiveFile>>(s, Data_LocalTransforms,
                        onPreSerialize: x => x.Pre_OnPreSerializeAction = f => f.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransforms));
                    SkipDataFile<RawData_File>(s); // TODO: Camera block (parsed at 0x800816e8)

                    AnimatedLocalTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.VerticallyMovingWoodenPlatform:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform));

                    AnimatedLocalTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    DoesAnimatedLocalTransformPingPong = true;
                    break;

                case GlobalModifierType.Cogwheel: // FUN_10_8__8011f560
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    var sector = loader.GlobalSectorIndex;

                    if (sector == 0x55)
                        ConstantRotationX = 4;
                    else if (sector == 0x50 || sector == 0x52)
                        ConstantRotationZ = 4;

                    ConstantRotationMin = -341;
                    ConstantRotationLength = 340;

                    break;

                case GlobalModifierType.SpinningWood: // FUN_10_8__8012059c
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    if (Short_02 == 0)
                    {
                        AnimatedAbsoluteTransformSpeed = 0.5f;
                        DoesAnimatedAbsoluteTransformPingPong = true;
                    }
                    else if (Short_02 == 1)
                    {
                        ConstantRotationZ = 24;
                    }
                    break;

                case GlobalModifierType.SpinningWoodAttachedPlatform: // FUN_10_8__801201ec
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    AnimatedLocalTransformSpeed = 0.5f;
                    DoesAnimatedLocalTransformPingPong = true;
                    break;

                case GlobalModifierType.MovingLedge: // FUN_10_8__8011f198
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    AnimatedAbsoluteTransformSpeed = 1;
                    break;

                case GlobalModifierType.Ledge: // FUN_12_8__8011a74c
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    Data_TMD_Secondary = SerializeDataFile<PS1_TMD>(s, Data_TMD_Secondary, name: nameof(Data_TMD_Secondary));
                    break;

                case GlobalModifierType.UnstablePlatform: // FUN_12_8__8011b2f0
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform));
                    Data_LocalTransform_Secondary = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform_Secondary,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform_Secondary));

                    AnimatedLocalTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.SwingingPlatform: // FUN_12_8__8011af80
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform));

                    AnimatedLocalTransformSpeed = 0xb00 / (float)0x1000;
                    break;

                case GlobalModifierType.Bone:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    break;

                case GlobalModifierType.GreenBoulder:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    Data_TMD_Secondary = SerializeDataFile<PS1_TMD>(s, Data_TMD_Secondary, name: nameof(Data_TMD_Secondary));
                    break;

                case GlobalModifierType.RedBoulder:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_TMD_Secondary = SerializeDataFile<PS1_TMD>(s, Data_TMD_Secondary, name: nameof(Data_TMD_Secondary));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    break;

                case GlobalModifierType.DestroyedHouse:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_TMD_Secondary = SerializeDataFile<PS1_TMD>(s, Data_TMD_Secondary, name: nameof(Data_TMD_Secondary));
                    break;

                case GlobalModifierType.BlockingBoulder:
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    break;

                case GlobalModifierType.SwingingWoodPlank: // FUN_14_7__8011648c
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    break;

                case GlobalModifierType.Collision: // FUN_14_7__801170a8
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    break;

                case GlobalModifierType.Rocks: // FUN_14_7__8011724c
                    Data_TMD_Secondary = SerializeDataFile<PS1_TMD>(s, Data_TMD_Secondary, name: nameof(Data_TMD_Secondary));
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform));
                    SkipDataFile<RawData_File>(s, isUnused: true);
                    break;

                case GlobalModifierType.MovingPlatformOnTrack: // FUN_15_8__8011d040
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform));

                    AnimatedLocalTransformSpeed = 0.5f;
                    AnimatedAbsoluteTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    DoesAnimatedAbsoluteTransformPingPong = true;
                    break;

                case GlobalModifierType.SpinningWheel: // FUN_15_8__8011a9dc
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    ConstantRotationZ = 24;
                    AnimatedAbsoluteTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.FallingTargetPlatform: // FUN_15_8__8011af24
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    Data_LocalTransforms = new ArchiveFile<ObjTransform_ArchiveFile>()
                    {
                        Files = new ObjTransform_ArchiveFile[3]
                    };

                    Data_LocalTransforms.Files[0] = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransforms.Files[0],
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: $"{nameof(Data_LocalTransforms)}[{0}]");
                    Data_LocalTransforms.Files[1] = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransforms.Files[1],
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: $"{nameof(Data_LocalTransforms)}[{1}]");
                    Data_LocalTransforms.Files[2] = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransforms.Files[2],
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: $"{nameof(Data_LocalTransforms)}[{2}]");

                    AnimatedLocalTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.LockedDoor_3: // FUN_15_8__8011e0c0
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    AnimatedAbsoluteTransformSpeed = 0.5f;
                    break;

                case GlobalModifierType.BlockingLedge: // FUN_15_8__8011ee5c
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    SkipDataFile<RawData_File>(s, isUnused: true);

                    AnimatedAbsoluteTransformSpeed = 0xf00 / (float)0x1000;
                    break;

                case GlobalModifierType.UnknownOrbRelatedObj: // FUN_15_8__8011dc04
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));

                    Data_AbsoluteTransforms = new ArchiveFile<ObjTransform_ArchiveFile>()
                    {
                        Files = new ObjTransform_ArchiveFile[2]
                    };

                    Data_AbsoluteTransforms.Files[0] = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransforms.Files[0],
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: $"{nameof(Data_AbsoluteTransforms)}[{0}]");
                    Data_AbsoluteTransforms.Files[1] = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransforms.Files[1],
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: $"{nameof(Data_AbsoluteTransforms)}[{1}]");

                    AnimatedLocalTransformSpeed = 0x2aa / (float)0x1000;
                    break;

                case GlobalModifierType.MovingPlatformWithOptionalLocal: // FUN_16_8__8011d3ac
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_MovementPaths = SerializeDataFile<MovementPath_File>(s, Data_MovementPaths, name: nameof(Data_MovementPaths));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));

                    if (Short_02 == 1)
                        Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform));
                    else if (DataFileIndices[4] != 0)
                        SkipDataFile<RawData_File>(s, isUnused: true);

                    AnimatedLocalTransformSpeed = -0.5f;
                    AnimatedAbsoluteTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    DoesAnimatedAbsoluteTransformPingPong = true;
                    break;

                case GlobalModifierType.MovingWallPillars: // FUN_16_8__80120140
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform));

                    AnimatedLocalTransformSpeed = 0.5f;
                    DoesAnimatedLocalTransformPingPong = true;
                    break;

                case GlobalModifierType.DarkLightPlatform: // FUN_16_8__8011f698
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));
                    Data_TMD_Secondary = SerializeDataFile<PS1_TMD>(s, Data_TMD_Secondary, name: nameof(Data_TMD_Secondary));
                    Data_AbsoluteTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_AbsoluteTransform));
                    break;

                case GlobalModifierType.DarkLightSwitcher: // FUN_16_8__801229b8
                    // Has no data. Switches between the dark/light state in the level.
                    break;

                case GlobalModifierType.IronGate: // FUN_16_8__8011fa54
                    Data_TMD = SerializeDataFile<PS1_TMD>(s, Data_TMD, name: nameof(Data_TMD));
                    Data_Collision = SerializeDataFile<ObjCollisionItems_File>(s, Data_Collision, name: nameof(Data_Collision));

                    Data_AbsoluteTransforms = new ArchiveFile<ObjTransform_ArchiveFile>()
                    {
                        Files = new ObjTransform_ArchiveFile[4]
                    };

                    Data_AbsoluteTransforms.Files[0] = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransforms.Files[0],
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: $"{nameof(Data_AbsoluteTransforms)}[{0}]");
                    Data_AbsoluteTransforms.Files[1] = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransforms.Files[1],
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: $"{nameof(Data_AbsoluteTransforms)}[{1}]");
                    Data_AbsoluteTransforms.Files[2] = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransforms.Files[2],
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: $"{nameof(Data_AbsoluteTransforms)}[{2}]");
                    Data_AbsoluteTransforms.Files[3] = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_AbsoluteTransforms.Files[3],
                        onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: $"{nameof(Data_AbsoluteTransforms)}[{3}]");

                    AnimatedAbsoluteTransformSpeed = 0.5f;
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

                case GlobalModifierType.VRAMScrollAnimationWithTexture:
                    Data_TIM = SerializeDataFile<PS1_TIM>(s, Data_TIM, name: nameof(Data_TIM));
                    VRAMScrollInfos = loader.Config.VRAMScrollInfos[loader.BINBlock];
                    break;

                case GlobalModifierType.RGBAnimation:
                    Data_RGBAnimations = SerializeDataFile<RGBAnimations_File>(s, Data_RGBAnimations, name: nameof(Data_RGBAnimations));
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
                case GlobalModifierType.PaletteAnimations:
                    if (GlobalModifierType == GlobalModifierType.PaletteAnimation)
                        Data_PaletteAnimation = SerializeDataFile<PaletteAnimation_ArchiveFile>(s, Data_PaletteAnimation, name: nameof(Data_PaletteAnimation));
                    else
                        Data_PaletteAnimations = SerializeDataFile<PaletteAnimations_ArchiveFile>(s, Data_PaletteAnimations, name: nameof(Data_PaletteAnimations));

                    PaletteAnimationInfo = loader.Config.PaletteAnimationInfos[loader.BINBlock];

                    Pointer p;

                    if (GlobalModifierType == GlobalModifierType.PaletteAnimation && PaletteAnimationInfo.BlocksCount == -1)
                        p = new Pointer(PaletteAnimationInfo.Address_Regions + (Short_02 * 8), loader.FindCodeFile(PaletteAnimationInfo.Address_Regions));
                    else
                        p = new Pointer(PaletteAnimationInfo.Address_Regions, loader.FindCodeFile(PaletteAnimationInfo.Address_Regions));

                    s.DoAt(p, () =>
                    {
                        int count;
                        
                        if (GlobalModifierType == GlobalModifierType.PaletteAnimation && PaletteAnimationInfo.BlocksCount != -1)
                            count = PaletteAnimationInfo.BlocksCount;
                        else if (GlobalModifierType == GlobalModifierType.PaletteAnimation)
                            count = 1;
                        else
                            count = Data_PaletteAnimations.OffsetTable.FilesCount;

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
        private void SkipDataFile<T>(SerializerObject s, bool isUnused = false)
            where T : BinarySerializable, new()
        {
            if (!isUnused)
                s.LogWarning($"Data file skipped at index {_dataFileIndex} for object of type {GlobalModifierType}");
            
            SerializeDataFile<T>(s, null, name: isUnused ? "Unused" : "Unknown");
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