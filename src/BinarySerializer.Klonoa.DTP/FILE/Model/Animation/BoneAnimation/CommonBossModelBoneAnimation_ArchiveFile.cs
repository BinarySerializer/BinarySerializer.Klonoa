using System.Linq;

namespace BinarySerializer.Klonoa.DTP
{
    public class CommonBossModelBoneAnimation_ArchiveFile : ArchiveFile
    {
        public ushort Pre_ModelsCount { get; set; }
        public bool Pre_DoModelPositionsComeFirst { get; set; } = false;
        public bool Pre_DoesPositionsFileHaveHeader { get; set; } = false;
        public bool Pre_HasInitialPositions { get; set; } = true;

        public RawData_File File_0 { get; set; }
        public VectorAnimation_File Positions { get; set; }
        public VectorAnimationKeyFrames_File[] Rotations { get; set; }
        public VectorAnimation_File[] ModelPositions { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<RawData_File>(s, File_0, 0, name: nameof(File_0));

            var startFileIndex = Pre_HasInitialPositions ? 2 : 1;
            var animsCount = (OffsetTable.FilesCount - startFileIndex) / 2;

            Rotations ??= new VectorAnimationKeyFrames_File[animsCount];
            ModelPositions ??= new VectorAnimation_File[animsCount];

            for (int i = 0; i < animsCount; i++)
            {
                Rotations[i] = SerializeFile<VectorAnimationKeyFrames_File>(s, Rotations[i], startFileIndex + (i * 2) + (Pre_DoModelPositionsComeFirst ? 1 : 0), name: $"{nameof(Rotations)}[{i}]");
                ModelPositions[i] = SerializeFile<VectorAnimation_File>(s, ModelPositions[i], startFileIndex + (i * 2) + (Pre_DoModelPositionsComeFirst ? 0 : 1), onPreSerialize: x =>
                {
                    x.Pre_ObjectsCount = Pre_ModelsCount;
                }, name: $"{nameof(ModelPositions)}[{i}]");
            }

            if (Pre_HasInitialPositions)
            {
                Positions = SerializeFile<VectorAnimation_File>(s, Positions, 1, onPreSerialize: x =>
                {
                    if (!Pre_DoesPositionsFileHaveHeader)
                    {
                        x.Pre_ObjectsCount = Rotations.First().BonesCount;
                        x.Pre_FramesCount = 1;
                    }
                }, name: nameof(Positions));
            }
        }
    }
}