namespace BinarySerializer.Klonoa.LV
{
    // Data in this archive gets loaded into VRAM, IOP RAM, and SPU2 RAM before loading the level data archive
    public class PreloadDataPack_ArchiveFile : ArchiveFile
    {
        public ArchiveFile<GMS_File> NakanoPack { get; set; }
        public ArchiveFile OkanoPack { get; set; }
        public PreloadHoshinoPack_ArchiveFile HoshinoPack { get; set; }
        public ArchiveFile HaradaPack { get; set; }
        public PreloadAbePack_ArchiveFile AbePack { get; set; } // Particle and reflection textures
        public ArchiveFile HatoPack { get; set; }
        public ArchiveFile TakePack { get; set; }
        public ArchiveFile KazuyaPack { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            NakanoPack = SerializeFile(s, NakanoPack, 0, name: nameof(NakanoPack));
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