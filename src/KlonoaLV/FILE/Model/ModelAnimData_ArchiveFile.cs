using System.Collections.Generic;

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
                List<long> serializedOffsets = new List<long>();
                for (int i = 1; i < OffsetTable.FilesCount; i++)
                {
                    // Don't serialize animations that have already been serialized
                    if (serializedOffsets.Contains(OffsetTable.FilePointers[i].FileOffset))
                        continue;
                    
                    ModelAnimations[i - 1] = SerializeFile(s, ModelAnimations[i - 1], i, name: $"{nameof(ModelAnimations)}[{i}]");
                    serializedOffsets.Add(OffsetTable.FilePointers[i].FileOffset);
                }
            }
            
        }
    }
}