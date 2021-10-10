namespace BinarySerializer.Klonoa.LV
{
    // Data in this archive gets loaded into VRAM, IOP RAM, and SPU2 RAM before loading the level data archive
    public class LevelPreloadPack_ArchiveFile : ArchiveFile
    {
        public ArchiveFile<GSTextures_File> LevelSprites { get; set; }
        public RawData_File Boundary0 { get; set; }
        public SoundbankData_ArchiveFile SoundbankData { get; set; }
        public RawData_File Boundary1 { get; set; }
        public WorldTextures_ArchiveFile WorldTextures { get; set; } // Particle and reflection textures
        public RawData_File Boundary2 { get; set; }
        public RawData_File Boundary3 { get; set; }
        public RawData_File Boundary4 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            LevelSprites = SerializeFile(s, LevelSprites, 0, name: nameof(LevelSprites));
            Boundary0 = SerializeFile(s, Boundary0, 1, name: nameof(Boundary0));
            SoundbankData = SerializeFile(s, SoundbankData, 2, name: nameof(SoundbankData));
            Boundary1 = SerializeFile(s, Boundary1, 3, name: nameof(Boundary1));
            WorldTextures = SerializeFile(s, WorldTextures, 4, name: nameof(WorldTextures));
            Boundary2 = SerializeFile(s, Boundary2, 5, name: nameof(Boundary2));
            Boundary3 = SerializeFile(s, Boundary3, 6, name: nameof(Boundary3));
            Boundary4 = SerializeFile(s, Boundary4, 7, name: nameof(Boundary4));
        }
    }
}