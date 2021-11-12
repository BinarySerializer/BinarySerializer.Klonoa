using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// A sector of a level
    /// </summary>
    public class Sector_ArchiveFile : ArchiveFile
    {
        public PS1_TMD LevelModel { get; set; } // The 3D level model

        public LevelModelObjectSectorMap_File LevelModelObjectSectorMap { get; set; } // Defines which objects to render where
        public LevelCollisionSectorMap_File LevelCollisionSectorMap { get; set; } // Defines which collision to check for where

        public CollisionTriangles_File LevelCollisionTriangles { get; set; } // The level collision
        
        public ArchiveFile<MovementPath_File> MovementPaths { get; set; } // The movement paths in the level

        public UnknownModelObjectsData_File UnknownModelObjectsData { get; set; } // Light related?

        protected override void SerializeFiles(SerializerObject s)
        {
            LevelModel = SerializeFile<PS1_TMD>(s, LevelModel, 0, name: nameof(LevelModel));
            LevelModelObjectSectorMap = SerializeFile<LevelModelObjectSectorMap_File>(s, LevelModelObjectSectorMap, 1, name: nameof(LevelModelObjectSectorMap));
            LevelCollisionSectorMap = SerializeFile<LevelCollisionSectorMap_File>(s, LevelCollisionSectorMap, 2, name: nameof(LevelCollisionSectorMap));
            LevelCollisionTriangles = SerializeFile<CollisionTriangles_File>(s, LevelCollisionTriangles, 3, onPreSerialize: x => x.Pre_HasCount = false, name: nameof(LevelCollisionTriangles));
            MovementPaths = SerializeFile<ArchiveFile<MovementPath_File>>(s, MovementPaths, 4, name: nameof(MovementPaths));
            UnknownModelObjectsData = SerializeFile<UnknownModelObjectsData_File>(s, UnknownModelObjectsData, 5, onPreSerialize: x => x.Pre_ObjsCount = LevelModel.ObjectsCount, name: nameof(UnknownModelObjectsData));
        }
    }
}