namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// Binds vertices in the mesh to the model bones
    /// </summary>
    public class SFXSubpart : BinarySerializable
    {
        public Pointer Pre_GeometryPointer { get; set; }
        public Pointer Pre_VerticesPointer { get; set; }
        public Pointer Pre_NormalsPointer { get; set; }

        public short[] JointInfluences { get; set; } // The indices of the joints that influence the vertices of the mesh
        public ushort VertexCount { get; set; }
        public ushort NormalCount { get; set; }
        public ushort Ushort_0C { get; set; } // The game does not read this value
        public ushort Ushort_0E { get; set; } // The game does not read this value
        public Pointer VerticesPointer { get; set; } // Uses vertices pointer from the mesh as the anchor pointer
        public Pointer VertexWeightsPointer { get; set; }
        public Pointer NormalsPointer { get; set; } // Uses normals pointer from the mesh as the anchor pointer
        public Pointer WeightsPointer_2 { get; set; } // Seems like it points to another set of weights that's the exact same as the vertex weights...? (normal weights????)
        public KlonoaLV_Vector16[] Vertices { get; set; }
        public SFXVertexWeight[] VertexWeights { get; set; } // Weights of the joint influences for each vertex
        public KlonoaLV_Vector16[] Normals { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            JointInfluences = s.SerializeArray<short>(JointInfluences, 4, name: nameof(JointInfluences));
            VertexCount = s.Serialize<ushort>(VertexCount, name: nameof(VertexCount));
            NormalCount = s.Serialize<ushort>(NormalCount, name: nameof(NormalCount));
            Ushort_0C = s.Serialize<ushort>(Ushort_0C, name: nameof(Ushort_0C));
            Ushort_0E = s.Serialize<ushort>(Ushort_0E, name: nameof(Ushort_0E));
            VerticesPointer = s.SerializePointer(VerticesPointer, anchor: Pre_VerticesPointer, name: nameof(VerticesPointer));
            VertexWeightsPointer = s.SerializePointer(VertexWeightsPointer, anchor: Pre_GeometryPointer, name: nameof(VertexWeightsPointer));
            NormalsPointer = s.SerializePointer(NormalsPointer, anchor: Pre_NormalsPointer, name: nameof(NormalsPointer));
            WeightsPointer_2 = s.SerializePointer(WeightsPointer_2, anchor: Pre_GeometryPointer, name: nameof(WeightsPointer_2));

            s.DoAt(VerticesPointer, () => Vertices = s.SerializeObjectArray<KlonoaLV_Vector16>(Vertices, VertexCount, name: nameof(Vertices)));
            s.DoAt(NormalsPointer, () => Normals = s.SerializeObjectArray<KlonoaLV_Vector16>(Normals, NormalCount, name: nameof(Normals)));
            s.DoAt(VertexWeightsPointer, () => VertexWeights = s.SerializeObjectArray<SFXVertexWeight>(VertexWeights, VertexCount, name: nameof(VertexWeights)));
        }
    }
}