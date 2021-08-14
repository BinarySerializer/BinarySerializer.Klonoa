namespace BinarySerializer.Klonoa.DTP
{
    // According to the prototype the source extensions for the data in this file were .TIA and .NAK
    public class LevelPack_ArchiveFile : ArchiveFile
    {
        public ArchiveFile AdditionalLevelFilePack { get; set; } // Referenced from objects

        // TODO: Parse this - Seems to be a bunch of stuff like sprites etc., some models too. Seems to be unused though?
        public ArchiveFile<RawData_File> File_1 { get; set; }
        
        public CutscenePack_ArchiveFile CutscenePack { get; set; }

        // TODO: Parse these - not available in all levels
        public RawData_File File_3 { get; set; }
        public RawData_ArchiveFile File_4 { get; set; }
        public RawData_ArchiveFile File_5 { get; set; }
        public RawData_File File_6 { get; set; }
        public RawData_File File_7 { get; set; }

        // A level is made out of multiple sectors (changes when Klonoa walks through a door etc.)
        public Sector_ArchiveFile[] Sectors { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            AdditionalLevelFilePack = SerializeFile<ArchiveFile>(s, AdditionalLevelFilePack, 0, name: nameof(AdditionalLevelFilePack));
            File_1 = SerializeFile<ArchiveFile<RawData_File>>(s, File_1, 1, name: nameof(File_1));
            CutscenePack = SerializeFile<CutscenePack_ArchiveFile>(s, CutscenePack, 2, name: nameof(CutscenePack));
            File_3 = SerializeFile<RawData_File>(s, File_3, 3, name: nameof(File_3));
            File_4 = SerializeFile<RawData_ArchiveFile>(s, File_4, 4, name: nameof(File_4));
            File_5 = SerializeFile<RawData_ArchiveFile>(s, File_5, 5, name: nameof(File_5));
            File_6 = SerializeFile<RawData_File>(s, File_6, 6, name: nameof(File_6));
            File_7 = SerializeFile<RawData_File>(s, File_7, 7, name: nameof(File_7));

            // The last file is always a dummy file to show the game that it's the last sector
            var sectorsCount = OffsetTable.FilesCount - 9;

            Sectors ??= new Sector_ArchiveFile[sectorsCount];

            var sectorToParse = Loader_DTP.GetLoader(s.Context).LevelSector;

            for (int i = 0; i < sectorsCount; i++)
            {
                if (sectorToParse == -1 || sectorToParse == i)
                    Sectors[i] = SerializeFile<Sector_ArchiveFile>(s, Sectors[i], 8 + i, name: $"{Sectors}[{i}]");
            }
        }
    }
}