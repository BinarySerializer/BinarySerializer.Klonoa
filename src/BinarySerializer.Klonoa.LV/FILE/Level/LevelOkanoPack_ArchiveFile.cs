namespace BinarySerializer.Klonoa.LV
{
    public class LevelOkanoPack_ArchiveFile : ArchiveFile
    {
        public GSTextures_File DreamstoneTexture { get; set; }
        public GSTextures_File LargeDreamstoneTexture { get; set; }
        public VIFGeometry_File DreamstoneGeometry { get; set; }
        public VIFGeometry_File LargeDreamstoneGeometry { get; set; }

        // Collision, TODO: find out what these names are supposed to mean
        public RawData_File MyuboCollision { get; set; }
        public RawData_File GmooCollision { get; set; }
        public RawData_File Hako3Collision { get; set; }
        public RawData_File Yuka01Collision { get; set; }
        public RawData_File Yuka02Collision { get; set; }
        public RawData_File DoramCollision { get; set; }
        public RawData_File OkuidoyukaCollision { get; set; }
        public RawData_File Map0636Collision { get; set; }
        public RawData_File Map0635Collision { get; set; }
        public RawData_File Oku5mCollision { get; set; }
        public RawData_File PlaneCollision { get; set; }
        public RawData_File KaitenBCollision { get; set; }
        public RawData_File Map0631Collision { get; set; }

        // Routes
        public RouteData_File Map0636Route { get; set; }
        public RouteData_File Map0635Route { get; set; }
        public RouteData_File Oku5mRoute { get; set; }
        public RouteData_File PlaneRoute { get; set; }
        public RouteData_File KanranRoute { get; set; }
        public RouteData_File Map0631Route { get; set; }

        // Zako IDs
        public ZakoIDs_File ZakoIDs { get; set; }

        // Enemy/item data for each vision
        public RawData_File[] Zak { get; set; }
        public RawData_File[] Items { get; set; }
        public RouteData_File[] ZakoRoutes { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            DreamstoneTexture = SerializeFile(s, DreamstoneTexture, 0, name: nameof(DreamstoneTexture));
            LargeDreamstoneTexture = SerializeFile(s, LargeDreamstoneTexture, 1, name: nameof(LargeDreamstoneTexture));
            DreamstoneGeometry = SerializeFile(s, DreamstoneGeometry, 2, name: nameof(DreamstoneGeometry));
            LargeDreamstoneGeometry = SerializeFile(s, LargeDreamstoneGeometry, 3, name: nameof(LargeDreamstoneGeometry));
            
            MyuboCollision = SerializeFile(s, MyuboCollision, 4, name: nameof(MyuboCollision));
            GmooCollision = SerializeFile(s, GmooCollision, 5, name: nameof(GmooCollision));
            Hako3Collision = SerializeFile(s, Hako3Collision, 6, name: nameof(Hako3Collision));
            Yuka01Collision = SerializeFile(s, Yuka01Collision, 7, name: nameof(Yuka01Collision));
            Yuka02Collision = SerializeFile(s, Yuka02Collision, 8, name: nameof(Yuka02Collision));
            DoramCollision = SerializeFile(s, DoramCollision, 9, name: nameof(DoramCollision));
            OkuidoyukaCollision = SerializeFile(s, OkuidoyukaCollision, 10, name: nameof(OkuidoyukaCollision));
            Map0636Collision = SerializeFile(s, Map0636Collision, 11, name: nameof(Map0636Collision));
            Map0635Collision = SerializeFile(s, Map0635Collision, 12, name: nameof(Map0635Collision));
            Oku5mCollision = SerializeFile(s, Oku5mCollision, 13, name: nameof(Oku5mCollision));
            PlaneCollision = SerializeFile(s, PlaneCollision, 14, name: nameof(PlaneCollision));
            KaitenBCollision = SerializeFile(s, KaitenBCollision, 15, name: nameof(KaitenBCollision));
            Map0631Collision = SerializeFile(s, Map0631Collision, 16, name: nameof(Map0631Collision));

            Map0636Route = SerializeFile(s, Map0636Route, 17, name: nameof(Map0636Route));
            Map0635Route = SerializeFile(s, Map0635Route, 18, name: nameof(Map0635Route));
            Oku5mRoute = SerializeFile(s, Oku5mRoute, 19, name: nameof(Oku5mRoute));
            PlaneRoute = SerializeFile(s, PlaneRoute, 20, name: nameof(PlaneRoute));
            KanranRoute = SerializeFile(s, KanranRoute, 21, name: nameof(KanranRoute));
            Map0631Route = SerializeFile(s, Map0631Route, 22, name: nameof(Map0631Route));

            ZakoIDs = SerializeFile(s, ZakoIDs, 23, name: nameof(ZakoIDs));
            
            int visionCount = (OffsetTable.FilesCount - 24) / 3;
            Zak ??= new RawData_File[visionCount];
            Items ??= new RawData_File[visionCount];
            ZakoRoutes ??= new RouteData_File[visionCount];
            for (int i = 0; i < visionCount; i++) {
                Zak[i] = SerializeFile(s, Zak[i], 24 + i * 3, name: $"{nameof(Zak)}[{i}]");
                Items[i] = SerializeFile(s, Items[i], 24 + i * 3 + 1, name: $"{nameof(Items)}[{i}]");
                ZakoRoutes[i] = SerializeFile(s, ZakoRoutes[i], 24 + i * 3 + 2, name: $"{nameof(ZakoRoutes)}[{i}]");
            }
        }
    }
}