namespace BinarySerializer.Klonoa.KH
{
    public class MenuPack_ArchiveFile : PF_ArchiveFile
    {
        public Graphics_File File_0 { get; set; }
        public ArchiveFile<Graphics_File> File_1 { get; set; }
        public ArchiveFile<Graphics_File> File_2 { get; set; }
        public MenuPack_BestiaryUI_ArchiveFile BestiaryUI { get; set; }
        public ArchiveFile<BestiaryEntry_Archive> Bestiary { get; set; }
        public TextCommands_File File_5 { get; set; }
        public Graphics_File File_6 { get; set; }
        public Graphics_File File_7 { get; set; }
        public MenuPack_Tutorials_ArchiveFile Tutorials { get; set; }
        public MenuPack_MamettHouse_ArchiveFile MamettHouse { get; set; }
        public MenuPack_MomettHouse_ArchiveFile MomettHouse { get; set; }
        public MenuPack_CutsceneViewer_ArchiveFile CutsceneViewer { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<Graphics_File>(s, File_0, 0, name: nameof(File_0));
            File_1 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_1, 1, name: nameof(File_1));
            File_2 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_2, 2, name: nameof(File_2));
            BestiaryUI = SerializeFile<MenuPack_BestiaryUI_ArchiveFile>(s, BestiaryUI, 3, name: nameof(BestiaryUI));
            Bestiary = SerializeFile<ArchiveFile<BestiaryEntry_Archive>>(s, Bestiary, 4, fileEncoder: new BytePairEncoder(), name: nameof(Bestiary));
            File_5 = SerializeFile<TextCommands_File>(s, File_5, 5, name: nameof(File_5));
            File_6 = SerializeFile<Graphics_File>(s, File_6, 6, name: nameof(File_6));
            File_7 = SerializeFile<Graphics_File>(s, File_7, 7, fileEncoder: new BytePairEncoder(), name: nameof(File_7));
            Tutorials = SerializeFile<MenuPack_Tutorials_ArchiveFile>(s, Tutorials, 8, name: nameof(Tutorials));
            MamettHouse = SerializeFile<MenuPack_MamettHouse_ArchiveFile>(s, MamettHouse, 9, fileEncoder: new BytePairEncoder(), name: nameof(MamettHouse));
            MomettHouse = SerializeFile<MenuPack_MomettHouse_ArchiveFile>(s, MomettHouse, 10, fileEncoder: new BytePairEncoder(), name: nameof(MomettHouse));
            CutsceneViewer = SerializeFile<MenuPack_CutsceneViewer_ArchiveFile>(s, CutsceneViewer, 11, fileEncoder: new BytePairEncoder(), name: nameof(CutsceneViewer));
        }
    }
}