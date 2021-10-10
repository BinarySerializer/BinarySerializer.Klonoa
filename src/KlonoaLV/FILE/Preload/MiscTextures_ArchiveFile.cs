namespace BinarySerializer.Klonoa.LV
{
    public class MiscTextures_ArchiveFile : ArchiveFile
    {
        public RawData_File File_0 { get; set; }
        public ArchiveFile<GIM_File> GIM_Files { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile(s, File_0, 0, name: nameof(File_0));
            GIM_Files = SerializeFile(s, GIM_Files, 1, name: nameof(GIM_Files));
        }
    }
}