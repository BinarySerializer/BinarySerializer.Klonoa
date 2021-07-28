namespace BinarySerializer.KlonoaDTP
{
    public class WorldMapGraphics_ArchiveFile : ArchiveFile
    {
        public SpriteAnimations_File SpriteAnimations { get; set; }
        public Sprites_ArchiveFile Sprites { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            SpriteAnimations = SerializeFile<SpriteAnimations_File>(s, SpriteAnimations, 0, name: nameof(SpriteAnimations));
            Sprites = SerializeFile<Sprites_ArchiveFile>(s, Sprites, 1, name: nameof(Sprites));
        }
    }
}