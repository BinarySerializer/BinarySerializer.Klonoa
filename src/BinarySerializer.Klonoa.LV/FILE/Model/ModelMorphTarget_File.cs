namespace BinarySerializer.Klonoa.LV {
    public class ModelMorphTarget_File : BaseFile
    {
        // This is needed to get the vertices/normals pointers from the skin descriptors of the target mesh
        public ModelMesh[] Pre_Meshes { get; set; }
        
        public string Magic { get; set; } // FZ
        public byte[] Magic2 { get; set; } // 80 00 40 00
        public ushort MeshCount { get; set; } // Should always be 1
        public float VertexScale { get; set; }
        public ushort TargetMesh { get; set; }
        public ushort DataSize { get; set; }
        public ushort VertexCount { get; set; }
        public ushort NormalCount { get; set; }
        public Pointer VerticesPointer { get; set; }
        public Pointer NormalsPointer { get; set; }
        
        // Separated by skin descriptor
        public KlonoaLV_Vector16[][] Vertices { get; set; }
        public KlonoaLV_Vector16[][] Normals { get; set; }

        public bool IsEmpty => Pre_FileSize == 16;

        public override void SerializeImpl(SerializerObject s) 
        {
            if (!IsEmpty)
            {
                Magic = s.SerializeString(Magic, 2, name: nameof(Magic));
                MeshCount = s.Serialize<ushort>(MeshCount, name: nameof(MeshCount));
                Magic2 = s.SerializeArray<byte>(Magic2, 4, name: nameof(Magic2));
                VertexScale = s.Serialize<float>(VertexScale, name: nameof(VertexScale));
                s.SerializePadding(4, logIfNotNull: true);
                TargetMesh = s.Serialize<ushort>(TargetMesh, name: nameof(TargetMesh));
                DataSize = s.Serialize<ushort>(DataSize, name: nameof(DataSize));
                VertexCount = s.Serialize<ushort>(VertexCount, name: nameof(VertexCount));
                NormalCount = s.Serialize<ushort>(NormalCount, name: nameof(NormalCount));
                VerticesPointer = s.SerializePointer(VerticesPointer, anchor: Offset, name: nameof(VerticesPointer));
                NormalsPointer = s.SerializePointer(NormalsPointer, anchor: Offset, name: nameof(NormalsPointer));

                ModelMesh mesh = Pre_Meshes[TargetMesh];
                Vertices ??= new KlonoaLV_Vector16[mesh.SkinDescriptorCount][];
                Normals ??= new KlonoaLV_Vector16[mesh.SkinDescriptorCount][];

                for (int i = 0; i < mesh.SkinDescriptorCount; i++)
                {
                    ModelSkinDescriptor skinDescriptor = mesh.SkinDescriptors[i];
                    Pointer verticesPointer = new Pointer(skinDescriptor.VerticesPointer.FileOffset - skinDescriptor.Pre_VerticesPointer.FileOffset + VerticesPointer.FileOffset, Offset.File);
                    Pointer normalsPointer = new Pointer(skinDescriptor.NormalsPointer.FileOffset - skinDescriptor.Pre_NormalsPointer.FileOffset + NormalsPointer.FileOffset, Offset.File);
                    s.DoAt(verticesPointer, () => Vertices[i] = s.SerializeObjectArray<KlonoaLV_Vector16>(Vertices[i], skinDescriptor.VertexCount, name: $"{nameof(Vertices)}[{i}]"));
                    s.DoAt(normalsPointer, () => Normals[i] = s.SerializeObjectArray<KlonoaLV_Vector16>(Normals[i], skinDescriptor.NormalCount, name: $"{nameof(Normals)}[{i}]"));
                }
            }

            s.Goto(Offset + Pre_FileSize);
        }
    }
}