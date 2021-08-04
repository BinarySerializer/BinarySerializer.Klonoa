namespace BinarySerializer.KlonoaDTP
{
    public class BackgroundDescriptor : BinarySerializable
    {
        public short XPos { get; set; }
        public short YPos { get; set; }
        public short Type { get; set; }

        public int BGDIndex { get; set; }
        public int UnknownValues { get; set; }
        public int CELIndex { get; set; }
        public bool UnknownFlag1 { get; set; }
        public bool UnknownFlag2 { get; set; }

        public byte[] Data { get; set; } // This data is structured differently depending on the type

        public override void SerializeImpl(SerializerObject s)
        {
            XPos = s.Serialize<short>(XPos, name: nameof(XPos));
            YPos = s.Serialize<short>(YPos, name: nameof(YPos));
            Type = s.Serialize<short>(Type, name: nameof(Type));

            s.SerializeBitValues<ushort>(bitFunc =>
            {
                BGDIndex = bitFunc(BGDIndex, 5, name: nameof(BGDIndex));
                UnknownValues = bitFunc(UnknownValues, 7, name: nameof(UnknownValues));
                CELIndex = bitFunc(CELIndex, 2, name: nameof(CELIndex));
                UnknownFlag1 = bitFunc(UnknownFlag1 ? 1 : 0, 1, name: nameof(UnknownFlag1)) == 1;
                UnknownFlag2 = bitFunc(UnknownFlag2 ? 1 : 0, 1, name: nameof(UnknownFlag2)) == 1;
            });

            Data = s.SerializeArray<byte>(Data, 56, name: nameof(Data));
        }
    }
}