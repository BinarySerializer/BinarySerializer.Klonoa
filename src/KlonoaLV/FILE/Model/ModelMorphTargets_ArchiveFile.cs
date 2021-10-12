using System.Collections.Generic;

namespace BinarySerializer.Klonoa.LV {
    public class ModelMorphTargets_ArchiveFile : ArchiveFile {
        public ModelMorphTarget_File[] ModelMorphTargets { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            if (OffsetTable.FilesCount != 0)
            {
                ModelMorphTargets = new ModelMorphTarget_File[OffsetTable.FilesCount];
                for (int i = 0; i < OffsetTable.FilesCount; i++)
                {
                    // Don't parse empty morph targets
                    if ((i != OffsetTable.FilesCount - 1 ? OffsetTable.FilePointers[i + 1].SerializedOffset : Pre_FileSize) - OffsetTable.FilePointers[i].SerializedOffset == 0x10)
                        continue;
                    
                    ModelMorphTargets[i] = SerializeFile(s, ModelMorphTargets[i], i, name: $"{nameof(ModelMorphTargets)}[{i}]");
                }
            }
            
        }
    }
}