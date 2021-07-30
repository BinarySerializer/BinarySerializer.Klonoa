namespace BinarySerializer.KlonoaDTP
{
    public class ObjPositions_File : BaseFile
    {
        public ObjTransformInfo_File Pre_Info { get; set; }

        public ushort ObjectsCount { get; set; }
        public ushort FramesCount { get; set; }
        public ObjPosition[][] Positions { get; set; }

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

            Positions ??= new ObjPosition[FramesCount][];

            for (int i = 0; i < Positions.Length; i++)
                Positions[i] = s.SerializeObjectArray<ObjPosition>(Positions[i], ObjectsCount, name: $"{nameof(Positions)}[{i}]");
        }
    }
}