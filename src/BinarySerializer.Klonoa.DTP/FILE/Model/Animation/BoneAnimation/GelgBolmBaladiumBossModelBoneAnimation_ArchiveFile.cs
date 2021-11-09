using System.Linq;

namespace BinarySerializer.Klonoa.DTP
{
    public class GelgBolmBaladiumBossModelBoneAnimation_ArchiveFile : ArchiveFile
    {
        public ushort Pre_ModelsCount { get; set; }

        public RawData_File File_0 { get; set; }
        public VectorAnimation_File Positions { get; set; }
        public VectorAnimationKeyFrames_File[] Rotations { get; set; }
        public VectorAnimation_File[] ModelPositions { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<RawData_File>(s, File_0, 0, name: nameof(File_0));

            var animsCount = (OffsetTable.FilesCount - 2) / 2;

            Rotations ??= new VectorAnimationKeyFrames_File[animsCount];
            ModelPositions ??= new VectorAnimation_File[animsCount];

            for (int i = 0; i < animsCount; i++)
            {
                Rotations[i] = SerializeFile<VectorAnimationKeyFrames_File>(s, Rotations[i], 2 + (i * 2), name: $"{nameof(Rotations)}[{i}]");
                ModelPositions[i] = SerializeFile<VectorAnimation_File>(s, ModelPositions[i], 2 + (i * 2) + 1, onPreSerialize: x =>
                {
                    x.Pre_ObjectsCount = Pre_ModelsCount;
                }, name: $"{nameof(ModelPositions)}[{i}]");
            }

            Positions = SerializeFile<VectorAnimation_File>(s, Positions, 1, onPreSerialize: x =>
            {
                x.Pre_ObjectsCount = Rotations.First().BonesCount;
                x.Pre_FramesCount = 1;
            }, name: nameof(Positions));
        }
    }
}