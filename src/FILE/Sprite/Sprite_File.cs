namespace BinarySerializer.KlonoaDTP
{
    /// <summary>
    /// A sprite, consisting of multiple textures
    /// </summary>
    public class Sprite_File : BaseFile
    {
        public SpriteTexture[] Textures { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Textures = s.SerializeObjectArray<SpriteTexture>(Textures, Pre_FileSize / 12, name: nameof(Textures));
        }
    }
}