using System.Linq;

namespace BinarySerializer.Klonoa.DTP
{
    public class KaralModelBoneAnimation_ArchiveFile : ArchiveFile
    {
        public RawData_File File_0 { get; set; }
        public VectorAnimation_File Positions { get; set; }
        public VectorAnimationKeyFrames_File[] Rotations { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<RawData_File>(s, File_0, 0, name: nameof(File_0));

            Rotations ??= new VectorAnimationKeyFrames_File[OffsetTable.FilesCount - 2];

            for (int i = 0; i < Rotations.Length; i++)
                Rotations[i] = SerializeFile<VectorAnimationKeyFrames_File>(s, Rotations[i], 2 + i, name: $"{nameof(Rotations)}[{i}]");

            Positions = SerializeFile<VectorAnimation_File>(s, Positions, 1, onPreSerialize: x =>
            {
                x.Pre_ObjectsCount = Rotations.First().BonesCount;
                x.Pre_FramesCount = 1;
            }, name: nameof(Positions));
        }
    }
}