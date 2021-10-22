namespace BinarySerializer.Klonoa.KH
{
    public class ItemsPack_Archive : ArchiveFile
    {
        public ItemsCollection_File Weapons { get; set; }
        public ItemsCollection_File Armor { get; set; }
        public ItemsCollection_File Accessories { get; set; }
        public ItemsCollection_File ConsumableItems { get; set; }
        public ItemsCollection_File KeyItems { get; set; }
        public ArchiveFile<RawData_File> File_5 { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Weapons = SerializeFile<ItemsCollection_File>(s, Weapons, 0, name: nameof(Weapons));
            Armor = SerializeFile<ItemsCollection_File>(s, Armor, 1, name: nameof(Armor));
            Accessories = SerializeFile<ItemsCollection_File>(s, Accessories, 2, name: nameof(Accessories));
            ConsumableItems = SerializeFile<ItemsCollection_File>(s, ConsumableItems, 3, name: nameof(ConsumableItems));
            KeyItems = SerializeFile<ItemsCollection_File>(s, KeyItems, 4, onPreSerialize: x => x.Pre_AdditionalCount = 4, name: nameof(KeyItems));
            File_5 = SerializeFile<ArchiveFile<RawData_File>>(s, File_5, 5, name: nameof(File_5));
        }
    }
}