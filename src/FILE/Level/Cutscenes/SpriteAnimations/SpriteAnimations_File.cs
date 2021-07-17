namespace BinarySerializer.KlonoaDTP
{
    public class SpriteAnimations_File : BaseFile
    {
        public uint AnimationsCount { get; set; }
        public SpriteAnimation[] Animations { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            AnimationsCount = s.Serialize<uint>(AnimationsCount, name: nameof(AnimationsCount));
            Animations = s.SerializeObjectArray<SpriteAnimation>(Animations, AnimationsCount, x => x.Pre_OffsetAnchor = Offset, name: nameof(Animations));

            // Go to the end of the file
            s.Goto(Offset + Pre_FileSize);
        }
    }
}