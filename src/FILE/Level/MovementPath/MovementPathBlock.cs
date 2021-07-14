namespace BinarySerializer.KlonoaDTP
{
    public class MovementPathBlock : BinarySerializable
    {
        // TODO: Parse
        public short[] Data { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Data = s.SerializeArray<short>(Data, 28 / 2, name: nameof(Data));
        }
    }
}