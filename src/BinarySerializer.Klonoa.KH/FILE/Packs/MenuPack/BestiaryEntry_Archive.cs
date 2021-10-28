namespace BinarySerializer.Klonoa.KH
{
    public class BestiaryEntry_Archive : PF_ArchiveFile
    {
        public RawData_File File_0 { get; set; }
        public RawData_File File_1 { get; set; }
        public TextCommands_File Name { get; set; }
        public Graphics_File EnemySprite { get; set; }
        public TextCommands_File Description { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<RawData_File>(s, File_0, 0, name: nameof(File_0));
            File_1 = SerializeFile<RawData_File>(s, File_1, 1, name: nameof(File_1));
            Name = SerializeFile<TextCommands_File>(s, Name, 2, name: nameof(Name));
            EnemySprite = SerializeFile<Graphics_File>(s, EnemySprite, 3, name: nameof(EnemySprite));
            Description = SerializeFile<TextCommands_File>(s, Description, 4, name: nameof(Description));
        }
    }
}