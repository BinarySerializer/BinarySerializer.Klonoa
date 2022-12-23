using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class VIFGeometry_Block : BinarySerializable
    {
        public GIFtag[] GIFtags { get; set; } // 0x0000
        public VIFGeometry_Vertex[] Vertices { get; set; } // 0x00C0
        public VIFGeometry_UV[] UVs { get; set; } // 0x0520
        public VIFGeometry_Color[] VertexColors { get; set; } // 0x0980
        public GSReg_TEX0_1 TEX0 { get; set; } // 0x3980
        public KlonoaLV_FloatVector SectionPosition { get; set; } // 0x39B0

        public override void SerializeImpl(SerializerObject s)
        {
            GIFtags = s.DoAt(Offset + 0x0000, () => s.SerializeObjectArrayUntil(GIFtags, (g) => g.EOP, name: nameof(GIFtags)));

            int vertexCount = 0;
            foreach (GIFtag tag in GIFtags) {
                vertexCount += tag.NLOOP;
            }

            Vertices = s.DoAt(Offset + 0x00C0, () => s.SerializeObjectArray<VIFGeometry_Vertex>(Vertices, vertexCount, name: nameof(Vertices)));
            UVs = s.DoAt(Offset + 0x0520, () => s.SerializeObjectArray<VIFGeometry_UV>(UVs, vertexCount, name: nameof(UVs)));
            VertexColors = s.DoAt(Offset + 0x0980, () => s.SerializeObjectArray<VIFGeometry_Color>(VertexColors, vertexCount, name: nameof(VertexColors)));
            TEX0 = s.DoAt(Offset + 0x3980, () => s.SerializeObject<GSReg_TEX0_1>(TEX0, name: nameof(TEX0)));
            SectionPosition = s.DoAt(Offset + 0x39B0, () => s.SerializeObject<KlonoaLV_FloatVector>(SectionPosition, name: nameof(SectionPosition)));
        }
    }
}