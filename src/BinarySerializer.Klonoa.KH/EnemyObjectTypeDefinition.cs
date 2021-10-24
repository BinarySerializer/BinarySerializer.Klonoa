namespace BinarySerializer.Klonoa.KH
{
    public class EnemyObjectTypeDefinition : BinarySerializable
    {
        public Pointer Function1 { get; set; }
        public Pointer Function2 { get; set; }
        public byte AnimFileIndex { get; set; }
        public byte AnimIndex { get; set; }
        public byte AnimGroupIndex { get; set; }
        public byte Byte_06 { get; set; } // Index to array at 0x08068480
        public byte Flags { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Function1 = s.SerializePointer(Function1, name: nameof(Function1));
            Function2 = s.SerializePointer(Function2, name: nameof(Function2));
            AnimFileIndex = s.Serialize<byte>(AnimFileIndex, name: nameof(AnimFileIndex));
            s.SerializeBitValues<byte>(bitFunc =>
            {
                AnimIndex = (byte)bitFunc(AnimIndex, 3, name: nameof(AnimIndex));
                AnimGroupIndex = (byte)bitFunc(AnimGroupIndex, 5, name: nameof(AnimGroupIndex));
            });
            Byte_06 = s.Serialize<byte>(Byte_06, name: nameof(Byte_06));
            Flags = s.Serialize<byte>(Flags, name: nameof(Flags));
        }
    }
}