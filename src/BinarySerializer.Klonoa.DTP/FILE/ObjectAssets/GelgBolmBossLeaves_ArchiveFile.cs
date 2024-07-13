using BinarySerializer.PlayStation.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class GelgBolmBossLeaves_ArchiveFile : ArchiveFile
    {
        public TMD[] Models { get; set; }
        public RawData_File[] UnknownFiles { get; set; } // TODO: Parse
        // 4-9: Pairs of vertices/normals
        // 10-> ??

        protected override void SerializeFiles(SerializerObject s)
        {
            Models ??= new TMD[4];

            for (int i = 0; i < Models.Length; i++)
                Models[i] = SerializeFile<TMD>(s, Models[i], i, name: $"{nameof(Models)}[{i}]");

            UnknownFiles ??= new RawData_File[OffsetTable.FilesCount - 4];

            for (int i = 0; i < UnknownFiles.Length; i++)
                UnknownFiles[i] = SerializeFile<RawData_File>(s, UnknownFiles[i], 4 + i, name: $"{nameof(UnknownFiles)}[{i}]");
        }
    }
}