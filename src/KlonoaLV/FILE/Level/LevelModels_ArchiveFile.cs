using System.Collections.Generic;

namespace BinarySerializer.Klonoa.LV {
    public class LevelModels_ArchiveFile : ArchiveFile {
        public ArchiveFile<Model_ArchiveFile>[] Models;
        public RawData_File File_Last { get; set; } // ?

        protected override void SerializeFiles(SerializerObject s)
        {   
            Models = new ArchiveFile<Model_ArchiveFile>[OffsetTable.FilesCount - 1];
            List<long> serializedOffsets = new List<long>();
            serializedOffsets.Add(OffsetTable.FilePointers[OffsetTable.FilesCount - 1].FileOffset); // Don't parse the last file as a model
            for (int i = 0; i < OffsetTable.FilesCount - 1; i++)
            {
                // Don't serialize models that have already been serialized
                if (serializedOffsets.Contains(OffsetTable.FilePointers[i].FileOffset))
                    continue;
                
                Models[i] = SerializeFile<ArchiveFile<Model_ArchiveFile>>(s, Models[i], i, name: $"{nameof(Models)}[{i}]");
                serializedOffsets.Add(OffsetTable.FilePointers[i].FileOffset);
            }

            File_Last = SerializeFile(s, File_Last, OffsetTable.FilesCount - 1, name: nameof(File_Last));
        }
    }
}