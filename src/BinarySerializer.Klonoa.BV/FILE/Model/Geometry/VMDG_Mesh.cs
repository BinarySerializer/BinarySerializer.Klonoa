namespace BinarySerializer.Klonoa.BV
{
    public class VMDG_Mesh : BinarySerializable
    {
        /// <summary>
        /// The name of this mesh.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Start index of this mesh's triangles in the <c>Triangles</c> buffer.
        /// </summary>
        public ushort TrianglesOffset { get; set; }

        /// <summary>
        /// End index of this mesh's triangles in the <c>Triangles</c> buffer.
        /// </summary>
        public ushort TrianglesEndOffset { get; set; }

        /// <summary>
        /// Start index of this mesh's vertices in the <c>Vertices</c> buffer.
        /// </summary>
        public ushort VerticesOffset { get; set; }

        /// <summary>
        /// End index of this mesh's vertices in the <c>Vertices</c> buffer.
        /// </summary>
        public ushort VerticesEndOffset { get; set; }

        /// <summary>
        /// Start index of this mesh's normals in the <c>Normals</c> buffer.
        /// </summary>
        public ushort NormalsOffset { get; set; }

        /// <summary>
        /// End index of this mesh's normals in the <c>Normals</c> buffer.
        /// </summary>
        public ushort NormalsEndOffset { get; set; }

        /// <summary>
        /// The number of triangles this mesh has.
        /// </summary>
        public ushort TriangleCount { get; set; }

        /// <summary>
        /// The number of vertices this mesh has.
        /// </summary>
        public byte VertexCount { get; set; }

        /// <summary>
        /// The number of normals this mesh has.
        /// </summary>
        public byte NormalCount { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializePadding(4, logIfNotNull: true);
            Name = s.SerializeString(Name, 12, name: nameof(Name));
            TrianglesOffset = s.Serialize<ushort>(TriangleCount, name: nameof(TrianglesOffset));
            TrianglesEndOffset = s.Serialize<ushort>(TrianglesEndOffset, name: nameof(TrianglesEndOffset));
            VerticesOffset = s.Serialize<ushort>(VerticesOffset, name: nameof(VerticesOffset));
            VerticesEndOffset = s.Serialize<ushort>(VerticesEndOffset, name: nameof(VerticesEndOffset));
            NormalsOffset = s.Serialize<ushort>(NormalsOffset, name: nameof(NormalsOffset));
            NormalsEndOffset = s.Serialize<ushort>(NormalsEndOffset, name: nameof(NormalsEndOffset));
            TriangleCount = s.Serialize<ushort>(TriangleCount, name: nameof(TriangleCount));
            VertexCount = s.Serialize<byte>(VertexCount, name: nameof(VertexCount));
            NormalCount = s.Serialize<byte>(NormalCount, name: nameof(NormalCount));
        }
    }
}