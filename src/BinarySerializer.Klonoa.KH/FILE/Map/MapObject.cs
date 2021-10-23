namespace BinarySerializer.Klonoa.KH
{
    public class MapObject : BinarySerializable
    {
        public short ObjType { get; set; }
        public byte Byte_02 { get; set; }
        public byte ZPos { get; set; }
        public short XPos { get; set; }
        public short YPos { get; set; }
        public byte[] Bytes_08 { get; set; } // TODO: Parse remaining values
        public short Short_1E { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjType = s.Serialize<short>(ObjType, name: nameof(ObjType));
            Byte_02 = s.Serialize<byte>(Byte_02, name: nameof(Byte_02));
            ZPos = s.Serialize<byte>(ZPos, name: nameof(ZPos));
            XPos = s.Serialize<short>(XPos, name: nameof(XPos));
            YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            Bytes_08 = s.SerializeArray<byte>(Bytes_08, 22, name: nameof(Bytes_08));
            Short_1E = s.Serialize<short>(Short_1E, name: nameof(Short_1E));
        }
    }
}