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
        public ArchiveFile<MovementPath_File> Proto_MovementPaths { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Loader loader = Loader.GetLoader(s.Context);

            if (loader.GameVersion != KlonoaGameVersion.DTP_Prototype_19970717)
            {
                Cutscene_Normal = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_Normal, Pre_ArchiveFileIndex + 0, name: $"{nameof(Cutscene_Normal)}");
                Cutscene_Skip = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_Skip, Pre_ArchiveFileIndex + 1, name: $"{nameof(Cutscene_Skip)}");
                Font = Pre_Archive.SerializeFile<CutsceneFont_File>(s, Font, Pre_ArchiveFileIndex + 2, name: $"{nameof(Font)}");
            }
            else
            {
                bool parseNormalCutscene = true;
                bool parseSkipCutscene = true;
                bool parseFont = true;
                bool parseCamAnim = true;

                // The skip, font and cam anim files all point to the paths file here, so skip those
                if (loader.BINBlock == 21 && Pre_ArchiveFileIndex == 8)
                {
                    parseSkipCutscene = false;
                    parseFont = false;
                    parseCamAnim = false;
                }
                // Don't parse last cutscene in block 23 as it has garbage data (skip one is null)
                else if (loader.BINBlock == 23 && Pre_ArchiveFileIndex == 20)
                {
                    parseNormalCutscene = false;
                }

                if (parseNormalCutscene)
                    Cutscene_Normal = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_Normal, Pre_ArchiveFileIndex + 0, name: $"{nameof(Cutscene_Normal)}");
                
                if (parseSkipCutscene)
                    Cutscene_Skip = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_Skip, Pre_ArchiveFileIndex + 1, name: $"{nameof(Cutscene_Skip)}");
                
                if (parseFont)
                    Font = Pre_Archive.SerializeFile<CutsceneFont_File>(s, Font, Pre_ArchiveFileIndex + 2, name: $"{nameof(Font)}");

                if (parseCamAnim)
                    Proto_CameraAnimations = Pre_Archive.SerializeFile<CameraAnimations_File>(s, Proto_CameraAnimations, Pre_ArchiveFileIndex + 3, name: $"{nameof(Proto_CameraAnimations)}");
                
                Proto_MovementPaths = Pre_Archive.SerializeFile<ArchiveFile<MovementPath_File>>(s, Proto_MovementPaths, Pre_ArchiveFileIndex + 4, name: $"{nameof(Proto_MovementPaths)}");
            }
        }
    }
}