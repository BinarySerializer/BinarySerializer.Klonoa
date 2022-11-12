namespace BinarySerializer.Klonoa.LV
{
    public class Route : BinarySerializable
    {
        public KlonoaLV_Vector16 EndPosition { get; set; } // Offset from StartPosition
        public KlonoaLV_Vector32 StartPosition { get; set; }
        public short Flag { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            EndPosition = s.SerializeObject<KlonoaLV_Vector16>(EndPosition, name: nameof(EndPosition), onPreSerialize: x => x.Pre_Padding = true);
            StartPosition = s.SerializeObject<KlonoaLV_Vector32>(StartPosition, name: nameof(StartPosition), onPreSerialize: x => x.Pre_Padding = true);
            Flag = s.Serialize<short>(Flag, name: nameof(Flag));
            s.SerializePadding(2, logIfNotNull: true);
        }
    }
}