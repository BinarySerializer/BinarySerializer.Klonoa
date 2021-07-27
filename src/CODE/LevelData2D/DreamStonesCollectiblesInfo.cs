namespace BinarySerializer.KlonoaDTP
{
    public class DreamStonesCollectiblesInfo : BinarySerializable
    {
        public ushort Index { get; set; }
        public ushort DreamStonesCount { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Index = s.Serialize<ushort>(Index, name: nameof(Index));
            DreamStonesCount = s.Serialize<ushort>(DreamStonesCount, name: nameof(DreamStonesCount));
        }
    }
}