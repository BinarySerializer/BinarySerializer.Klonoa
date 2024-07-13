using BinarySerializer.PlayStation.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class TMDNormals_File : BaseFile
    {
        public long? Pre_NormalsCount { get; set; }

        public TMD_Normal[] Normals { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Normals = s.SerializeObjectArray<TMD_Normal>(Normals, Pre_NormalsCount ?? Pre_FileSize / 8, name: nameof(Normals));
        }
    }
}