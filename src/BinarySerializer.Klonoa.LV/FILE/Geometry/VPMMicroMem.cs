using BinarySerializer.PlayStation.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class VPMMicroMem : BinarySerializable
    {
        public uint Pre_ProgramAddress { get; set; }

        public GIFtag[] GIFtags { get; set; } // 0x0000
        public VPMMicroMemVertex[] Vertices { get; set; } // 0x00C0
        public VPMMicroMemUV[] UVs { get; set; } // 0x0520 / 0x0480
        public VPMMicroMemColor[] VertexColors { get; set; } // 0x0980 / 0x0840
        public VPMMicroMemVertex[] Normals { get; set; } // 0x0C00 (Type 1 only)
        public GSReg_TEX0_1 TEX0 { get; set; } // 0x3980
        public KlonoaLV_FloatVector BasePosition { get; set; } // 0x39B0

        public override void SerializeImpl(SerializerObject s)
        {
            GIFtags = s.DoAt(Offset + 0x0000, () => s.SerializeObjectArrayUntil(GIFtags, (g) => g.EOP, name: nameof(GIFtags)));

            int vertexCount = 0;
            foreach (GIFtag tag in GIFtags) {
                vertexCount += tag.NLOOP;
            }

            if (Pre_ProgramAddress == 0x12 || Pre_ProgramAddress == 0x04) {
                Vertices = s.DoAt(Offset + 0x00C0, () => s.SerializeObjectArray<VPMMicroMemVertex>(Vertices, vertexCount, name: nameof(Vertices)));
                UVs = s.DoAt(Offset + 0x0520, () => s.SerializeObjectArray<VPMMicroMemUV>(UVs, vertexCount, name: nameof(UVs)));
                VertexColors = s.DoAt(Offset + 0x0980, () => s.SerializeObjectArray<VPMMicroMemColor>(VertexColors, vertexCount, name: nameof(VertexColors)));
                TEX0 = s.DoAt(Offset + 0x3980, () => s.SerializeObject<GSReg_TEX0_1>(TEX0, name: nameof(TEX0)));
                BasePosition = s.DoAt(Offset + 0x39B0, () => s.SerializeObject<KlonoaLV_FloatVector>(BasePosition, name: nameof(BasePosition)));
            } else if (Pre_ProgramAddress == 0x10) {
                Vertices = s.DoAt(Offset + 0x00C0, () => s.SerializeObjectArray<VPMMicroMemVertex>(Vertices, vertexCount, name: nameof(Vertices)));
                UVs = s.DoAt(Offset + 0x0480, () => s.SerializeObjectArray<VPMMicroMemUV>(UVs, vertexCount, name: nameof(UVs)));
                VertexColors = s.DoAt(Offset + 0x0840, () => s.SerializeObjectArray<VPMMicroMemColor>(VertexColors, vertexCount, name: nameof(VertexColors)));
                Normals = s.DoAt(Offset + 0x0C00, () => s.SerializeObjectArray<VPMMicroMemVertex>(Normals, vertexCount, name: nameof(Normals)));
                TEX0 = s.DoAt(Offset + 0x3980, () => s.SerializeObject<GSReg_TEX0_1>(TEX0, name: nameof(TEX0)));
            } else {
                throw new BinarySerializableException(this, $"Unknown microprogram address: {Pre_ProgramAddress}");
            }
        }
    }
}