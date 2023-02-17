namespace BinarySerializer.Klonoa.LV
{
    public class LevelHaradaPack_ArchiveFile : ArchiveFile
    {
        public ArchiveFile<LevelScriptPack_ArchiveFile> ScriptData { get; set; } // Seems mainly like script data for each level section, but looks like there may be a bit of geometry in here as well
        public GSTextures_File DialogueBoxTexture { get; set; }
        public BackgroundTexturesArchive_ArchiveFile BackgroundTextures { get; set; }
        public ArchiveFile<LevelSectorConfig_File> SectorConfigs { get; set; }
        public RawData_File File_4 { get; set; }
        public GSTextures_File DialogueFont { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            ScriptData = SerializeFile(s, ScriptData, 0, name: nameof(ScriptData));
            DialogueBoxTexture = SerializeFile(s, DialogueBoxTexture, 1, name: nameof(DialogueBoxTexture));
            BackgroundTextures = SerializeFile(s, BackgroundTextures, 2, name: nameof(BackgroundTextures));
            SectorConfigs = SerializeFile(s, SectorConfigs, 3, name: nameof(SectorConfigs));
            File_4 = SerializeFile(s, File_4, 4, name: nameof(File_4));
            DialogueFont = SerializeFile(s, DialogueFont, 5, name: nameof(DialogueFont));
        }
    }
}