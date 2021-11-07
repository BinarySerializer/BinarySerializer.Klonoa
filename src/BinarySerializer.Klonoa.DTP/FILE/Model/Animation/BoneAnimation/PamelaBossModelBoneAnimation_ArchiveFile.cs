namespace BinarySerializer.Klonoa.DTP
{
    public class PamelaBossModelBoneAnimation_ArchiveFile : ArchiveFile
    {
        public RawData_File File_0 { get; set; }
        public VectorAnimation_File[] Positions { get; set; }
        public VectorAnimationKeyFrames_File[] Rotations { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<RawData_File>(s, File_0, 0, name: nameof(File_0));

            var animsCount = (OffsetTable.FilesCount - 1) / 2;

            Positions ??= new VectorAnimation_File[animsCount];
            Rotations ??= new VectorAnimationKeyFrames_File[animsCount];

            for (int i = 0; i < animsCount; i++)
            {
                Positions[i] = SerializeFile<VectorAnimation_File>(s, Positions[i], 1 + (i * 2), name: $"{nameof(Positions)}[{i}]");
                Rotations[i] = SerializeFile<VectorAnimationKeyFrames_File>(s, Rotations[i], 1 + (i * 2) + 1, name: $"{nameof(Rotations)}[{i}]");
            }
        }
    }
}