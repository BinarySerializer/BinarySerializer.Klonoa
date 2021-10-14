namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// Series of triangle meshes sharing vertices. Winding must be calculated using normals.
    /// </summary>
    public class ModelTriangleStrip : BinarySerializable
    {
        public int IndicesCount { get; set; }
        public TriangleStripIndex[] Indices { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            int i = 0;
            Indices = s.SerializeObjectArrayUntil<TriangleStripIndex>(
                obj: Indices,
                name: nameof(Indices),
                conditionCheckFunc: x =>
                {
                    // The first index defines how many indices are in the triangle strip
                    if (i == 0)
                    {
                        if (!x.StartTriangleStrip)
                            throw new BinarySerializableException(this, $"Invalid triangle strip size: {x.Extra}");
                        IndicesCount = x.Extra;
                    }
                    
                    // Stop once all indices in the triangle strip have been parsed
                    return ++i == IndicesCount;
                }
            );
        }
    }
}