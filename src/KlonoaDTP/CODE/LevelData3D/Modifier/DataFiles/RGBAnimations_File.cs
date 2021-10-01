namespace BinarySerializer.Klonoa.DTP
{
    public class RGBAnimations_File : BaseFile
    {
        public RGBAnimation[] Animations { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Animations = s.SerializeObjectArrayUntil(Animations, x => x.ModelObjectIndex == -1, () => new RGBAnimation()
            {
                ModelObjectIndex = -1
            }, name: nameof(Animations));
        }
    }
}