namespace BinarySerializer.Klonoa.DTP
{
    public class STM_Entry : BinarySerializable
    {
        public Pointer Pre_Anchor { get; set; }

        public Pointer FileOffset { get; set; }
        public int FileLength { get; set; }

        public bool IsDummy => FileLength < 0;

        public override void SerializeImpl(SerializerObject s)
        {
            FileOffset = s.SerializePointer(FileOffset, anchor: Pre_Anchor, name: nameof(FileOffset));
            FileLength = s.Serialize<int>(FileLength, name: nameof(FileLength));
        }
    }
}