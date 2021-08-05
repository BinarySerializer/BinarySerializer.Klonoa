namespace BinarySerializer.KlonoaDTP
{
    public class MenuSprites_ArchiveFile : ArchiveFile
    {
        public Sprites_ArchiveFile Sprites_0 { get; set; } // Menu text
        public Sprites_ArchiveFile Sprites_1 { get; set; }
        public Sprites_ArchiveFile Sprites_2 { get; set; }
        public AnimatedSprites_ArchiveFile AnimatedSprites { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Sprites_0 = SerializeFile<Sprites_ArchiveFile>(s, Sprites_0, 0, name: nameof(Sprites_0));
            Sprites_1 = SerializeFile<Sprites_ArchiveFile>(s, Sprites_1, 1, name: nameof(Sprites_1));
            Sprites_2 = SerializeFile<Sprites_ArchiveFile>(s, Sprites_2, 2, name: nameof(Sprites_2));
            AnimatedSprites = SerializeFile<AnimatedSprites_ArchiveFile>(s, AnimatedSprites, 3, name: nameof(AnimatedSprites));
        }
    }
}