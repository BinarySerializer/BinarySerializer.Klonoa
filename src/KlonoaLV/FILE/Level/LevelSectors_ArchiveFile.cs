namespace BinarySerializer.Klonoa.LV
{
    public class LevelSectors_ArchiveFile : ArchiveFile
    {
        public RawData_File LevelSectorsDescriptor { get; set; } // Not sure what this file is for, but let's just assume it's some sort of descriptor given its position in the archive
        public LevelSector_ArchiveFile[] LevelSectors { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            LevelSectorsDescriptor = SerializeFile(s, LevelSectorsDescriptor, 0, name: nameof(LevelSectorsDescriptor));
            LevelSectors = new LevelSector_ArchiveFile[OffsetTable.FilesCount - 1];
            for (int i = 1; i < OffsetTable.FilesCount; i++) {
                LevelSectors[i - 1] = SerializeFile(s, LevelSectors[i - 1], i, name: $"{nameof(LevelSectors)}[{i}]");
            }
        }
    }
}