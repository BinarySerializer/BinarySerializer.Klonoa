using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class GameObjectData_Model
    {
        public PS1_TMD TMD { get; set; }
        public UnknownModelObjectsData_File UnknownModelObjectsData { get; set; }
        public ArchiveFile<ModelBoneAnimation_ArchiveFile> ModelAnimations { get; set; }
        public GameObjectData_ModelVertexAnimation VertexAnimation { get; set; }

        public ModelAnimation_ArchiveFile LocalTransform { get; set; }
        public ModelAnimation_ArchiveFile LocalTransform_Secondary { get; set; } // TODO: Get rid of this
        public ArchiveFile<ModelAnimation_ArchiveFile> LocalTransforms { get; set; }
        public KlonoaVector16 Position { get; set; }
        public KlonoaVector16 Rotation { get; set; }

        public GameObjectData_ConstantRotation ConstantRotation { get; set; }

        public float AnimatedLocalTransformSpeed { get; set; } = 1;
        public bool DoesAnimatedLocalTransformPingPong { get; set; }
    }
}