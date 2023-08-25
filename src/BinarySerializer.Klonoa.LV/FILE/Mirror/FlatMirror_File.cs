namespace BinarySerializer.Klonoa.LV
{
    public class FlatMirror_File : BaseFile
    {
        public int Count { get; set; }
        public int Type { get; set; }
        public KlonoaLV_Vector2<float>[] Scalars { get; set; }
        public FlatMirror[] Mirrors { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("F", 4);
            Count = s.Serialize<int>(Count, name: nameof(Count));
            Type = s.Serialize<int>(Type, name: nameof(Type));
            s.SerializePadding(4);
            if (Type == 1) {
                Scalars = s.SerializeObjectArray<KlonoaLV_Vector2<float>>(Scalars, Count, name: nameof(Scalars));
            }
            Mirrors = s.SerializeObjectArray<FlatMirror>(Mirrors, Count, onPreSerialize: x => x.Pre_Anchor = Offset, name: nameof(Mirrors));
        }
        
    }
}