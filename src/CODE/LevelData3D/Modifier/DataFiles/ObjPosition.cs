namespace BinarySerializer.KlonoaDTP
{
    public class ObjPosition : BinarySerializable
    {
        public short XPos { get; set; }
        public short YPos { get; set; }
        public short ZPos { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            XPos = s.Serialize<short>(XPos, name: nameof(XPos));
            YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            ZPos = s.Serialize<short>(ZPos, name: nameof(ZPos));
        }
    }
}