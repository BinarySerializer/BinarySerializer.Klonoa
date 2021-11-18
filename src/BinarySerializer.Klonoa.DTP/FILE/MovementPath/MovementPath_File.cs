namespace BinarySerializer.Klonoa.DTP
{
    public class MovementPath_File : BaseFile
    {
        public MovementPathBlock[] Blocks { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            if (Pre_FileSize == 4)
                s.SerializePadding(4, logIfNotNull: true);
            else
                Blocks = s.SerializeObjectArray<MovementPathBlock>(Blocks, Pre_FileSize / 28, name: nameof(Blocks));
        }
    }
}