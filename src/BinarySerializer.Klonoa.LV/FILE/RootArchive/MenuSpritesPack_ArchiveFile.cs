namespace BinarySerializer.Klonoa.LV {
    public class MenuSpritesPack_ArchiveFile : ArchiveFile {
        public NowLoadingSprite_ArchiveFile NowLoadingSprite;
        public RawData_File File_1; // ?
        public RawData_File Boundary_0;
        public RawData_File Boundary_1;
        public RawData_File Boundary_2;
        public RawData_File Boundary_3;
        public RawData_File Boundary_4;
        public GMS_File MenuSprites;

        protected override void SerializeFiles(SerializerObject s)
        {
            NowLoadingSprite = SerializeFile(s, NowLoadingSprite, 0, name: nameof(NowLoadingSprite));
            File_1 = SerializeFile(s, File_1, 1, name: nameof(File_1));
            Boundary_0 = SerializeFile(s, Boundary_0, 2, name: nameof(Boundary_0));
            Boundary_1 = SerializeFile(s, Boundary_1, 3, name: nameof(Boundary_1));
            Boundary_2 = SerializeFile(s, Boundary_2, 4, name: nameof(Boundary_2));
            Boundary_3 = SerializeFile(s, Boundary_3, 5, name: nameof(Boundary_3));
            Boundary_4 = SerializeFile(s, Boundary_4, 6, name: nameof(Boundary_4));
            MenuSprites = SerializeFile(s, MenuSprites, 7, name: nameof(MenuSprites));
        }
    }
}