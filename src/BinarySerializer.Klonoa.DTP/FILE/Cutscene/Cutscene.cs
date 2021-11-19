namespace BinarySerializer.Klonoa.DTP
{
    public class Cutscene : BinarySerializable
    {
        public ArchiveFile Pre_Archive { get; set; }
        public int Pre_ArchiveFileIndex { get; set; }

        public Cutscene_File Cutscene_Normal { get; set; }
        public Cutscene_File Cutscene_Skip { get; set; } // The game switches to this when skipping the cutscene
        public CutsceneFont_File Font { get; set; }

        // In the final version these are all stored in the cutscene assets pack
        public CameraAnimations_File Proto_CameraAnimations { get; set; }
        public MovementPath_File Proto_MovementPaths { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Loader loader = Loader.GetLoader(s.Context);
            bool isProto = loader.GameVersion == KlonoaGameVersion.DTP_Prototype_19970717;

            // Don't parse last cutscene in block 23 for prototype as it has garbage data
            if (!(isProto && loader.BINBlock == 23 && Pre_ArchiveFileIndex == 20))
                Cutscene_Normal = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_Normal, Pre_ArchiveFileIndex + 0, name: $"{nameof(Cutscene_Normal)}");
            
            Cutscene_Skip = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_Skip, Pre_ArchiveFileIndex + 1, name: $"{nameof(Cutscene_Skip)}");
            Font = Pre_Archive.SerializeFile<CutsceneFont_File>(s, Font, Pre_ArchiveFileIndex + 2, name: $"{nameof(Font)}");

            if (Loader.GetLoader(s.Context).GameVersion == KlonoaGameVersion.DTP_Prototype_19970717)
            {
                Proto_CameraAnimations = Pre_Archive.SerializeFile<CameraAnimations_File>(s, Proto_CameraAnimations, Pre_ArchiveFileIndex + 3, name: $"{nameof(Proto_CameraAnimations)}");
                Proto_MovementPaths = Pre_Archive.SerializeFile<MovementPath_File>(s, Proto_MovementPaths, Pre_ArchiveFileIndex + 4, name: $"{nameof(Proto_MovementPaths)}");
            }
        }
    }
}