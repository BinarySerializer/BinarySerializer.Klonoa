using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class TMDVertices_File : BaseFile
    {
        public long? Pre_VerticesCount { get; set; }

        public PS1_TMD_Vertex[] Vertices { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Vertices = s.SerializeObjectArray<PS1_TMD_Vertex>(Vertices, Pre_VerticesCount ?? Pre_FileSize / 8, name: nameof(Vertices));
        }
    }
}