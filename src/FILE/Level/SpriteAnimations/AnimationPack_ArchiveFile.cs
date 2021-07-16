namespace BinarySerializer.KlonoaDTP
{
    public class AnimationPack_ArchiveFile : BaseArchiveFile
    {
        public SpriteAnimations_File SpriteAnimations { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            SpriteAnimations = SerializeFile<SpriteAnimations_File>(s, SpriteAnimations, 0, name: nameof(SpriteAnimations));
            // 1: Sprites
            // 2: Compressed files (TIM?)
            // 3: 
            // ...

            // TODO: Parse the rest
        }
    }
}