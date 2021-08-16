namespace BinarySerializer.Klonoa.DTP
{
    public class ScenerySprites_File : BaseFile
    {
        public short PositionsCount { get; set; }
        public short Short_02 { get; set; }
        public ObjPosition[] Positions { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            PositionsCount = s.Serialize<short>(PositionsCount, name: nameof(PositionsCount));
            Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
            Positions = s.SerializeObjectArray<ObjPosition>(Positions, PositionsCount, name: nameof(Positions));
        }
    }
}