namespace BinarySerializer.KlonoaDTP
{
    public class Cutscene : BinarySerializable
    {
        public ArchiveFile Pre_Archive { get; set; }
        public int Pre_ArchiveFileIndex { get; set; }

        // TODO: Why are there two sets of instructions? Second one always seems smaller, maybe the version when you skip the cutscene?
        public Cutscene_File Cutscene_0 { get; set; }
        public Cutscene_File Cutscene_1 { get; set; }
        public CutsceneFont_File Font { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Cutscene_0 = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_0, Pre_ArchiveFileIndex + 0, name: $"{nameof(Cutscene_0)}");
            Cutscene_1 = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_1, Pre_ArchiveFileIndex + 1, name: $"{nameof(Cutscene_1)}");
            Font = Pre_Archive.SerializeFile<CutsceneFont_File>(s, Font, Pre_ArchiveFileIndex + 2, name: $"{nameof(Font)}");
        }
    }
}