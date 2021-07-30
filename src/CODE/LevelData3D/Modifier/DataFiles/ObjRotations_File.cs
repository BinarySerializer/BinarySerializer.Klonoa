namespace BinarySerializer.KlonoaDTP
{
    public class ObjRotations_File : BaseFile
    {
        public ObjTransformInfo_File Pre_Info { get; set; }

        public ushort ObjectsCount { get; set; }
        public ushort FramesCount { get; set; }
        public ObjRotation[][] Rotations { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            if (Pre_Info == null)
            {
                ObjectsCount = s.Serialize<ushort>(ObjectsCount, name: nameof(ObjectsCount));
                FramesCount = s.Serialize<ushort>(FramesCount, name: nameof(FramesCount));
            }
            else
            {
                ObjectsCount = Pre_Info.ObjectsCount;
                FramesCount = Pre_Info.FramesCount;
            }

            Rotations ??= new ObjRotation[FramesCount][];

            for (int i = 0; i < Rotations.Length; i++)
                Rotations[i] = s.SerializeObjectArray<ObjRotation>(Rotations[i], ObjectsCount, name: $"{nameof(Rotations)}[{i}]");
        }
    }
}