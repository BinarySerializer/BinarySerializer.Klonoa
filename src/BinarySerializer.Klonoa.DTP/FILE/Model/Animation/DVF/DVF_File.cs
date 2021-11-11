namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// Vertex animation frame
    /// </summary>
    public class DVF_File : BaseFile
    {
        public DVFEntry[] Entries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Entries = s.SerializeObjectArrayUntil(Entries, x => x.VerticesIndex == -1, () => new DVFEntry()
            {
                VerticesIndex = -1
            }, name: nameof(Entries));
        }
    }
}