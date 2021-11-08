namespace BinarySerializer.Klonoa.DTP
{
    public class GameObjectData_ModelBoneAnimation
    {
        public VectorAnimationKeyFrames_File BoneRotations { get; set; }
        public VectorAnimation_File BonePositions { get; set; }
        public VectorAnimation_File ModelPositions { get; set; } // Gelg Bolm animates several models
    }
}