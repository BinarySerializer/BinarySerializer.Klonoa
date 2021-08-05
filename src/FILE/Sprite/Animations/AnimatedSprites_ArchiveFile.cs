namespace BinarySerializer.KlonoaDTP
{
    public class AnimatedSprites_ArchiveFile : ArchiveFile
    {
        public SpriteAnimations_File Animations { get; set; }
        public Sprites_ArchiveFile Sprites { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Animations = SerializeFile<SpriteAnimations_File>(s, Animations, 0, name: nameof(Animations));
            Sprites = SerializeFile<Sprites_ArchiveFile>(s, Sprites, 1, name: nameof(Sprites));
        }
    }
}