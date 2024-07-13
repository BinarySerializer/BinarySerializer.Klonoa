using BinarySerializer.PlayStation.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class NahatombBossCanons_ArchiveFile : ArchiveFile
    {
        public TMD CanonBase { get; set; }
        public TMD[] Canons { get; set; }
        public RawData_File[] VertexAnimations { get; set; } // TODO: Parse

        protected override void SerializeFiles(SerializerObject s)
        {
            CanonBase = SerializeFile(s, CanonBase, 0, name: nameof(CanonBase));

            Canons ??= new TMD[5];

            for (int i = 0; i < Canons.Length; i++)
                Canons[i] = SerializeFile<TMD>(s, Canons[i], 1 + i, name: $"{nameof(Canons)}[{i}]");

            VertexAnimations ??= new RawData_File[3];

            for (int i = 0; i < VertexAnimations.Length; i++)
                VertexAnimations[i] = SerializeFile<RawData_File>(s, VertexAnimations[i], 6 + i, name: $"{nameof(VertexAnimations)}[{i}]");
        }
    }
}