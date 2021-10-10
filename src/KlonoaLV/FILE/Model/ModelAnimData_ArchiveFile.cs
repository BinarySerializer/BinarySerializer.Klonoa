namespace BinarySerializer.Klonoa.LV {
    public class ModelAnimData_ArchiveFile : ArchiveFile {
        public ModelBoneData_File ModelBoneData { get; set; }
        public ModelAnimation_File[] ModelAnimations { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            if (OffsetTable.FilesCount != 0)
            {
                ModelBoneData = SerializeFile(s, ModelBoneData, 0, name: nameof(ModelBoneData));
                ModelAnimations = new ModelAnimation_File[OffsetTable.FilesCount - 1];
                for (int i = 1; i < OffsetTable.FilesCount; i++)
                {
                    ModelAnimations[i - 1] = SerializeFile(s, ModelAnimations[i - 1], i, name: $"{nameof(ModelAnimations)}[{i}]");
                }
            }
            
        }
    }
}