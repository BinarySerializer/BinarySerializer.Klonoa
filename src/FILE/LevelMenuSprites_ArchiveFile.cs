namespace BinarySerializer.KlonoaDTP
{
    public class LevelMenuSprites_ArchiveFile : ArchiveFile
    {
        public Sprites_ArchiveFile Sprites_0 { get; set; }
        public Sprites_ArchiveFile Sprites_1 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Sprites_0 = SerializeFile<Sprites_ArchiveFile>(s, Sprites_0, 0, name: nameof(Sprites_0));
            Sprites_1 = SerializeFile<Sprites_ArchiveFile>(s, Sprites_1, 1, name: nameof(Sprites_1));
        }
    }
}