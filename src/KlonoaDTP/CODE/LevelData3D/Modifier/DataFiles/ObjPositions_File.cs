namespace BinarySerializer.Klonoa.DTP
{
    public class ObjPositions_File : BaseFile
    {
        // One of these files in block 9 is shorter than what the game reads. The game corrects this by setting the values to 0 afterwards.
        public override bool DisableNotFullySerializedWarning => Loader_DTP.GetLoader(Context).BINBlock == 9;

        public ObjTransformInfo_File Pre_Info { get; set; }

        public ushort ObjectsCount { get; set; }
        public ushort FramesCount { get; set; }
        public KlonoaVector16[][] Positions { get; set; }

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

            Positions ??= new KlonoaVector16[FramesCount][];

            for (int i = 0; i < Positions.Length; i++)
                Positions[i] = s.SerializeObjectArray<KlonoaVector16>(Positions[i], ObjectsCount, name: $"{nameof(Positions)}[{i}]");
        }
    }
}