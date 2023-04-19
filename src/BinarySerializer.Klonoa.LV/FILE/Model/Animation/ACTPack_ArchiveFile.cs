namespace BinarySerializer.Klonoa.LV 
{
    public class ACTPack_ArchiveFile : ArchiveFile
    {
        public INF_File BoneData { get; set; }
        public ACT_File[] Animations { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            if (OffsetTable.FilesCount != 0)
            {
                BoneData = SerializeFile(s, BoneData, 0, name: nameof(BoneData));
                Animations = new ACT_File[OffsetTable.FilesCount - 1];
                for (int i = 1; i < OffsetTable.FilesCount; i++)
                    Animations[i - 1] = SerializeFile(s, Animations[i - 1], i, name: $"{nameof(Animations)}[{i}]");
            }
        }
    }
}