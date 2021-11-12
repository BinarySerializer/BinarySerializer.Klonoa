using BinarySerializer.PS1;
using System;
using System.Linq;

namespace BinarySerializer.Klonoa.DTP
{
    public class GameObjectDefinition : BinarySerializable
    {
        public ArchiveFile Pre_ObjectAssets { get; set; }

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

        // Serialized from pointers
        public ushort[] DataFileIndices { get; set; }

        // Parameters
        public GameObjectDefinitionParams_MovingPlatform Params_MovingPlatform { get; set; }

        // Data
        public GameObjectData Data { get; set; }

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

            if (PrimaryType == PrimaryObjectType.Invalid || PrimaryType == PrimaryObjectType.None || 
                SecondaryType == -1 || SecondaryType == 0)
            {
                Data = null;
                return;
            }

            var loader = Loader.GetLoader(s.Context);

            Data = new GameObjectData
            {
                // Determine the type
                GlobalGameObjectType = loader.Settings.GetGlobalGameObjectType(loader.BINBlock, (int) PrimaryType, SecondaryType),
                PrimaryType = PrimaryType,
                SecondaryType = SecondaryType,
                DefinitionOffset = Offset
            };

            // Serialize the parameters
            switch (Data.GlobalGameObjectType)
            {
                case GlobalGameObjectType.Gondola:
                case GlobalGameObjectType.VerticallyMovingWoodenPlatform:
                case GlobalGameObjectType.MovingPlatformOnTrack:
                case GlobalGameObjectType.MovingPlatformWithOptionalLocal:
                case GlobalGameObjectType.ObjectWithPaletteAnimation:
                case GlobalGameObjectType.MovingCavePlatform:
                    s.DoAt(ParametersPointer, () => Params_MovingPlatform = s.SerializeObject<GameObjectDefinitionParams_MovingPlatform>(Params_MovingPlatform, name: nameof(Params_MovingPlatform)));
                    break;
            }
        }

        public void SerializeData(SerializerObject s)
        {
            if (Data == null)
                return;

            s.Log($"Serializing data files for type {Data.GlobalGameObjectType}");

            if (Data.GlobalGameObjectType == GlobalGameObjectType.Unknown)
            {
                var count = DataFileIndices?.Select((x, i) => new { x, i }).ToList().FindIndex(x => x.x == 0 && x.i > 0);
                s.LogWarning($"Unknown game object at {Offset} with {count ?? 0} data files");

                Data.UnknownData = new RawData_File[count ?? 0];

                if (count == null)
                    return;

                for (int i = 0; i < count; i++)
                    Data.UnknownData[i] = SerializeDataFile<RawData_File>(s, Data.UnknownData[i], name: $"{nameof(Data.UnknownData)}[{i}]");

                return;
            }

            var loader = Loader.GetLoader(s.Context);

            switch (Data.GlobalGameObjectType)
            {
                case GlobalGameObjectType.Unknown:
                    break;
                
                case GlobalGameObjectType.WindSwirl:
                    CreateModels(2);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[1].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[1].TMD, modelIndex: 1, name: nameof(GameObjectData_Model.TMD));
                    Data.Position = SerializeDataFile<KlonoaVector16>(s, Data.Position, name: nameof(Data.Position));

                    Data.Models[0].ConstantRotation = new GameObjectData_ConstantRotation()
                    {
                        RotY = 128,
                    };
                    Data.Models[1].ConstantRotation = new GameObjectData_ConstantRotation()
                    {
                        RotY = 128,
                    };
                    Data.Models[1].Position = new KlonoaVector16(0, 182, 0);
                    break;
                
                case GlobalGameObjectType.BigWindmill:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform, 
                        onPreSerialize: x => x.Pre_UsesInfo = true, name: nameof(Data.AbsoluteTransform));
                    Data.TIM = SerializeDataFile<PS1_TIM>(s, Data.TIM, name: nameof(Data.TIM));

                    Data.Models[0].ConstantRotation = new GameObjectData_ConstantRotation()
                    {
                        RotZ = -9,
                    };
                    break;

