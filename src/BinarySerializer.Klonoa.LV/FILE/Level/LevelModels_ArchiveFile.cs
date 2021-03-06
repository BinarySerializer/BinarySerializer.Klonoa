using System.Collections.Generic;

namespace BinarySerializer.Klonoa.LV
{
    public class LevelModels_ArchiveFile : ArchiveFile
    {
        public ArchiveFile<Model_ArchiveFile>[] Models { get; set; }
        public RawData_File File_Last { get; set; } // ?

        protected override void SerializeFiles(SerializerObject s)
        {   
            Models = new ArchiveFile<Model_ArchiveFile>[OffsetTable.FilesCount - 1];
            for (int i = 0; i < OffsetTable.FilesCount - 1; i++)
            {
                // Some models have duplicate offsets. This prevents it from serializing the last file as a model.
                if (OffsetTable.FilePointers[i].FileOffset == OffsetTable.FilePointers[OffsetTable.FilesCount - 1].FileOffset)
                    break;
                Models[i] = SerializeFile<ArchiveFile<Model_ArchiveFile>>(s, Models[i], i, name: $"{nameof(Models)}[{i}]");
            }

            File_Last = SerializeFile(s, File_Last, OffsetTable.FilesCount - 1, name: nameof(File_Last));
        }
    }
}