using System.Linq;

namespace BinarySerializer.Klonoa.DTP
{
    public class GameObjectData_ModelBoneAnimations
    {
        public VectorAnimation_File InitialBonePositions { get; set; }
        public VectorAnimationKeyFrames_File InitialBoneRotations { get; set; }
        public GameObjectData_ModelBoneAnimation[] Animations { get; set; }

        public static GameObjectData_ModelBoneAnimations FromCommonBossModelBoneAnimation(CommonBossModelBoneAnimation_ArchiveFile commonModelAnimations)
        {
            return new GameObjectData_ModelBoneAnimations()
            {
                InitialBonePositions = commonModelAnimations.InitialPositions,
                InitialBoneRotations = commonModelAnimations.InitialRotations,
                Animations = Enumerable.Range(0, commonModelAnimations.Rotations.Length).Select(x => new GameObjectData_ModelBoneAnimation
                {
                    BoneRotations = commonModelAnimations.Rotations[x],
                    ModelPositions = commonModelAnimations.ModelPositions[x],
                }).ToArray()
            };
        }
    }
}