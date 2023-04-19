using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class SFXPart : BinarySerializable
    {
        public Pointer Pre_GeometryPointer { get; set; }

        public ushort RenderFlag { get; set; } // Whether this mesh should be rendered or not (this might have more than two values)
        public ushort Ushort_02 { get; set; } // The game doesn't seem to read this value
        public ushort TriangleStripCount { get; set; }
        public ushort TriangleStripSectionCount { get; set; }
        public ushort UVCount { get; set; }
        public ushort SubpartCount { get; set; } // A mesh has more than 1 subpart if the mesh has more than 4 joint influences
        public ushort VertexCount { get; set; }
        public ushort NormalCount { get; set; }
        public Pointer IndicesPointer { get; set; }
        public Pointer UVsPointer { get; set; }
        public Pointer VerticesPointer { get; set; }
        public Pointer NormalsPointer { get; set; }
        public Pointer SkinPointer { get; set; }
        public GSReg_TEX0_1 TEX0 { get; set; }
        public SFXSubpart[] Subparts { get; set; }
        public KlonoaLV_UV32[] UVs { get; set; }
        public SFXTriangleStrip[] TriangleStrips { get; set; }
        public KlonoaLV_Vector16[] Vertices { get; set; }
        public KlonoaLV_Vector16[] Normals { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            RenderFlag = s.Serialize<ushort>(RenderFlag, name: nameof(RenderFlag));
            Ushort_02 = s.Serialize<ushort>(Ushort_02, name: nameof(Ushort_02));

            TriangleStripCount = s.Serialize<ushort>(TriangleStripCount, name: nameof(TriangleStripCount));
            TriangleStripSectionCount = s.Serialize<ushort>(TriangleStripSectionCount, name: nameof(TriangleStripSectionCount));
            UVCount = s.Serialize<ushort>(UVCount, name: nameof(UVCount));
            SubpartCount = s.Serialize<ushort>(SubpartCount, name: nameof(SubpartCount));
            VertexCount = s.Serialize<ushort>(VertexCount, name: nameof(VertexCount));
            NormalCount = s.Serialize<ushort>(NormalCount, name: nameof(NormalCount));

            IndicesPointer = s.SerializePointer(IndicesPointer, anchor: Pre_GeometryPointer, name: nameof(IndicesPointer));
            UVsPointer = s.SerializePointer(UVsPointer, anchor: Pre_GeometryPointer, name: nameof(UVsPointer));
            VerticesPointer = s.SerializePointer(VerticesPointer, anchor: Pre_GeometryPointer, name: nameof(VerticesPointer));
            NormalsPointer = s.SerializePointer(NormalsPointer, anchor: Pre_GeometryPointer, name: nameof(NormalsPointer));
            SkinPointer = s.SerializePointer(SkinPointer, anchor: Pre_GeometryPointer, name: nameof(SkinPointer));
            
            s.SerializePadding(4, logIfNotNull: true);
            TEX0 = s.SerializeObject<GSReg_TEX0_1>(TEX0, name: nameof(TEX0));

            if (SubpartCount > 0)
            {
                s.DoAt(SkinPointer, () => Subparts = s.SerializeObjectArray<SFXSubpart>(Subparts, SubpartCount, name: nameof(Subparts), onPreSerialize: x =>
                {
                    x.Pre_GeometryPointer = Pre_GeometryPointer;
                    x.Pre_VerticesPointer = VerticesPointer;
                    x.Pre_NormalsPointer = NormalsPointer;
                }));
            } else
            {
                s.DoAt(VerticesPointer, () => Vertices = s.SerializeObjectArray<KlonoaLV_Vector16>(Vertices, VertexCount, name: nameof(Vertices)));
                s.DoAt(NormalsPointer, () => Normals = s.SerializeObjectArray<KlonoaLV_Vector16>(Normals, NormalCount, name: nameof(Normals)));
            }
            
            s.DoAt(UVsPointer, () => UVs = s.SerializeObjectArray<KlonoaLV_UV32>(UVs, UVCount, name: nameof(UVs)));
            s.DoAt(IndicesPointer, () => TriangleStrips = s.SerializeObjectArray<SFXTriangleStrip>(TriangleStrips, TriangleStripCount, name: nameof(TriangleStrips)));
        }
    }
}