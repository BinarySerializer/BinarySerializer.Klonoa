namespace BinarySerializer.Klonoa.BV
{
    public class VMDG_File : BaseFile
    {
        #region Header
        public string ID { get; set; }
        public ushort Ushort_0C { get; set; }
        public ushort Ushort_0E { get; set; }
        public ushort BoneCount { get; set; }
        public ushort MeshCount { get; set; }
        public ushort TriangleCount { get; set; }
        public ushort VertexCount { get; set; }
        public ushort NormalCount { get; set; }
        public ushort TexcoordGroupCount { get; set; }
        public ushort TexcoordCount { get; set; }
        #endregion

        #region Buffers
        public VMDG_Bone[] Bones { get; set; }
        public VMDG_Mesh[] Meshes { get; set; }
        public VMDG_Triangle[] Triangles { get; set; }
        public VMDG_WeightedVector[] Vertices { get; set; }
        public VMDG_WeightedVector[] Normals { get; set; }
        public VMDG_TexcoordGroup[] TexcoordGroups { get; set; }
        public VMDG_Texcoord[] Texcoords { get; set; }
        #endregion

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("VMDG", 4);
            s.SerializePadding(4, logIfNotNull: true);
            ID = s.SerializeString(ID, 2, name: nameof(ID));
            s.SerializePadding(2, logIfNotNull: true);
            Ushort_0C = s.Serialize<ushort>(Ushort_0C, name: nameof(Ushort_0C));
            Ushort_0E = s.Serialize<ushort>(Ushort_0E, name: nameof(Ushort_0E));
            BoneCount = s.Serialize<ushort>(BoneCount, name: nameof(BoneCount));
            MeshCount = s.Serialize<ushort>(MeshCount, name: nameof(MeshCount));
            TriangleCount = s.Serialize<ushort>(TriangleCount, name: nameof(TriangleCount));
            VertexCount = s.Serialize<ushort>(VertexCount, name: nameof(VertexCount));
            NormalCount = s.Serialize<ushort>(NormalCount, name: nameof(NormalCount));
            TexcoordGroupCount = s.Serialize<ushort>(TexcoordGroupCount, name: nameof(TexcoordGroupCount));
            TexcoordCount = s.Serialize<ushort>(TexcoordCount, name: nameof(TexcoordCount));
            s.SerializePadding(34, logIfNotNull: true);

            Bones = s.SerializeObjectArray<VMDG_Bone>(Bones, BoneCount, name: nameof(Bones));
            Meshes = s.SerializeObjectArray<VMDG_Mesh>(Meshes, MeshCount, name: nameof(Meshes));
            Triangles = s.SerializeObjectArray<VMDG_Triangle>(Triangles, TriangleCount, name: nameof(Triangles));
            Vertices = s.SerializeObjectArray<VMDG_WeightedVector>(Vertices, VertexCount, name: nameof(Vertices));
            Normals = s.SerializeObjectArray<VMDG_WeightedVector>(Normals, NormalCount, name: nameof(Normals));
            TexcoordGroups = s.SerializeObjectArray<VMDG_TexcoordGroup>(TexcoordGroups, TexcoordGroupCount, name: nameof(TexcoordGroups));
            Texcoords = s.SerializeObjectArray<VMDG_Texcoord>(Texcoords, TexcoordCount, name: nameof(Texcoords));
        }
    }
}