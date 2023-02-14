using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class GameObjectData_Model
    {
        public TMD TMD { get; set; }
        public bool IsMissingImageData { get; set; } // Set to true if the model is missing VRAM data to render
        public KlonoaVector16[] TMDObjectPositionOffsets { get; set; }
        public UnknownModelObjectsData_File UnknownModelObjectsData { get; set; }
        public ArchiveFile<RongoLangoModelBoneAnimation_ArchiveFile> RongoLangoModelAnimations { get; set; }
        public GameObjectData_ModelVertexAnimation VertexAnimation { get; set; }

        public ModelAnimation_ArchiveFile LocalTransform { get; set; }
        public ArchiveFile<ModelAnimation_ArchiveFile> LocalTransforms { get; set; }
        public KlonoaVector16 Position { get; set; }
        public KlonoaVector16 Rotation { get; set; }

        public GameObjectData_ModelBoneAnimations ModelBoneAnimations { get; set; }
        public GameObjectData_ConstantRotation ConstantRotation { get; set; }

        public float AnimatedLocalTransformSpeed { get; set; } = 1;
        public bool DoesAnimatedLocalTransformPingPong { get; set; }
    }
}