namespace BinarySerializer.Klonoa.KH
{
    public class BestiaryEntry_Archive : PF_ArchiveFile
    {
        public RawData_File File_0 { get; set; }
        public RawData_File File_1 { get; set; }
        public TextCommands Name { get; set; }
        public Graphics_File EnemySprite { get; set; }
        public TextCommands Description { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile(s, File_0, 0, name: nameof(File_0));
            File_1 = SerializeFile(s, File_1, 1, name: nameof(File_1));
            Name = SerializeFile(s, Name, 2, name: nameof(Name));
            EnemySprite = SerializeFile(s, EnemySprite, 3, name: nameof(EnemySprite));
            Description = SerializeFile(s, Description, 4, name: nameof(Description));
        }
    }
}