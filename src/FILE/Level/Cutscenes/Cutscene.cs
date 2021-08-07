namespace BinarySerializer.KlonoaDTP
{
    public class Cutscene : BinarySerializable
    {
        public ArchiveFile Pre_Archive { get; set; }
        public int Pre_ArchiveFileIndex { get; set; }

        public Cutscene_File Cutscene_Normal { get; set; }
        public Cutscene_File Cutscene_Skip { get; set; } // The game switches to this when skipping the cutscene
        public CutsceneFont_File Font { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Cutscene_Normal = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_Normal, Pre_ArchiveFileIndex + 0, name: $"{nameof(Cutscene_Normal)}");
            Cutscene_Skip = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_Skip, Pre_ArchiveFileIndex + 1, name: $"{nameof(Cutscene_Skip)}");
            Font = Pre_Archive.SerializeFile<CutsceneFont_File>(s, Font, Pre_ArchiveFileIndex + 2, name: $"{nameof(Font)}");
        }
    }
}