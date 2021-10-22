namespace BinarySerializer.Klonoa.LV {
    public class NowLoadingSprite_ArchiveFile : ArchiveFile {
        public RawData_File File_0; // ? (has a bunch of float values)
        public GSTextures_File Sprite;

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile(s, File_0, 0, name: nameof(File_0));
            Sprite = SerializeFile(s, Sprite, 1, name: nameof(Sprite));
        }
    }
}