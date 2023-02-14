using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class DVFEntry : BinarySerializable
    {
        public short VerticesIndex { get; set; } // Start index in the vertices array
        public short VerticesCount { get; set; }
        public TMD_Vertex[] Vertices { get; set; } // Relative

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("DVF ", 4);
            VerticesIndex = s.Serialize<short>(VerticesIndex, name: nameof(VerticesIndex));
            VerticesCount = s.Serialize<short>(VerticesCount, name: nameof(VerticesCount));
            Vertices = s.SerializeObjectArray<TMD_Vertex>(Vertices, VerticesCount, name: nameof(Vertices));
        }
    }
}