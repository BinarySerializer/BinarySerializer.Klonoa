namespace BinarySerializer.Klonoa.DTP
{
    public class CollisionTriangles_File : BaseFile
    {
        public CollisionTriangle[] CollisionTriangles { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            CollisionTriangles = s.SerializeObjectArray<CollisionTriangle>(CollisionTriangles, Pre_FileSize / 28, name: nameof(CollisionTriangles));
        }
    }
}