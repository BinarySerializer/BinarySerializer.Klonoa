namespace BinarySerializer.KlonoaDTP
{
    public class LevelPack_ArchiveFile : BaseArchiveFile
    {
        // TODO: Parse this - most files are TMD, but not all
        public RawData_ArchiveFile ObjectModels { get; set; }

        // TODO: Parse this - Seems to be a bunch of stuff like sprites etc.
        public ArchiveFile<ArchiveFile<ArchiveFile<RawData_File>>> File_1 { get; set; }
        
        public AnimationPack_ArchiveFile AnimationPack { get; set; }

        // TODO: Parse these - not available in all levels
        public RawData_File File_3 { get; set; }
        public RawData_File File_4 { get; set; }
        public RawData_File File_5 { get; set; } // Archive?
        public RawData_File File_6 { get; set; }
        public RawData_File File_7 { get; set; }

        // A level is made out of multiple sectors (changes when Klonoa walks through a door etc.)
        public Sector_ArchiveFile[] Sectors { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            ObjectModels = SerializeFile<RawData_ArchiveFile>(s, ObjectModels, 0, name: nameof(ObjectModels));
            File_1 = SerializeFile<ArchiveFile<ArchiveFile<ArchiveFile<RawData_File>>>>(s, File_1, 1, name: nameof(File_1));
            AnimationPack = SerializeFile<AnimationPack_ArchiveFile>(s, AnimationPack, 2, name: nameof(AnimationPack));

            File_3 = SerializeFile<RawData_File>(s, File_3, 3, name: nameof(File_3));
            File_4 = SerializeFile<RawData_File>(s, File_4, 4, name: nameof(File_4));
            File_5 = SerializeFile<RawData_File>(s, File_5, 5, name: nameof(File_5));
            File_6 = SerializeFile<RawData_File>(s, File_6, 6, name: nameof(File_6));
            File_7 = SerializeFile<RawData_File>(s, File_7, 7, name: nameof(File_7));

            // The last file is always a dummy file to show the game that it's the last sector
            var sectorsCount = OffsetTable.FilesCount - 9;

            Sectors ??= new Sector_ArchiveFile[sectorsCount];

            for (int i = 0; i < sectorsCount; i++)
                Sectors[i] = SerializeFile<Sector_ArchiveFile>(s, Sectors[i], 8 + i, name: $"{Sectors}[{i}]");
        }
    }
}