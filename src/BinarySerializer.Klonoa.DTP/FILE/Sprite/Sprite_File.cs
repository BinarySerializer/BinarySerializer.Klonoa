namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// A sprite, consisting of multiple textures
    /// </summary>
    public class Sprite_File : BaseFile
    {
        public SpriteTexture[] Textures { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            if (Pre_FileSize == 4)
                s.SerializePadding(4, logIfNotNull: true);

            Textures = s.SerializeObjectArray<SpriteTexture>(Textures, Pre_FileSize / 12, name: nameof(Textures));
        }
    }
}