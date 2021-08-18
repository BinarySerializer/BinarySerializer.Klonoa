using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    // The game defines 24 secondary types by using the type as an index in a function table. This table is located in the code files
    // for each level block and thus will differ. For Vision 1-1 NTSC the function pointer table is at 0x80110808. A lot of the pointers
    // are nulled out, so you would believe the actual indices themselves will be globally the same, with each level only implementing
    // functions for the used ones, but oddly enough the indices differ between levels.

    public class ModifierObjectDynamicData_File : BaseFile
    {
        public uint Pre_ModelObjsCount { get; set; }
        public GlobalModifierFileType Pre_FileType { get; set; }

        public PS1_TMD TMD { get; set; }
        public ObjTransform_ArchiveFile Transform { get; set; }
        public ArchiveFile<ObjTransform_ArchiveFile> Transforms { get; set; }
        public ObjPosition Position { get; set; }
        public ObjCollisionItems_File Collision { get; set; }
        public PS1_TIM TIM { get; set; }
        public TIM_ArchiveFile TextureAnimation { get; set; }
        public ScenerySprites_File ScenerySprites { get; set; }
        public UVScrollAnimation_File UVScrollAnimation { get; set; }
        public UnknownModelObjectsData_File UnknownModelObjectsData { get; set; }
        public byte[] Raw { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            void onPreSerialize(BaseFile f)
            {
                f.Pre_FileSize = Pre_FileSize;
                f.Pre_IsCompressed = Pre_IsCompressed;
            }

            switch (Pre_FileType)
            {
                case GlobalModifierFileType.TMD:
                    TMD = s.SerializeObject<PS1_TMD>(TMD, name: nameof(TMD));
                    s.Goto(Offset + Pre_FileSize);
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

                case GlobalModifierFileType.Collision:
                    Collision = s.SerializeObject<ObjCollisionItems_File>(Collision, onPreSerialize: onPreSerialize, name: nameof(Collision));
                    break;

                case GlobalModifierFileType.TIM:
                    TIM = s.SerializeObject<PS1_TIM>(TIM, name: nameof(TIM));
                    break;

                case GlobalModifierFileType.TextureAnimation:
                    TextureAnimation = s.SerializeObject<TIM_ArchiveFile>(TextureAnimation, onPreSerialize: onPreSerialize, name: nameof(TextureAnimation));
                    break;

                case GlobalModifierFileType.ScenerySprites:
                    ScenerySprites = s.SerializeObject<ScenerySprites_File>(ScenerySprites, onPreSerialize: onPreSerialize, name: nameof(ScenerySprites));
                    break;

                case GlobalModifierFileType.UVScrollAnimation:
                    UVScrollAnimation = s.SerializeObject<UVScrollAnimation_File>(UVScrollAnimation, onPreSerialize: onPreSerialize, name: nameof(UVScrollAnimation));
                    break;

                case GlobalModifierFileType.UnknownModelObjectsData:
                    UnknownModelObjectsData = s.SerializeObject<UnknownModelObjectsData_File>(UnknownModelObjectsData, onPreSerialize: x =>
                    {
                        onPreSerialize(x);
                        x.Pre_ObjsCount = Pre_ModelObjsCount;
                    }, name: nameof(UnknownModelObjectsData));
                    break;

                case GlobalModifierFileType.UnknownArchive:
                    s.SerializeObject<RawData_ArchiveFile>(default, onPreSerialize: onPreSerialize);
                    break;

                case GlobalModifierFileType.UnknownArchiveArchive:
                    s.SerializeObject<ArchiveFile<RawData_ArchiveFile>>(default, onPreSerialize: onPreSerialize);
                    break;

                case GlobalModifierFileType.Unknown:
                default:
                    Raw = s.SerializeArray<byte>(Raw, Pre_FileSize, name: nameof(Raw));
                    break;
            }
        }
    }
}