                case GlobalGameObjectType.SmallWindmill:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = true, name: nameof(Data.AbsoluteTransform));

                    Data.Models[0].ConstantRotation = new GameObjectData_ConstantRotation()
                    {
                        RotZ = -24,
                    };
                    break;

                case GlobalGameObjectType.MovingPlatform:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.AnimatedAbsoluteTransformSpeed = 1;
                    break;

                case GlobalGameObjectType.RoadSign:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = true, name: nameof(Data.AbsoluteTransform));
                    break;

                case GlobalGameObjectType.TiltRock: // FUN_4_8__8011fff8
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    Data.CameraAnimations = SerializeDataFile<CameraAnimations_File>(s, Data.CameraAnimations, name: nameof(Data.CameraAnimations));

                    Data.AnimatedAbsoluteTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.Minecart: // FUN_4_8__8011a638
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.AbsoluteTransforms = SerializeDataFile<ArchiveFile<ModelAnimation_ArchiveFile>>(s, Data.AbsoluteTransforms,
                        onPreSerialize: x => x.Pre_OnPreSerializeAction = t => t.Pre_UsesInfo = true, name: nameof(Data.AbsoluteTransforms));
                    Data.CameraAnimations = SerializeDataFile<CameraAnimations_File>(s, Data.CameraAnimations, name: nameof(Data.CameraAnimations));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = true, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));

                    Data.AnimatedAbsoluteTransformSpeed = 1;
                    break;

                case GlobalGameObjectType.RongoLango:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, onPreSerialize: x => x.Pre_HasBones = true, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[0].RongoLangoModelAnimations = SerializeDataFile<ArchiveFile<RongoLangoModelBoneAnimation_ArchiveFile>>(s, Data.Models[0].RongoLangoModelAnimations, modelIndex: 0, name: nameof(GameObjectData_Model.RongoLangoModelAnimations));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    SkipDataFile<RawData_File>(s); // TODO: Vertex animation data
                    SkipDataFile<RawData_File>(s); // TODO: Vertex animation data
                    SkipDataFile<RawData_ArchiveFile>(s); // TODO: Unknown data
                    SkipDataFile<RawData_ArchiveFile>(s); // TODO: Palettes (for when hitting the boss)

                    Data.Models[0].ModelBoneAnimations = new GameObjectData_ModelBoneAnimations()
                    {
                        Animations = Data.Models[0].RongoLangoModelAnimations.Files.Select(x => new GameObjectData_ModelBoneAnimation
                        {
                            BoneRotations = x.Rotations,
                            BonePositions = x.Positions,
                        }).ToArray()
                    };
                    break;

                case GlobalGameObjectType.Bell:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Position = SerializeDataFile<KlonoaVector16>(s, Data.Position, name: nameof(Data.Position));
                    break;

                case GlobalGameObjectType.LockedDoor_0: // FUN_800790e4
                case GlobalGameObjectType.LockedDoor_1:
                case GlobalGameObjectType.LockedDoor_2:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));

                    if (Data.GlobalGameObjectType != GlobalGameObjectType.LockedDoor_2)
                        Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                            onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));

                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    if (Data.GlobalGameObjectType == GlobalGameObjectType.LockedDoor_0)
                        SkipDataFile<RawData_File>(s); // TODO: Unused?

                    if (Data.GlobalGameObjectType != GlobalGameObjectType.LockedDoor_2)
                        Data.Models[0].AnimatedLocalTransformSpeed = 1;
                    else
                        Data.AnimatedAbsoluteTransformSpeed = 1;

                    break;

                case GlobalGameObjectType.WaterWheel:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.Models[0].ConstantRotation = new GameObjectData_ConstantRotation()
                    {
                        RotZ = -2,
                    };
                    break;

                case GlobalGameObjectType.Crate:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.AnimatedAbsoluteTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.MultiWheel:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    break;

                case GlobalGameObjectType.Gondola: // FUN_9_8__80120d24
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[0].TMDObjectPositionOffsets = new KlonoaVector16[]
                    {
                        null,
                        new KlonoaVector16(0, -0x120, 0),
                    };
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    // TODO: Sometimes has two additional transforms - unused?

                    Data.AnimatedAbsoluteTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    Data.DoesAnimatedAbsoluteTransformPingPong = true;
                    break;

                case GlobalGameObjectType.FallingTreePart: // FUN_9_8__80122870
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.AnimatedAbsoluteTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.WoodenCart: // FUN_9_8__80122cfc
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.AbsoluteTransforms = SerializeDataFile<ArchiveFile<ModelAnimation_ArchiveFile>>(s, Data.AbsoluteTransforms,
                        onPreSerialize: x => x.Pre_OnPreSerializeAction = t => t.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransforms));
                    Data.CameraAnimations = SerializeDataFile<CameraAnimations_File>(s, Data.CameraAnimations, name: nameof(Data.CameraAnimations));
                    // NOTE: The positions file in here only has one entry. The game parses 3.
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = true, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));

                    // Correct overflow values
                    foreach (var pos in Data.Models[0].LocalTransform.Positions.Vectors.SelectMany(x => x))
                    {
                        pos.X = 0;
                        pos.Y = 0;
                        pos.Z = 0;
                    }

                    Data.AnimatedAbsoluteTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.WoodenMallet:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.Models[0].LocalTransforms = SerializeDataFile<ArchiveFile<ModelAnimation_ArchiveFile>>(s, Data.Models[0].LocalTransforms, onPreSerialize: x => x.Pre_OnPreSerializeAction = f => f.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransforms));
                    Data.CameraAnimations = SerializeDataFile<CameraAnimations_File>(s, Data.CameraAnimations, name: nameof(Data.CameraAnimations));

                    Data.Models[0].AnimatedLocalTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.VerticallyMovingWoodenPlatform:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));

                    Data.Models[0].AnimatedLocalTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    Data.Models[0].DoesAnimatedLocalTransformPingPong = true;
                    break;

                case GlobalGameObjectType.Cogwheel: // FUN_10_8__8011f560
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    int sector = loader.GlobalSectorIndex;

                    if (sector == 0x55)
                        Data.Models[0].ConstantRotation = new GameObjectData_ConstantRotation()
                        {
                            RotX = 4,
                            Min = -341,
                            Length = 340,
                        };
                    else if (sector == 0x50 || sector == 0x52)
                        Data.Models[0].ConstantRotation = new GameObjectData_ConstantRotation()
                        {
                            RotZ = 4,
                            Min = -341,
                            Length = 340,
                        };

                    break;

                case GlobalGameObjectType.SpinningWood: // FUN_10_8__8012059c
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    if (Short_02 == 0)
                    {
                        Data.AnimatedAbsoluteTransformSpeed = 0.5f;
                        Data.DoesAnimatedAbsoluteTransformPingPong = true;
                    }
                    else if (Short_02 == 1)
                    {
                        Data.Models[0].ConstantRotation = new GameObjectData_ConstantRotation()
                        {
                            RotZ = 24,
                        };
                    }
                    break;

                case GlobalGameObjectType.SpinningWoodAttachedPlatform: // FUN_10_8__801201ec
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));

                    Data.Models[0].AnimatedLocalTransformSpeed = 0.5f;
                    Data.Models[0].DoesAnimatedLocalTransformPingPong = true;
                    break;

                case GlobalGameObjectType.MovingLedge: // FUN_10_8__8011f198
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.AnimatedAbsoluteTransformSpeed = 1;
                    break;

                case GlobalGameObjectType.Ledge: // FUN_12_8__8011a74c
                    CreateModels(2);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    Data.Models[1].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[1].TMD, modelIndex: 1, name: nameof(GameObjectData_Model.TMD));
                    break;

                case GlobalGameObjectType.UnstablePlatform: // FUN_12_8__8011b2f0
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));
                    Data.Models[0].LocalTransform_Secondary = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform_Secondary, onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform_Secondary));

                    Data.Models[0].AnimatedLocalTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.SwingingPlatform: // FUN_12_8__8011af80
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));

                    Data.Models[0].AnimatedLocalTransformSpeed = 0xb00 / (float)0x1000;
                    break;

                case GlobalGameObjectType.Bone:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    break;

                case GlobalGameObjectType.GreenBoulder:
                    CreateModels(2);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    Data.Models[1].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[1].TMD, modelIndex: 1, name: nameof(GameObjectData_Model.TMD));
                    break;

                case GlobalGameObjectType.RedBoulder:
                    CreateModels(2);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[1].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[1].TMD, modelIndex: 1, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    break;

                case GlobalGameObjectType.DestroyedHouse:
                    CreateModels(2);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[1].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[1].TMD, modelIndex: 1, name: nameof(GameObjectData_Model.TMD));
                    break;

                case GlobalGameObjectType.BlockingBoulder:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    break;

                case GlobalGameObjectType.SwingingWoodPlank: // FUN_14_7__8011648c
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    break;

                case GlobalGameObjectType.Collision: // FUN_14_7__801170a8
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    break;

                case GlobalGameObjectType.Rocks: // FUN_14_7__8011724c
                    CreateModels(2);

                    Data.Models[1].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[1].TMD, modelIndex: 1, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));
                    SkipDataFile<RawData_File>(s, isUnused: true);
                    break;

                case GlobalGameObjectType.MovingPlatformOnTrack: // FUN_15_8__8011d040
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));

                    Data.Models[0].AnimatedLocalTransformSpeed = 0.5f;
                    Data.AnimatedAbsoluteTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    Data.DoesAnimatedAbsoluteTransformPingPong = true;
                    break;

                case GlobalGameObjectType.SpinningWheel: // FUN_15_8__8011a9dc
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.Models[0].ConstantRotation = new GameObjectData_ConstantRotation()
                    {
                        RotZ = 24,
                    };
                    Data.AnimatedAbsoluteTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.FallingTargetPlatform: // FUN_15_8__8011af24
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.Models[0].LocalTransforms ??= new ArchiveFile<ModelAnimation_ArchiveFile>()
                    {
                        Files = new ModelAnimation_ArchiveFile[3]
                    };

                    Data.Models[0].LocalTransforms.Files[0] = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransforms.Files[0], onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: $"{nameof(GameObjectData_Model.LocalTransforms)}[{0}]");
                    Data.Models[0].LocalTransforms.Files[1] = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransforms.Files[1], onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: $"{nameof(GameObjectData_Model.LocalTransforms)}[{1}]");
                    Data.Models[0].LocalTransforms.Files[2] = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransforms.Files[2], onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: $"{nameof(GameObjectData_Model.LocalTransforms)}[{2}]");

                    Data.Models[0].AnimatedLocalTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.LockedDoor_3: // FUN_15_8__8011e0c0
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.AnimatedAbsoluteTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.BlockingLedge: // FUN_15_8__8011ee5c
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    SkipDataFile<RawData_File>(s, isUnused: true);

                    Data.AnimatedAbsoluteTransformSpeed = 0xf00 / (float)0x1000;
                    break;

                case GlobalGameObjectType.UnknownOrbRelatedObj: // FUN_15_8__8011dc04
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));

                    Data.AbsoluteTransforms = new ArchiveFile<ModelAnimation_ArchiveFile>()
                    {
                        Files = new ModelAnimation_ArchiveFile[2]
                    };

                    Data.AbsoluteTransforms.Files[0] = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransforms.Files[0],
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: $"{nameof(Data.AbsoluteTransforms)}[{0}]");
                    Data.AbsoluteTransforms.Files[1] = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransforms.Files[1],
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: $"{nameof(Data.AbsoluteTransforms)}[{1}]");

                    Data.Models[0].AnimatedLocalTransformSpeed = 0x2aa / (float)0x1000;
                    break;

                case GlobalGameObjectType.MovingPlatformWithOptionalLocal: // FUN_16_8__8011d3ac
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    if (Short_02 == 1)
                        Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                            onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));
                    else if (DataFileIndices[4] != 0)
                        SkipDataFile<RawData_File>(s, isUnused: true);

                    Data.Models[0].AnimatedLocalTransformSpeed = -0.5f;
                    Data.AnimatedAbsoluteTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    Data.DoesAnimatedAbsoluteTransformPingPong = true;
                    break;

                case GlobalGameObjectType.MovingWallPillars: // FUN_16_8__80120140
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));

                    Data.Models[0].AnimatedLocalTransformSpeed = 0.5f;
                    Data.Models[0].DoesAnimatedLocalTransformPingPong = true;
                    break;

                case GlobalGameObjectType.DarkLightPlatform: // FUN_16_8__8011f698
                    CreateModels(2);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.Models[1].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[1].TMD, modelIndex: 1, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    break;

                case GlobalGameObjectType.DarkLightSwitcher: // FUN_16_8__801229b8
                    // Has no data. Switches between the dark/light state in the level.
                    break;

                case GlobalGameObjectType.IronGate: // FUN_16_8__8011fa54
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));

                    Data.AbsoluteTransforms = new ArchiveFile<ModelAnimation_ArchiveFile>()
                    {
                        Files = new ModelAnimation_ArchiveFile[4]
                    };

                    Data.AbsoluteTransforms.Files[0] = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransforms.Files[0],
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: $"{nameof(Data.AbsoluteTransforms)}[{0}]");
                    Data.AbsoluteTransforms.Files[1] = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransforms.Files[1],
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: $"{nameof(Data.AbsoluteTransforms)}[{1}]");
                    Data.AbsoluteTransforms.Files[2] = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransforms.Files[2],
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: $"{nameof(Data.AbsoluteTransforms)}[{2}]");
                    Data.AbsoluteTransforms.Files[3] = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransforms.Files[3],
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: $"{nameof(Data.AbsoluteTransforms)}[{3}]");

                    Data.AnimatedAbsoluteTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.BossUnknown: // FUN_17_7__80113b00
                    SkipDataFile<RawData_File>(s);
                    SkipDataFile<RawData_File>(s);
                    break;

                case GlobalGameObjectType.JokaSpinningCore: // FUN_17_7__80113854
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.Models[0].ConstantRotation = new GameObjectData_ConstantRotation()
                    {
                        RotY = 8,
                    };
                    break;

                case GlobalGameObjectType.TransparentGemPlatform: // FUN_18_8__8011eaec, FUN_19_8__8011d9b0
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    SkipDataFile<RawData_File>(s); // TODO: 20 byte structs
                    break;

                case GlobalGameObjectType.BirdStatueWithSwitch: // FUN_18_8__80121694
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    // NOTE: This loads a block into VRAM, seems to be for an effect only?
                    break;

                case GlobalGameObjectType.ObjectWithPaletteAnimation: // FUN_18_8__80123c18
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    Data.PaletteAnimation = SerializeDataFile<PaletteAnimation_ArchiveFile>(s, Data.PaletteAnimation, name: nameof(Data.PaletteAnimation));

                    Data.AnimatedAbsoluteTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    Data.DoesAnimatedAbsoluteTransformPingPong = true;

                    Data.PaletteAnimationInfo = loader.Settings.ObjectWithPaletteAnimationInfos[loader.BINBlock];

                    s.DoAt(new Pointer(Data.PaletteAnimationInfo.Address_Regions, loader.FindCodeFile(Data.PaletteAnimationInfo.Address_Regions)), () =>
                    {
                        Data.PaletteAnimationVRAMRegions = s.SerializeObjectArray<PS1_VRAMRegion>(Data.PaletteAnimationVRAMRegions, 1, name: nameof(Data.PaletteAnimationVRAMRegions));
                    });
                    break;

                case GlobalGameObjectType.LightField: // FUN_18_8__801226ec
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    break;

                case GlobalGameObjectType.MovingCavePlatform: // FUN_18_8__8012323c
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.AnimatedAbsoluteTransformSpeed = Params_MovingPlatform.AnimSpeed;
                    Data.DoesAnimatedAbsoluteTransformPingPong = true;
                    break;

                case GlobalGameObjectType.ColoredStatue: // FUN_19_8__80120a20
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));

                    Data.Models[0].AnimatedLocalTransformSpeed = 0x80 / (float)0x1000;
                    break;

                case GlobalGameObjectType.ColoredPillar: // FUN_19_8__8011bf40
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));

                    Data.Models[0].AnimatedLocalTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.ColoredDoor: // FUN_19_8__8011fc90
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.Models[0].AnimatedLocalTransformSpeed = 0xC00 / (float)0x1000;
                    Data.Models[0].DoesAnimatedLocalTransformPingPong = true;
                    break;

                case GlobalGameObjectType.OnWayMovingWallPillar: // FUN_19_8__8011f8fc
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));

                    Data.Models[0].AnimatedLocalTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.DoorWithPillar: // FUN_19_8__8012011c
                    CreateModels(2);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[1].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[1].TMD, modelIndex: 1, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[0].LocalTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.Models[0].LocalTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, modelIndex: 0, name: nameof(GameObjectData_Model.LocalTransform));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));

                    Data.Models[0].AnimatedLocalTransformSpeed = 0.5f;
                    break;

                case GlobalGameObjectType.CutsceneCrystal: // FUN_20_8__80117918
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    break;

                case GlobalGameObjectType.GhadiusCirclePlatform: // FUN_20_8__80116070
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    SkipDataFile<RawData_File>(s); // TODO: Movement paths? Aligned to 28.
                    break;

                case GlobalGameObjectType.NahatombSphere: // FUN_22_8__8011549c
                    CreateModels(2);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[1].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[1].TMD, modelIndex: 1, name: nameof(GameObjectData_Model.TMD));
                    Data.AbsoluteTransform = SerializeDataFile<ModelAnimation_ArchiveFile>(s, Data.AbsoluteTransform,
                        onPreSerialize: x => x.Pre_UsesInfo = false, name: nameof(Data.AbsoluteTransform));
                    //Data_LocalTransform = SerializeDataFile<ObjTransform_ArchiveFile>(s, Data_LocalTransform,
                    //    onPreSerialize: x => x.Pre_UsesTransformInfo = false, name: nameof(Data_LocalTransform));
                    SkipDataFile<RawData_File>(s); // TODO: Transform
                    SkipDataFile<RawData_File>(s); // TODO: 20 byte structs
                    break;

                case GlobalGameObjectType.NahatombEscaping: // FUN_22_8__8011be8c
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));

                    Data.Models[0].ConstantRotation = new GameObjectData_ConstantRotation()
                    {
                        RotY = 16,
                    };
                    break;

                case GlobalGameObjectType.NahatombPaletteAnimation: // FUN_23_7__8011499c
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.PaletteAnimation = SerializeDataFile<PaletteAnimation_ArchiveFile>(s, Data.PaletteAnimation, name: nameof(Data.PaletteAnimation));

                    Data.PaletteAnimationInfo = loader.Settings.PaletteAnimationInfos[loader.BINBlock][SecondaryType];

                    s.DoAt(new Pointer(Data.PaletteAnimationInfo.Address_Regions, loader.FindCodeFile(Data.PaletteAnimationInfo.Address_Regions)), () =>
                    {
                        Data.PaletteAnimationVRAMRegions = s.SerializeObjectArray<PS1_VRAMRegion>(Data.PaletteAnimationVRAMRegions, Data.PaletteAnimation.OffsetTable.FilesCount, name: nameof(Data.PaletteAnimationVRAMRegions));
                    });

                    break;

                case GlobalGameObjectType.NahatombBluePlatformAndGem: // FUN_23_7__80114d9c
                    CreateModels(2);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    Data.MovementPaths = SerializeDataFile<MovementPath_File>(s, Data.MovementPaths, name: nameof(Data.MovementPaths));
                    Data.Models[1].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[1].TMD, modelIndex: 1, name: nameof(GameObjectData_Model.TMD));
                    break;

                case GlobalGameObjectType.Light:

                    if (Short_00 == 0x11)
                    {
                        Data.LightPositions = SerializeDataFile<VectorAnimation_File>(s, Data.LightPositions, name: nameof(Data.LightPositions));
                    }
                    else
                    {
                        CreateModels(1);
                        Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    }

                    break;

                case GlobalGameObjectType.GeyserPlatform:
                    Data.GeyserPlatformPositionsPointer = loader.Settings.GeyserPlatformPositionsPointers[loader.GlobalSectorIndex];
                    s.DoAt(new Pointer(Data.GeyserPlatformPositionsPointer, loader.FindCodeFile(Data.GeyserPlatformPositionsPointer)), () =>
                    {
                        Data.GeyserPlatformPositions = s.SerializeObjectArrayUntil(Data.GeyserPlatformPositions, x => x.Ushort_06 == 0, () => new GameObjectData_GeyserPlatformPosition()
                        {
                            Position = new KlonoaVector16()
                        }, name: nameof(Data.GeyserPlatformPositions));
                    });

                    int count = Data.GeyserPlatformPositions.Length;

                    CreateModels(count);

                    PS1_TMD tmd = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));

                    for (int i = 0; i < count; i++)
                    {
                        Data.Models[i].TMD = tmd;
                        Data.Models[i].Position = Data.GeyserPlatformPositions[i].Position;
                    }

                    Data.Collision = SerializeDataFile<CollisionTriangles_File>(s, Data.Collision, name: nameof(Data.Collision));
                    break;

                case GlobalGameObjectType.ScrollAnimation:
                    Data.UVScrollAnimation = SerializeDataFile<UVScrollAnimation_File>(s, Data.UVScrollAnimation, name: nameof(Data.UVScrollAnimation));
                    break;

                case GlobalGameObjectType.VRAMScrollAnimation:
                    Data.VRAMScrollInfos = loader.Settings.VRAMScrollInfos[loader.BINBlock];
                    break;

                case GlobalGameObjectType.VRAMScrollAnimationWithTexture:
                    Data.TIM = SerializeDataFile<PS1_TIM>(s, Data.TIM, name: nameof(Data.TIM));
                    Data.VRAMScrollInfos = loader.Settings.VRAMScrollInfos[loader.BINBlock];
                    break;

                case GlobalGameObjectType.RGBAnimation:
                    Data.RGBAnimations = SerializeDataFile<RGBAnimations_File>(s, Data.RGBAnimations, name: nameof(Data.RGBAnimations));
                    break;

                case GlobalGameObjectType.Object:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    break;

                case GlobalGameObjectType.LevelModelSection:
                    CreateModels(1);

                    Data.Models[0].TMD = SerializeDataFile<PS1_TMD>(s, Data.Models[0].TMD, modelIndex: 0, name: nameof(GameObjectData_Model.TMD));
                    Data.Models[0].UnknownModelObjectsData = SerializeDataFile<UnknownModelObjectsData_File>(s, Data.Models[0].UnknownModelObjectsData, onPreSerialize: x => x.Pre_ObjsCount = Data.Models[0].TMD.ObjectsCount, modelIndex: 0, name: nameof(GameObjectData_Model.UnknownModelObjectsData)); // TODO: Parsed at FUN_8002692c
                    break;

                case GlobalGameObjectType.ScenerySprites:
                    Data.ScenerySprites = SerializeDataFile<VectorAnimation_File>(s, Data.ScenerySprites, name: nameof(Data.ScenerySprites));
                    break;

                case GlobalGameObjectType.TextureAnimation:
                    Data.TextureAnimation = SerializeDataFile<TIM_ArchiveFile>(s, Data.TextureAnimation, name: nameof(Data.TextureAnimation));

                    Data.TextureAnimationInfo = loader.Settings.TextureAnimationInfos[loader.BINBlock];
                    break;

                case GlobalGameObjectType.PaletteAnimation:
                case GlobalGameObjectType.PaletteAnimations:
                    if (Data.GlobalGameObjectType == GlobalGameObjectType.PaletteAnimation)
                        Data.PaletteAnimation = SerializeDataFile<PaletteAnimation_ArchiveFile>(s, Data.PaletteAnimation, name: nameof(Data.PaletteAnimation));
                    else
                        Data.PaletteAnimations = SerializeDataFile<PaletteAnimations_ArchiveFile>(s, Data.PaletteAnimations, name: nameof(Data.PaletteAnimations));

                    Data.PaletteAnimationInfo = loader.Settings.PaletteAnimationInfos[loader.BINBlock][SecondaryType];

                    // Ugly hard-coding for one of the types...
                    if (Data.PaletteAnimationInfo.Address_Regions == 0xFFFFFFFF)
                    {
                        Data.PaletteAnimationVRAMRegions = new PS1_VRAMRegion[]
                        {
                            new PS1_VRAMRegion(0xC0, 0x1E2, 0x10, 0x1),
                            new PS1_VRAMRegion(0x70, 0x1E4, 0x10, 0x1),
                            new PS1_VRAMRegion(0x80, 0x1E4, 0x10, 0x1),
                            new PS1_VRAMRegion(0x90, 0x1E4, 0x10, 0x1),
                            new PS1_VRAMRegion(0xA0, 0x1E4, 0x10, 0x1),
                            new PS1_VRAMRegion(0xB0, 0x1E4, 0x10, 0x1),
                            new PS1_VRAMRegion(0xC0, 0x1E4, 0x10, 0x1),
                            new PS1_VRAMRegion(0xD0, 0x1E4, 0x10, 0x1),
                            new PS1_VRAMRegion(0xE0, 0x1E4, 0x10, 0x1),
                        };

                        return;
                    }

                    Pointer p;

                    if (Data.GlobalGameObjectType == GlobalGameObjectType.PaletteAnimation && Data.PaletteAnimationInfo.BlocksCount == -1)
                        p = new Pointer(Data.PaletteAnimationInfo.Address_Regions + (Short_02 * 8), loader.FindCodeFile(Data.PaletteAnimationInfo.Address_Regions));
                    else
                        p = new Pointer(Data.PaletteAnimationInfo.Address_Regions, loader.FindCodeFile(Data.PaletteAnimationInfo.Address_Regions));

                    s.DoAt(p, () =>
                    {
                        int count;
                        
                        if (Data.GlobalGameObjectType == GlobalGameObjectType.PaletteAnimation && Data.PaletteAnimationInfo.BlocksCount != -1)
                            count = Data.PaletteAnimationInfo.BlocksCount;
                        else if (Data.GlobalGameObjectType == GlobalGameObjectType.PaletteAnimation)
                            count = 1;
                        else
                            count = Data.PaletteAnimations.OffsetTable.FilesCount;

                        Data.PaletteAnimationVRAMRegions = s.SerializeObjectArray<PS1_VRAMRegion>(Data.PaletteAnimationVRAMRegions, count, name: nameof(Data.PaletteAnimationVRAMRegions));
                    });
                    break;

                case GlobalGameObjectType.Textures:
                    Data.TIMArchive = SerializeDataFile<TIM_ArchiveFile>(s, Data.TIMArchive, name: nameof(Data.TIMArchive));
                    break;

                case GlobalGameObjectType.Special:
                    break;

                case GlobalGameObjectType.WeatherEffect:
                    break;

                case GlobalGameObjectType.LevelTimer:
                    break;

                case GlobalGameObjectType.Fireworks: // FUN_24_7__80119734
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (DataFileIndices != null && DataFileIndices[_dataFileIndex] != 0)
                s.LogWarning($"Game object of type {Data.GlobalGameObjectType} has unparsed data file(s)");
        }

        private void CreateModels(int count) => Data.Models ??= Enumerable.Range(0, count).Select(x => new GameObjectData_Model()).ToArray();
        private int _dataFileIndex;
        private T SerializeDataFile<T>(SerializerObject s, T obj, Action<T> onPreSerialize = null, bool logIfNotFullyParsed = true, int? modelIndex = null, string name = null)
            where T : BinarySerializable, new()
        {
            return Pre_ObjectAssets.SerializeFile<T>(
                s: s, 
                obj: obj, 
                index: DataFileIndices[_dataFileIndex++], 
                onPreSerialize: onPreSerialize, 
                logIfNotFullyParsed: logIfNotFullyParsed, 
                name: modelIndex != null ? $"{nameof(Data)}.{nameof(Data.Models)}[{modelIndex}].{name}" : $"{nameof(Data)}.{name}");
        }
        private void SkipDataFile<T>(SerializerObject s, bool isUnused = false)
            where T : BinarySerializable, new()
        {
            if (!isUnused)
                s.LogWarning($"Data file skipped at index {_dataFileIndex} for object of type {Data.GlobalGameObjectType}");
            
            SerializeDataFile<T>(s, null, name: isUnused ? "Unused" : "Unknown");
        }
    }
}