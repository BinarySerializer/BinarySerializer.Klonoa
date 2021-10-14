namespace BinarySerializer.Klonoa.LV
{
    public class ModelGeometry_File : BaseFile
    {
        public string Magic { get; set; } // FX
        public byte[] Magic2 { get; set; } // 80 00 40 00
        public ushort MeshCount { get; set; }
        public float VertexScale { get; set; } // Multiplies vertices by this value
        public ModelMesh[] Meshes { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            Magic = s.SerializeString(Magic, 2, name: nameof(Magic));
            MeshCount = s.Serialize<ushort>(MeshCount, name: nameof(MeshCount));
            Magic2 = s.SerializeArray<byte>(Magic2, 4, name: nameof(Magic2));
            VertexScale = s.Serialize<float>(VertexScale, name: nameof(VertexScale));
            s.SerializePadding(4);
            Meshes = s.SerializeObjectArray<ModelMesh>(Meshes, MeshCount, onPreSerialize: x => x.Pre_GeometryPointer = Offset, name: nameof(Meshes));
        }
    }
}