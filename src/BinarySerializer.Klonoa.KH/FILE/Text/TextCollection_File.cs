namespace BinarySerializer.Klonoa.KH
{
    public class TextCollection_File : BaseFile
    {
        public short Count { get; set; }
        public short TextOffset { get; set; }
        public short Short_08 { get; set; }
        public short Short_0A { get; set; }
        public short Short_0C { get; set; }
        public short TextSize { get; set; }

        public TextCommands[] Text { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("HM", 4);
            Count = s.Serialize<short>(Count, name: nameof(Count));
            TextOffset = s.Serialize<short>(TextOffset, name: nameof(TextOffset));
            Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
            Short_0A = s.Serialize<short>(Short_0A, name: nameof(Short_0A));
            Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
            TextSize = s.Serialize<short>(TextSize, name: nameof(TextSize));
            s.SerializePadding(16);
            s.DoAt(Offset + TextOffset, () => Text = s.SerializeObjectArray<TextCommands>(Text, Count, x => x.Pre_MaxLength = TextSize / 2, name: nameof(Text)));
            s.Goto(Offset + Pre_FileSize);
        }
    }
}