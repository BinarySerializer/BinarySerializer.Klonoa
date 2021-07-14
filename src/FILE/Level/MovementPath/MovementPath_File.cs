namespace BinarySerializer.KlonoaDTP
{
    public class MovementPath_File : BaseFile
    {
        public MovementPathBlock[] Blocks { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Blocks = s.SerializeObjectArray<MovementPathBlock>(Blocks, Pre_FileSize / 28, name: nameof(Blocks));
        }
    }
}