using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class GeometryCommand : BinarySerializable
    {
        public VIFcode VIFCode { get; set; }
        public CommandType Type { get; set; }

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
                case 0x14:
                    Type = CommandType.NOP;
                    break;
                case 0x50:
                    GIFTag = s.SerializeObject<GIFtag>(GIFTag, name: nameof(GIFTag));
                    CLAMP = s.SerializeObject<GSReg_CLAMP_1>(CLAMP, onPreSerialize: x => x.SerializeTag = true, name: nameof(CLAMP));
                    Type = CommandType.TransferData;
                    break;
                default:
                    VIFcode_Unpack unpack = new VIFcode_Unpack(VIFCode);
                    if (unpack.VN == VIFcode_Unpack.UnpackVN.V3 && unpack.VL == VIFcode_Unpack.UnpackVL.VL_32)
                    {
                        if (unpack.ADDR == 923)
                        {
                            SectionPosition = s.SerializeObject(SectionPosition, name: nameof(SectionPosition));
                            Type = CommandType.SectionPosition;
                        } else if (unpack.ADDR == 0)
                        {
                            TEX0 = s.SerializeObject<GSReg_TEX0_1>(TEX0, name: nameof(TEX0));
                            s.SerializePadding(8);
                            Type = CommandType.Tex0;
                        } else
                        {
                            throw new BinarySerializableException(this, $"Unknown command for address {unpack.ADDR}");
                        }
                    } else if (unpack.VN == VIFcode_Unpack.UnpackVN.V4 && unpack.VL == VIFcode_Unpack.UnpackVL.VL_32)
                    {
                        TriangleStrips = s.SerializeObjectArray<GeometryTriangleStrip>(TriangleStrips, unpack.SIZE - 1, name: nameof(TriangleStrips));
                        s.SerializePadding(0x10, logIfNotNull: true);
                        Type = CommandType.TriangleStrips;
                    } else if (unpack.VN == VIFcode_Unpack.UnpackVN.V3 && unpack.VL == VIFcode_Unpack.UnpackVL.VL_16)
                    {
                        Vertices = s.SerializeObjectArray<KlonoaLV_Vector16>(Vertices, unpack.SIZE, name: nameof(Vertices));
                        s.Align(4);
                        Type = CommandType.Vertices;
                    } else if (unpack.VN == VIFcode_Unpack.UnpackVN.V2 && unpack.VL == VIFcode_Unpack.UnpackVL.VL_16)
                    {
                        UVs = s.SerializeObjectArray<KlonoaLV_UV16>(UVs, unpack.SIZE, name: nameof(UVs));
                        Type = CommandType.UVs;
                    } else if (unpack.VN == VIFcode_Unpack.UnpackVN.V3 && unpack.VL == VIFcode_Unpack.UnpackVL.VL_8)
                    {
                        VertexColors = s.SerializeObjectArray<RGB888Color>(VertexColors, unpack.SIZE, name: nameof(VertexColors));
                        s.Align(4);
                        Type = CommandType.VertexColors;
                    } else
                    {
                        throw new BinarySerializableException(this, $"Unknown command for data type {unpack.VN}-{unpack.VL}");
                    }
                    break;
            }
        }

        public enum CommandType
        {
            Vertices,
            SectionPosition,
            TriangleStrips,
            UVs,
            VertexColors,
            Tex0,
            TransferData,
            NOP
        }
    }
}