namespace BinarySerializer.Klonoa.LV
{
    public class LevelDataPack_ArchiveFile : ArchiveFile
    {
        public int Pre_Level { get; set; } = -1; // Semi-hacky way to pass level down for parsing hard-coded files

        public LevelNakanoPack_ArchiveFile NakanoPack { get; set; } 
        public LevelOkanoPack_ArchiveFile OkanoPack { get; set; }
        public ArchiveFile<RawData_ArchiveFile> HoshinoPack { get; set; } // ? (seems like the number of archives inside this archive is equal to the number of level sectors)
        public LevelHaradaPack_ArchiveFile HaradaPack { get; set; }
        public RawData_ArchiveFile AbePack { get; set; } // ?
        public RawData_File HatoPack { get; set; } 
        public RawData_File TakePack { get; set; }
        public LevelKazuyaPack_ArchiveFile KazuyaPack { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            NakanoPack = SerializeFile(s, NakanoPack, 0, onPreSerialize: x => x.Pre_Level = Pre_Level, name: nameof(NakanoPack));
            OkanoPack = SerializeFile(s, OkanoPack, 1, name: nameof(OkanoPack));
            HoshinoPack = SerializeFile(s, HoshinoPack, 2, name: nameof(HoshinoPack));
            HaradaPack = SerializeFile(s, HaradaPack, 3, name: nameof(HaradaPack));
            AbePack = SerializeFile(s, AbePack, 4, name: nameof(AbePack));
            HatoPack = SerializeFile(s, HatoPack, 5, name: nameof(HatoPack));
            TakePack = SerializeFile(s, TakePack, 6, name: nameof(TakePack));
            KazuyaPack = SerializeFile(s, KazuyaPack, 7, name: nameof(KazuyaPack));
        }
    }
}