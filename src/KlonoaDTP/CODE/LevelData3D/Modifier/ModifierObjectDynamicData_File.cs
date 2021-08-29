using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    // The game defines 24 secondary types by using the type as an index in a function table. This table is located in the code files
    // for each level block and thus will differ. For Vision 1-1 NTSC the function pointer table is at 0x80110808. A lot of the pointers
    // are nulled out, so you would believe the actual indices themselves will be globally the same, with each level only implementing
    // functions for the used ones, but oddly enough the indices differ between levels.

    public class ModifierObjectDynamicData_File : BaseFile
    {
        public ModifierObject Pre_ModifierObject { get; set; }
        public GlobalModifierFileType Pre_FileType { get; set; }

        public byte[] Unknown { get; set; }
        public RawData_ArchiveFile UnknownArchive { get; set; }
        public ArchiveFile<RawData_ArchiveFile> UnknownArchiveArchive { get; set; }

        public PS1_TMD TMD { get; set; }
        public ObjCollisionItems_File Collision { get; set; }
        public MovementPath_File MovementPaths { get; set; }
        public UnknownModelObjectsData_File UnknownModelObjectsData { get; set; }
        public PS1_TIM TIM { get; set; }
        public LightObject LightObject { get; set; }
        
        public ObjTransform_ArchiveFile Transform { get; set; }
        public ArchiveFile<ObjTransform_ArchiveFile> Transforms { get; set; }
        public ObjPosition Position { get; set; }
        
        public TIM_ArchiveFile TextureAnimation { get; set; }
        public PaletteAnimation_ArchiveFile PaletteAnimation { get; set; }
        public UVScrollAnimation_File UVScrollAnimation { get; set; }
        
        public ScenerySprites_File ScenerySprites { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            void onPreSerialize(BaseFile f)
            {
                f.Pre_FileSize = Pre_FileSize;
                f.Pre_IsCompressed = Pre_IsCompressed;
            }

            switch (Pre_FileType)
            {
                case GlobalModifierFileType.None:
                    throw new BinarySerializableException(this, "Attempted to serialize non-existing modifier data");

                case GlobalModifierFileType.Unknown:
                default:
                    Unknown = s.SerializeArray<byte>(Unknown, Pre_FileSize, name: nameof(Unknown));
                    break;

                case GlobalModifierFileType.UnknownArchive:
                    UnknownArchive = s.SerializeObject<RawData_ArchiveFile>(UnknownArchive, onPreSerialize: onPreSerialize, name: nameof(UnknownArchive));
                    break;

                case GlobalModifierFileType.UnknownArchiveArchive:
                    UnknownArchiveArchive = s.SerializeObject<ArchiveFile<RawData_ArchiveFile>>(UnknownArchiveArchive, onPreSerialize: onPreSerialize, name: nameof(UnknownArchiveArchive));
                    break;

                case GlobalModifierFileType.TMD:
                    TMD = s.SerializeObject<PS1_TMD>(TMD, name: nameof(TMD));
                    s.Goto(Offset + Pre_FileSize);
                    break;

                case GlobalModifierFileType.Collision:
                    Collision = s.SerializeObject<ObjCollisionItems_File>(Collision, onPreSerialize: onPreSerialize, name: nameof(Collision));
                    break;

                case GlobalModifierFileType.MovementPaths:
                    MovementPaths = s.SerializeObject<MovementPath_File>(MovementPaths, onPreSerialize: onPreSerialize, name: nameof(MovementPaths));
                    break;

                case GlobalModifierFileType.UnknownModelObjectsData:
                    UnknownModelObjectsData = s.SerializeObject<UnknownModelObjectsData_File>(UnknownModelObjectsData, onPreSerialize: x =>
                    {
                        onPreSerialize(x);
                        x.Pre_ObjsCount = Pre_ModifierObject.DataFiles[0].TMD.ObjectsCount;
                    }, name: nameof(UnknownModelObjectsData));
                    break;

                case GlobalModifierFileType.TIM:
                    TIM = s.SerializeObject<PS1_TIM>(TIM, name: nameof(TIM));
                    break;

                case GlobalModifierFileType.LightObject:
                    LightObject = s.SerializeObject<LightObject>(LightObject, onPreSerialize: x =>
                    {
                        onPreSerialize(x);
                        x.Pre_ModifierObj = Pre_ModifierObject;
                    }, name: nameof(LightObject));
                    break;

                case GlobalModifierFileType.Transform_WithInfo:
                    Transform = s.SerializeObject<ObjTransform_ArchiveFile>(Transform, onPreSerialize: x =>
                    {
                        onPreSerialize(x);
                        x.Pre_UsesTransformInfo = true;
                    }, name: nameof(Transform));
                    break;

                case GlobalModifierFileType.Transform_WithoutInfo:
                    Transform = s.SerializeObject<ObjTransform_ArchiveFile>(Transform, onPreSerialize: x =>
                    {
                        onPreSerialize(x);
                        x.Pre_UsesTransformInfo = false;
                    }, name: nameof(Transform));
                    break;

                case GlobalModifierFileType.Transforms_WithInfo:
                    Transforms = s.SerializeObject<ArchiveFile<ObjTransform_ArchiveFile>>(Transforms, onPreSerialize: x =>
                    {
                        onPreSerialize(x);
                        x.Pre_OnPreSerializeAction = obj => obj.Pre_UsesTransformInfo = true;
                    }, name: nameof(Transforms));
                    break;

                case GlobalModifierFileType.Position:
                    Position = s.SerializeObject<ObjPosition>(Position, name: nameof(Position));
                    break;

                case GlobalModifierFileType.TextureAnimation:
                    TextureAnimation = s.SerializeObject<TIM_ArchiveFile>(TextureAnimation, onPreSerialize: onPreSerialize, name: nameof(TextureAnimation));
                    break;

                case GlobalModifierFileType.PaletteAnimation:
                    PaletteAnimation = s.SerializeObject<PaletteAnimation_ArchiveFile>(PaletteAnimation, onPreSerialize: onPreSerialize, name: nameof(PaletteAnimation));
                    break;

                case GlobalModifierFileType.UVScrollAnimation:
                    UVScrollAnimation = s.SerializeObject<UVScrollAnimation_File>(UVScrollAnimation, onPreSerialize: onPreSerialize, name: nameof(UVScrollAnimation));
                    break;

                case GlobalModifierFileType.ScenerySprites:
                    ScenerySprites = s.SerializeObject<ScenerySprites_File>(ScenerySprites, onPreSerialize: onPreSerialize, name: nameof(ScenerySprites));
                    break;

            }
        }
    }
}