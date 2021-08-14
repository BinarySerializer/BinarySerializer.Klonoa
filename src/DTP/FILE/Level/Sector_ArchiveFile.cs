using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// A sector of a level
    /// </summary>
    public class Sector_ArchiveFile : ArchiveFile
    {
        public PS1_TMD LevelModel { get; set; }
        public LevelModelObjectMap_File LevelModelObjectMap { get; set; }
        public LevelCollision_File LevelCollision { get; set; }
        public LevelCollisionItems_File LevelCollisionItems { get; set; }
        public ArchiveFile<MovementPath_File> MovementPaths { get; set; }
        public UnknownModelObjectsData_File UnknownModelObjectsData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Every third level (every boss) is not compressed
            var isCompressed = !Loader.GetLoader(s.Context).IsBossFight;

            if (isCompressed)
            {
                var compresedSize = Pre_FileSize;

                s.DoEncoded(new LevelSectorEncoder(), () =>
                {
                    Pre_FileSize = s.CurrentLength;

                    var start = s.CurrentPointer;

                    // Serialize the offset table
                    OffsetTable = s.SerializeObject<OffsetTable>(OffsetTable, name: nameof(OffsetTable));

                    ParsedFiles = new (BinarySerializable, string)[OffsetTable.FilesCount];

                    if (AddToParsedArchiveFiles)
                        ParsedArchiveFiles[this] = new bool[OffsetTable.FilesCount];

                    // Serialize the files
                    SerializeFiles(s);

                    s.Goto(start + s.CurrentLength);
                }, allowLocalPointers: true);

                // Go to the end of the archive
                s.Goto(Offset + compresedSize);

                return;
            }

            base.SerializeImpl(s);
        }

        protected override void SerializeFiles(SerializerObject s)
        {
            LevelModel = SerializeFile<PS1_TMD>(s, LevelModel, 0, logIfNotFullyParsed: false, name: nameof(LevelModel));
            LevelModelObjectMap = SerializeFile<LevelModelObjectMap_File>(s, LevelModelObjectMap, 1, name: nameof(LevelModelObjectMap));
            LevelCollision = SerializeFile<LevelCollision_File>(s, LevelCollision, 2, name: nameof(LevelCollision));
            LevelCollisionItems = SerializeFile<LevelCollisionItems_File>(s, LevelCollisionItems, 3, name: nameof(LevelCollisionItems));
            MovementPaths = SerializeFile<ArchiveFile<MovementPath_File>>(s, MovementPaths, 4, name: nameof(MovementPaths));
            UnknownModelObjectsData = SerializeFile<UnknownModelObjectsData_File>(s, UnknownModelObjectsData, 5, onPreSerialize: x => x.Pre_ObjsCount = LevelModel.ObjectsCount, name: nameof(UnknownModelObjectsData));
        }
    }
}