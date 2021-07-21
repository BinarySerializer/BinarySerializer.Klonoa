namespace BinarySerializer.KlonoaDTP
{
    public class ObjUnknown_File : BaseFile
    {
        public short Short_00 { get; set; }
        public short Short_02 { get; set; }
        public short Short_04 { get; set; }
        public short Short_06 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
            Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
            Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
            Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
        }
    }
}