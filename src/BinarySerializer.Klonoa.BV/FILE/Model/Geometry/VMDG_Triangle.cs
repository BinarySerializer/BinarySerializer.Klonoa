namespace BinarySerializer.Klonoa.BV
{
    public class VMDG_Triangle : BinarySerializable
    {
        /// <summary>
        /// Array of 3 indices of the vertices to use for this triangle from the mesh's local vertex buffer.
        /// </summary>
        public byte[] VertexIndices { get; set; }

        /// <summary>
        /// Array of 3 indices of the normals to use for this triangle from the mesh's local normal buffer.
        /// </summary>
        public byte[] NormalIndices { get; set; }

        /// <summary>
        /// The index of the texcoord to use from the texcoord group.
        /// </summary>
        public byte TexcoordIndex { get; set; }

        /// <summary>
        /// The index of the texcoord group to use in the <c>TexcoordGroups</c> buffer.
        /// </summary>
        public byte TexcoordGroupIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            VertexIndices = s.SerializeArray<byte>(VertexIndices, 3, name: nameof(VertexIndices));
            NormalIndices = s.SerializeArray<byte>(NormalIndices, 3, name: nameof(NormalIndices));
            TexcoordIndex = s.Serialize<byte>(TexcoordIndex, name: nameof(TexcoordIndex));
            TexcoordGroupIndex = s.Serialize<byte>(TexcoordGroupIndex, name: nameof(TexcoordGroupIndex));
        }
    }
}