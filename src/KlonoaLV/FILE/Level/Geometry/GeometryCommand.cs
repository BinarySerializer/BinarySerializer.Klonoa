using System.Diagnostics;
using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class GeometryCommand : BinarySerializable
    {
        public VIFcode VIFCode { get; set; }

        public KlonoaLV_FloatVector SectionPosition { get; set; }
        public GeometryTriangleStrip[] TriangleStrips { get; set; }
        public KlonoaLV_Vector16[] Vertices { get; set; }
        public KlonoaLV_UV16[] UVs { get; set; }
        public RGB888Color[] VertexColors { get; set; }
        public GSReg_TEX0_1 TEX0 { get; set; }
        public GIFtag GIFTag { get; set; }
        public GSReg_CLAMP_1 CLAMP { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            VIFCode = s.SerializeObject<VIFcode>(VIFCode, name: nameof(VIFCode));
            switch (VIFCode.CMD)
            {
                case 0x11:
                    break;
                case 0x50:
                    GIFTag = s.SerializeObject<GIFtag>(GIFTag, name: nameof(GIFTag));
                    CLAMP = s.SerializeObject<GSReg_CLAMP_1>(CLAMP, onPreSerialize: x => x.SerializeTag = true, name: nameof(CLAMP));
                    break;
                case 0x14:
                    break;
                default:
                    VIFcode_Unpack unpack = new VIFcode_Unpack(VIFCode);
                    switch (unpack.ADDR)
                    {
                        case 923:
                            SectionPosition = s.SerializeObject(SectionPosition, name: nameof(SectionPosition));
                            break;
                        case 0:
                            TriangleStrips = s.SerializeObjectArray<GeometryTriangleStrip>(TriangleStrips, unpack.SIZE - 1, name: nameof(TriangleStrips));
                            s.SerializePadding(0x10, logIfNotNull: true);
                            break;
                        case 12:
                            Vertices = s.SerializeObjectArray<KlonoaLV_Vector16>(Vertices, unpack.SIZE, name: nameof(Vertices));
                            s.Align(4);
                            break;
                        case 82:
                            UVs = s.SerializeObjectArray<KlonoaLV_UV16>(UVs, unpack.SIZE, name: nameof(UVs));
                            break;
                        case 152:
                            VertexColors = s.SerializeObjectArray<RGB888Color>(VertexColors, unpack.SIZE, name: nameof(VertexColors));
                            s.Align(4);
                            break;
                        case 920:
                            TEX0 = s.SerializeObject<GSReg_TEX0_1>(TEX0, name: nameof(TEX0));
                            s.SerializePadding(8);
                            break;
                        default:
                            throw new BinarySerializableException(this, $"Unknown command for address {unpack.ADDR}");
                    }
                    break;
            }
        }
    }
}