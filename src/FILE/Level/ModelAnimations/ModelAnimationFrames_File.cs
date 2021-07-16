namespace BinarySerializer.KlonoaDTP
{
    public class ModelAnimationFrames_File : BaseFile
    {
        public ModelAnimationFrame[] Frames { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Frames = s.SerializeObjectArray<ModelAnimationFrame>(Frames, Pre_FileSize / 28, name: nameof(Frames));
        }
    }
}