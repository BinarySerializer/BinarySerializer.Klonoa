namespace BinarySerializer.Klonoa.DTP
{
    public class CollisionTriangles_File : BaseFile
    {
        public bool Pre_HasCount { get; set; } = true;

        public int Count { get; set; }
        public CollisionTriangle[] CollisionTriangles { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            if (Pre_HasCount)
                Count = s.Serialize<int>(Count, name: nameof(Count));
            
            // The count doesn't always match for some reason...
            CollisionTriangles = s.SerializeObjectArray<CollisionTriangle>(CollisionTriangles, (Pre_FileSize - 4) / 28, name: nameof(CollisionTriangles));

            if (!Pre_HasCount)
                Count = CollisionTriangles.Length;
        }
    }
}