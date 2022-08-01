namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// A sector of a level
    /// </summary>
    public class LevelSector_ArchiveFile : ArchiveFile
    {
        public GSTextures_File LevelTextures { get; set; } // Compressed GSTextures
        public VIFGeometry_File LevelGeometry { get; set; } // Compressed VIF codes
        public RawData_File LevelObjectData { get; set; } // General data about objects in the level sector (actors, lights, etc)
        public RawData_File LevelCollisionData { get; set; } // TODO: Verify that this is in fact collision data
        public RawData_File LevelMovementPaths { get; set; } // TODO: Verify this as well
        public RawData_File LevelLightData { get; set; } // Data about the lights themselves, such as color

        protected override void SerializeFiles(SerializerObject s)
        {
            LevelSectorEncoder encoder = new LevelSectorEncoder();

            LevelTextures = SerializeFile(s, LevelTextures, 0, fileEncoder: encoder, name: nameof(LevelTextures));
            LevelGeometry = SerializeFile(s, LevelGeometry, 1, fileEncoder: encoder, name: nameof(LevelGeometry));
            LevelObjectData = SerializeFile(s, LevelObjectData, 2, name: nameof(LevelObjectData));
            LevelCollisionData = SerializeFile(s, LevelCollisionData, 3, fileEncoder: encoder, name: nameof(LevelCollisionData));
            LevelMovementPaths = SerializeFile(s, LevelMovementPaths, 4, name: nameof(LevelMovementPaths));
            LevelLightData = SerializeFile(s, LevelLightData, 5, name: nameof(LevelLightData));
        }
    }
}