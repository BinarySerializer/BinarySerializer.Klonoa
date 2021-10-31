namespace BinarySerializer.Klonoa.DTP
{
    public class ObjCollision_File : BaseFile
    {
        public uint Count { get; set; }
        public CollisionTriangle[] CollisionTriangles { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Count = s.Serialize<uint>(Count, name: nameof(Count));
            // The count doesn't always match for some reason...
            CollisionTriangles = s.SerializeObjectArray<CollisionTriangle>(CollisionTriangles, (Pre_FileSize - 4) / 28, name: nameof(CollisionTriangles));
        }
    }
}