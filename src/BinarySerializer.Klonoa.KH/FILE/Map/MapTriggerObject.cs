namespace BinarySerializer.Klonoa.KH
{
    public class MapTriggerObject : BinarySerializable
    {
        public short ObjType { get; set; }
        public short XPos { get; set; }
        public short ZPos { get; set; }
        public short YPos { get; set; }
        public byte[] Bytes_08 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjType = s.Serialize<short>(ObjType, name: nameof(ObjType));
            XPos = s.Serialize<short>(XPos, name: nameof(XPos));
            ZPos = s.Serialize<short>(ZPos, name: nameof(ZPos));
            YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            Bytes_08 = s.SerializeArray<byte>(Bytes_08, 8, name: nameof(Bytes_08));
        }
    }
}