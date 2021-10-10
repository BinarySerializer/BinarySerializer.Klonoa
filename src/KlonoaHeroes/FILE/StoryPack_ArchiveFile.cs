namespace BinarySerializer.Klonoa.KH
{
    public class StoryPack_ArchiveFile : ArchiveFile
    {
        public StoryPack_File0_ArchiveFile Backgrounds { get; set; }
        public MM_KH_Compressed_ArchiveFile<Graphics_File> Vignettes { get; set; }
        public ArchiveFile<ArchiveFile<Graphics_File>> CharacterPortraits_0 { get; set; }
        public ArchiveFile<ArchiveFile<ArchiveFile<Cutscene_File>>> Cutscenes { get; set; }
        public ArchiveFile<Graphics_File> TextGraphics_0 { get; set; }
        public Graphics_File TextBox { get; set; }
        public MM_KH_Compressed_ArchiveFile<Graphics_File> TextGraphics_1 { get; set; }
        public MM_KH_Compressed_ArchiveFile<Graphics_File> FullscreenVignettes { get; set; }
        public Graphics_File TextBoxPrompt { get; set; }
        public ArchiveFile<ArchiveFile<Graphics_File>> CharacterPortraits_1 { get; set; }
        public ArchiveFile<RawData_ArchiveFile> File_10 { get; set; }
        public RawData_File File_11 { get; set; }
        public ArchiveFile<RawData_ArchiveFile> File_12 { get; set; }
        public ArchiveFile<Graphics_File> File_13 { get; set; }
        public RawData_ArchiveFile File_14 { get; set; }

        // Null
        public RawData_File File_15 { get; set; }
        public RawData_File File_16 { get; set; }
        public RawData_File File_17 { get; set; }
        public RawData_File File_18 { get; set; }
        public RawData_File File_19 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Backgrounds = SerializeFile<StoryPack_File0_ArchiveFile>(s, Backgrounds, 0, name: nameof(Backgrounds));
            Vignettes = SerializeFile<MM_KH_Compressed_ArchiveFile<Graphics_File>>(s, Vignettes, 1, name: nameof(Vignettes));
            CharacterPortraits_0 = SerializeFile<ArchiveFile<ArchiveFile<Graphics_File>>>(s, CharacterPortraits_0, 2, name: nameof(CharacterPortraits_0));
            Cutscenes = SerializeFile<ArchiveFile<ArchiveFile<ArchiveFile<Cutscene_File>>>>(s, Cutscenes, 3, name: nameof(Cutscenes));
            TextGraphics_0 = SerializeFile<ArchiveFile<Graphics_File>>(s, TextGraphics_0, 4, name: nameof(TextGraphics_0));
            TextBox = SerializeFile<Graphics_File>(s, TextBox, 5, name: nameof(TextBox));
            TextGraphics_1 = SerializeFile<MM_KH_Compressed_ArchiveFile<Graphics_File>>(s, TextGraphics_1, 6, name: nameof(TextGraphics_1));
            FullscreenVignettes = SerializeFile<MM_KH_Compressed_ArchiveFile<Graphics_File>>(s, FullscreenVignettes, 7, name: nameof(FullscreenVignettes));
            TextBoxPrompt = SerializeFile<Graphics_File>(s, TextBoxPrompt, 8, name: nameof(TextBoxPrompt));
            CharacterPortraits_1 = SerializeFile<ArchiveFile<ArchiveFile<Graphics_File>>>(s, CharacterPortraits_1, 9, name: nameof(CharacterPortraits_1));
            File_10 = SerializeFile<ArchiveFile<RawData_ArchiveFile>>(s, File_10, 10, name: nameof(File_10));
            File_11 = SerializeFile<RawData_File>(s, File_11, 11, name: nameof(File_11));
            File_12 = SerializeFile<ArchiveFile<RawData_ArchiveFile>>(s, File_12, 12, name: nameof(File_12));
            File_13 = SerializeFile<ArchiveFile<Graphics_File>>(s, File_13, 13, name: nameof(File_13));
            File_14 = SerializeFile<RawData_ArchiveFile>(s, File_14, 14, name: nameof(File_14));
            File_15 = SerializeFile<RawData_File>(s, File_15, 15, name: nameof(File_15));
            File_16 = SerializeFile<RawData_File>(s, File_16, 16, name: nameof(File_16));
            File_17 = SerializeFile<RawData_File>(s, File_17, 17, name: nameof(File_17));
            File_18 = SerializeFile<RawData_File>(s, File_18, 18, name: nameof(File_18));
            File_19 = SerializeFile<RawData_File>(s, File_19, 19, name: nameof(File_19));
        }
    }
}