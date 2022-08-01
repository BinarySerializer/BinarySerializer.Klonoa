namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// A sector of a level
    /// </summary>
    public class LevelSector_ArchiveFile : ArchiveFile
    {
        public GSTextures_File Textures { get; set; } // Compressed GSTextures
        public VIFGeometry_File Geometry { get; set; } // Compressed VIF codes
        public RawData_File ObjectData { get; set; } // General data about objects in the level sector (actors, lights, etc)
        public RawData_File CollisionData { get; set; } // TODO: Verify that this is in fact collision data
        public CameraData_File CameraData { get; set; }
        public RawData_File LightData { get; set; } // Data about the lights themselves, such as color

        protected override void SerializeFiles(SerializerObject s)
        {
            LevelSectorEncoder encoder = new LevelSectorEncoder();

            Textures = SerializeFile(s, Textures, 0, fileEncoder: encoder, name: nameof(Textures));
            Geometry = SerializeFile(s, Geometry, 1, fileEncoder: encoder, name: nameof(Geometry));
            ObjectData = SerializeFile(s, ObjectData, 2, name: nameof(ObjectData));
            CollisionData = SerializeFile(s, CollisionData, 3, fileEncoder: encoder, name: nameof(CollisionData));
            CameraData = SerializeFile(s, CameraData, 4, name: nameof(CameraData));
            LightData = SerializeFile(s, LightData, 5, name: nameof(LightData));
        }
    }
}