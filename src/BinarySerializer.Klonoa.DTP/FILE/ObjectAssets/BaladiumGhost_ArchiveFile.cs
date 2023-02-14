using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class BaladiumGhost_ArchiveFile : ArchiveFile
    {
        public TMD[] Models { get; set; }
        public RawData_File[] UnknownFiles { get; set; } // TODO: Parse

        protected override void SerializeFiles(SerializerObject s)
        {
            Models ??= new TMD[3];

            for (int i = 0; i < Models.Length; i++)
                Models[i] = SerializeFile<TMD>(s, Models[i], i, onPreSerialize: x => x.Pre_HasBones = i == 0, name: $"{nameof(Models)}[{i}]");

            UnknownFiles ??= new RawData_File[OffsetTable.FilesCount - 3];

            for (int i = 0; i < UnknownFiles.Length; i++)
                UnknownFiles[i] = SerializeFile<RawData_File>(s, UnknownFiles[i], 3 + i, name: $"{nameof(UnknownFiles)}[{i}]");
        }
    }
}