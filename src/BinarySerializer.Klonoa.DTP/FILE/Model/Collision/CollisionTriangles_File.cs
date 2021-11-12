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
            {
                Count = s.Serialize<int>(Count, name: nameof(Count));

                long actualCount = (Pre_FileSize - 4) / 28;
                if (Count != actualCount)
                    s.LogWarning($"Collision count {Count} does not match count of data in file {actualCount}");
            }
            else
            {
                Count = (int)(Pre_FileSize / 28);
            }

            // The count doesn't always match for some reason...
            CollisionTriangles = s.SerializeObjectArray<CollisionTriangle>(CollisionTriangles, Count, name: nameof(CollisionTriangles));
        }
    }
}