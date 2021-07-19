namespace BinarySerializer.KlonoaDTP
{
    public class Object3DPosition_File : BaseFile
    {
        public short XPos { get; set; }
        public short YPos { get; set; }
        public short ZPos { get; set; }
        public short Short_06 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            XPos = s.Serialize<short>(XPos, name: nameof(XPos));
            YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            ZPos = s.Serialize<short>(ZPos, name: nameof(ZPos));
            Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
        }
    }
}