namespace BinarySerializer.Klonoa.DTP
{
    // According to the prototype the source extensions for the data in this file were .TIA and .NAK
    public class LevelPack_ArchiveFile : ArchiveFile
    {
        public ArchiveFile ObjectAssets { get; set; } // Referenced from objects
        public ArchiveFile<RawData_File> AdditionalAssets { get; set; } // Additional assets. Parsed from hard-coded functions. Has the boss data.
        
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
            ObjectAssets = SerializeFile<ArchiveFile>(s, ObjectAssets, 0, name: nameof(ObjectAssets));
            AdditionalAssets = SerializeFile<ArchiveFile<RawData_File>>(s, AdditionalAssets, 1, name: nameof(AdditionalAssets));
            CutscenePack = SerializeFile<CutscenePack_ArchiveFile>(s, CutscenePack, 2, name: nameof(CutscenePack));
            File_3 = SerializeFile<RawData_File>(s, File_3, 3, name: nameof(File_3));
            File_4 = SerializeFile<RawData_ArchiveFile>(s, File_4, 4, name: nameof(File_4));
            File_5 = SerializeFile<RawData_ArchiveFile>(s, File_5, 5, name: nameof(File_5));
            File_6 = SerializeFile<RawData_File>(s, File_6, 6, name: nameof(File_6));
            File_7 = SerializeFile<RawData_File>(s, File_7, 7, name: nameof(File_7));

            // The last file is always a dummy file to show the game that it's the last sector
            var sectorsCount = OffsetTable.FilesCount - 9;

            // Every third level (every boss) is not compressed
            var isCompressed = !Loader_DTP.GetLoader(s.Context).IsBossFight;
            var encoder = isCompressed ? new LevelSectorEncoder() : null;

            Sectors ??= new Sector_ArchiveFile[sectorsCount];

            var sectorToParse = Loader_DTP.GetLoader(s.Context).LevelSector;

            for (int i = 0; i < sectorsCount; i++)
            {
                if (sectorToParse == -1 || sectorToParse == i)
                    // TODO: Re-enable logging if not fully parsed. Had to disable since the encoder often reads 2 bytes too many.
                    Sectors[i] = SerializeFile<Sector_ArchiveFile>(s, Sectors[i], 8 + i, logIfNotFullyParsed: !isCompressed, fileEncoder: encoder, name: $"{Sectors}[{i}]");
            }
        }
    }
}