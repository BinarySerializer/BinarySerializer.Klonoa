using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class ModelMesh : BinarySerializable
    {
        public Pointer Pre_GeometryPointer { get; set; }

        public ushort Visible { get; set; } // Whether this mesh should be rendered or not
        public ushort Ushort_02 { get; set; } // The game doesn't appear to read this value
        public ushort TriangleStripCount { get; set; }
        
        // The end of a triangle strip section is marked when a TriangleStripIndex's Extra value is -1.
        // Parsing triangle strips using the number of tstrip sections is harder to implement, so just use TriangleStripCount instead.
        public ushort TriangleStripSectionCount { get; set; }
        public ushort UVCount { get; set; }
        public ushort SkinDescriptorCount { get; set; } // A mesh has more than 1 skin descriptor if the mesh has more than 4 joint influences
        public ushort VertexCount { get; set; }
        public ushort NormalCount { get; set; }
        public Pointer IndicesPointer { get; set; }
        public Pointer UVsPointer { get; set; }
        public Pointer VerticesPointer { get; set; }
        public Pointer NormalsPointer { get; set; }
        public Pointer SkinPointer { get; set; }
        public GSReg_TEX0 TEX0 { get; set; }
        public ModelSkinDescriptor[] SkinDescriptors { get; set; }
        public KlonoaLV_UV32[] UVs { get; set; }
        public ModelTriangleStrip[] TriangleStrips { get; set; }

        // If the mesh is not skinned, vertices & normals will be defined here rather than in the skin descriptor(s).
        // However, this only seems to occur when Visible is false, meaning the mesh should not even be rendered in the first place.
        // This part is purely for redundancy since the vertices are still technically defined for the mesh (for whatever reason...).
        public bool HasSkin => SkinDescriptorCount > 0;
        public KlonoaLV_Vector16[] Vertices { get; set; }
        public KlonoaLV_Vector16[] Normals { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Visible = s.Serialize<ushort>(Visible, name: nameof(Visible));
            Ushort_02 = s.Serialize<ushort>(Ushort_02, name: nameof(Ushort_02));

            TriangleStripCount = s.Serialize<ushort>(TriangleStripCount, name: nameof(TriangleStripCount));
            TriangleStripSectionCount = s.Serialize<ushort>(TriangleStripSectionCount, name: nameof(TriangleStripSectionCount));
            UVCount = s.Serialize<ushort>(UVCount, name: nameof(UVCount));
            SkinDescriptorCount = s.Serialize<ushort>(SkinDescriptorCount, name: nameof(SkinDescriptorCount));
            VertexCount = s.Serialize<ushort>(VertexCount, name: nameof(VertexCount));
            NormalCount = s.Serialize<ushort>(NormalCount, name: nameof(NormalCount));

            IndicesPointer = s.SerializePointer(IndicesPointer, anchor: Pre_GeometryPointer, name: nameof(IndicesPointer));
            UVsPointer = s.SerializePointer(UVsPointer, anchor: Pre_GeometryPointer, name: nameof(UVsPointer));
            VerticesPointer = s.SerializePointer(VerticesPointer, anchor: Pre_GeometryPointer, name: nameof(VerticesPointer));
            NormalsPointer = s.SerializePointer(NormalsPointer, anchor: Pre_GeometryPointer, name: nameof(NormalsPointer));
            SkinPointer = s.SerializePointer(SkinPointer, anchor: Pre_GeometryPointer, name: nameof(SkinPointer));
            
            s.SerializePadding(4, logIfNotNull: true);
            TEX0 = s.SerializeObject<GSReg_TEX0>(TEX0, name: nameof(TEX0));

            if (HasSkin)
            {
                s.DoAt(SkinPointer, () => SkinDescriptors = s.SerializeObjectArray<ModelSkinDescriptor>(SkinDescriptors, SkinDescriptorCount, name: nameof(SkinDescriptors), onPreSerialize: x =>
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
            s.DoAt(IndicesPointer, () => TriangleStrips = s.SerializeObjectArray<ModelTriangleStrip>(TriangleStrips, TriangleStripCount, name: nameof(TriangleStrips)));
        }
    }
}