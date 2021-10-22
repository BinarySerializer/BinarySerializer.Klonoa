namespace BinarySerializer.Klonoa.LV {
    public class SkyTexture_ArchiveFile : ArchiveFile {
        public GSTextures_File SkyTexture;
        public RawData_File File_1; // dummy16
        public RawData_File File_2; // dummy16

        protected override void SerializeFiles(SerializerObject s)
        {
            SkyTexture = SerializeFile(s, SkyTexture, 0, name: nameof(SkyTexture));
            File_1 = SerializeFile(s, File_1, 1, name: nameof(File_1));
            File_2 = SerializeFile(s, File_2, 2, name: nameof(File_2));
        }
    }
}