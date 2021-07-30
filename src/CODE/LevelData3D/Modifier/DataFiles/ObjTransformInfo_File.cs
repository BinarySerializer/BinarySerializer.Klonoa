namespace BinarySerializer.KlonoaDTP
{
    public class ObjTransformInfo_File : BaseFile
    {
        public ushort ObjectsCount { get; set; } // The number of objects the transform is for
        public ushort FramesCount { get; set; }
        public short[] Shorts_04 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjectsCount = s.Serialize<ushort>(ObjectsCount, name: nameof(ObjectsCount));
            FramesCount = s.Serialize<ushort>(FramesCount, name: nameof(FramesCount));
            Shorts_04 = s.SerializeArray<short>(Shorts_04, ObjectsCount, name: nameof(Shorts_04));
        }
    }
}