namespace BinarySerializer.Klonoa.LV
{
    public class FlatMirror : BinarySerializable
    {
        public Pointer Pre_Anchor { get; set; }

        public Pointer VectorPointer { get; set; }
        public uint Count { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float Length { get; set; }
        public KlonoaLV_FloatVector Normal { get; set; }
        public KlonoaLV_FloatVector Center { get; set; }
        public KlonoaLV_FloatVector[] Vertices { get; set; }


        public override void SerializeImpl(SerializerObject s)
        {
            VectorPointer = s.SerializePointer(VectorPointer, anchor: Pre_Anchor, name: nameof(VectorPointer));
            Count = s.Serialize<uint>(Count, name: nameof(Count));
            RotationX = s.Serialize<float>(RotationX, name: nameof(RotationX));
            RotationY = s.Serialize<float>(RotationY, name: nameof(RotationY));
            Length = s.Serialize<float>(Length, name: nameof(Length));

            s.DoAt(VectorPointer, () => {
                Normal = s.SerializeObject<KlonoaLV_FloatVector>(Normal, name: nameof(Normal), onPreSerialize: x => x.Pre_HasW = true);
                Center = s.SerializeObject<KlonoaLV_FloatVector>(Center, name: nameof(Center), onPreSerialize: x => x.Pre_HasW = true);
                Vertices = s.SerializeObjectArray<KlonoaLV_FloatVector>(Vertices, Count, name: nameof(Vertices), onPreSerialize: x => x.Pre_HasW = true);
            });
        }
    }
}