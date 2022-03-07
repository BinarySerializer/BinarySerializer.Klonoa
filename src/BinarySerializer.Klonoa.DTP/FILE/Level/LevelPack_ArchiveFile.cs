namespace BinarySerializer.Klonoa.DTP
{
    // File extension is NAK.

    public class LevelPack_ArchiveFile : ArchiveFile
    {
        public ArchiveFile ObjectAssets { get; set; } // Referenced from objects
        public ArchiveFile BossAssets { get; set; } // Referenced from boss objects
        
        public CutscenePack_ArchiveFile CutscenePack { get; set; }

        public RawData_File File_3 { get; set; } // TODO: Parse. Has sprites in BIN 17.
        public RawData_ArchiveFile File_4 { get; set; } // TODO: Parse. Read from BIN 8, 9, 15, 16, 19, 22, 24. Has models in some of them.
        public ArchiveFile<MovementPath_File> File_5 { get; set; } // TODO: What is this for? Used in BIN 7.
        public RawData_File File_6 { get; set; } // TODO: Parse. Game seems to ignore it? Does it ever have data?
        public RawData_File File_7 { get; set; } // TODO: Parse. Game seems to ignore it? Does it ever have data?

        // A level is made out of multiple sectors (changes when Klonoa walks through a door etc.)
        public Sector_ArchiveFile[] Sectors { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            ObjectAssets = SerializeFile<ArchiveFile>(s, ObjectAssets, 0, name: nameof(ObjectAssets));
            BossAssets = SerializeFile<ArchiveFile>(s, BossAssets, 1, name: nameof(BossAssets));
            CutscenePack = SerializeFile<CutscenePack_ArchiveFile>(s, CutscenePack, 2, name: nameof(CutscenePack));
            File_3 = SerializeFile<RawData_File>(s, File_3, 3, name: nameof(File_3));
            File_4 = SerializeFile<RawData_ArchiveFile>(s, File_4, 4, name: nameof(File_4));
            File_5 = SerializeFile<ArchiveFile<MovementPath_File>>(s, File_5, 5, name: nameof(File_5));
            File_6 = SerializeFile<RawData_File>(s, File_6, 6, name: nameof(File_6));
            File_7 = SerializeFile<RawData_File>(s, File_7, 7, name: nameof(File_7));

            // The last file is always a dummy file to show the game that it's the last sector
            var sectorsCount = OffsetTable.FilesCount - 9;

            // Every third level (every boss) is not compressed
            var isCompressed = !Loader.GetLoader(s.Context).IsBossFight;
            var encoder = isCompressed ? new LevelSectorEncoder() : null;

            Sectors ??= new Sector_ArchiveFile[sectorsCount];

            var sectorToParse = Loader.GetLoader(s.Context).LevelSector;

            for (int i = 0; i < sectorsCount; i++)
            {
                if (sectorToParse == -1 || sectorToParse == i)
                    // TODO: Re-enable logging if not fully parsed. Had to disable since the encoder often reads 2 bytes too many.
                    Sectors[i] = SerializeFile<Sector_ArchiveFile>(s, Sectors[i], 8 + i, logIfNotFullyParsed: !isCompressed, fileEncoder: encoder, name: $"{Sectors}[{i}]");
            }
        }
    }
}