namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// A sector of a level
    /// </summary>
    public class LevelSector_ArchiveFile : ArchiveFile
    {
        public GSTextures_File Textures { get; set; } // Compressed GSTextures
        public VIFGeometry_File Geometry { get; set; } // Compressed VIF codes
        public RouteData_File RouteData { get; set; }
        public RawData_File CollisionData { get; set; }
        public CameraData_File CameraData { get; set; }
        public LightData_File LightData { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            LevelSectorEncoder encoder = new LevelSectorEncoder();

            Textures = SerializeFile(s, Textures, 0, fileEncoder: encoder, logIfNotFullyParsed: false, name: nameof(Textures));
            Geometry = SerializeFile(s, Geometry, 1, fileEncoder: encoder, logIfNotFullyParsed: false, name: nameof(Geometry)); // Level 40_5's geometry does not decompress fully
            RouteData = SerializeFile(s, RouteData, 2, name: nameof(RouteData));
            CollisionData = SerializeFile(s, CollisionData, 3, fileEncoder: encoder, logIfNotFullyParsed: false, name: nameof(CollisionData));
            CameraData = SerializeFile(s, CameraData, 4, name: nameof(CameraData));
            LightData = SerializeFile(s, LightData, 5, name: nameof(LightData));
        }
    }
}