namespace BinarySerializer.Klonoa.LV
{
    public class CurveMirror : BinarySerializable
    {
        public Pointer Pre_Anchor { get; set; }

        public Pointer VectorPointer { get; set; }
        public uint Count { get; set; }
        public uint PolyCount { get; set; }
        public Pointer HeaderPointer { get; set; }
        public CurveMirrorHeader[] Headers { get; set; }
        public KlonoaLV_FloatVector[] Normal { get; set; }
        public KlonoaLV_FloatVector[] Center { get; set; }
        public KlonoaLV_FloatVector[] Vertices { get; set; }
        public KlonoaLV_Vector16[] Dest { get; set; } // This should probably be a 2D array

        public override void SerializeImpl(SerializerObject s)
        {
            VectorPointer = s.SerializePointer(VectorPointer, anchor: Pre_Anchor, name: nameof(VectorPointer));
            Count = s.Serialize<uint>(Count, name: nameof(Count));
            PolyCount = s.Serialize<uint>(PolyCount, name: nameof(PolyCount));
            HeaderPointer = s.SerializePointer(HeaderPointer, anchor: Pre_Anchor, name: nameof(HeaderPointer));

            s.DoAt(HeaderPointer, () => {
                Headers = s.SerializeObjectArray<CurveMirrorHeader>(Headers, PolyCount, name: nameof(Headers));
            });

            s.DoAt(VectorPointer, () => {
                Normal = s.SerializeObjectArray<KlonoaLV_FloatVector>(Normal, PolyCount, name: nameof(Normal), onPreSerialize: x => x.Pre_HasW = true);
                Center = s.SerializeObjectArray<KlonoaLV_FloatVector>(Center, PolyCount, name: nameof(Center), onPreSerialize: x => x.Pre_HasW = true);
                Vertices = s.SerializeObjectArray<KlonoaLV_FloatVector>(Vertices, Count, name: nameof(Vertices), onPreSerialize: x => x.Pre_HasW = true);
                Dest = s.SerializeObjectArray<KlonoaLV_Vector16>(Dest, Count * PolyCount, name: nameof(Dest));
            });
        }
    }
}