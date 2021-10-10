namespace BinarySerializer.Klonoa.LV
{
    public class WorldTextures_ArchiveFile : ArchiveFile<MiscTextures_ArchiveFile>
    {
        public MiscTextures_ArchiveFile ParticleTextures { get; set; }
        public MiscTextures_ArchiveFile ReflectionTextures { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            ParticleTextures = SerializeFile(s, ParticleTextures, 0, name: nameof(ParticleTextures));
            ReflectionTextures = SerializeFile(s, ReflectionTextures, 1, name: nameof(ReflectionTextures));
        }
    }
}