namespace BinarySerializer.Klonoa.LV
{
    public class GIMPack_ArchiveFile : ArchiveFile
    {
        public GIMIDs_File GIM_IDs { get; set; }
        public ArchiveFile<GIM_File> GIM_Files { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            GIM_Files = SerializeFile(s, GIM_Files, 1, name: nameof(GIM_Files));
            GIM_IDs = SerializeFile(s, GIM_IDs, 0, onPreSerialize: x => x.Pre_Count = GIM_Files.Files.Length, name: nameof(GIM_IDs));
        }
    }
}