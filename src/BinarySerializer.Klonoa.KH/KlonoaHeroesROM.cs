using BinarySerializer.GBA;

namespace BinarySerializer.Klonoa.KH
{
    public class KlonoaHeroesROM : GBA_ROMBase
    {
        public MenuPack_ArchiveFile MenuPack { get; set; }
        public ArchiveFile<Animation_File> ObjectAnimationsPack { get; set; }
        public GameplayPack_ArchiveFile GameplayPack { get; set; }
        public ItemsPack_Archive ItemsPack { get; set; }
        public UIPack_ArchiveFile UIPack { get; set; }
        public StoryPack_ArchiveFile StoryPack { get; set; }
        public ArchiveFile<RawData_File> MapsPack { get; set; } // TODO: Parse maps

        public override void SerializeImpl(SerializerObject s)
        {
            base.SerializeImpl(s);

            // TODO: Move pointers to pointer table to avoid hard-coding them

            // TODO: Parse remaining data
            // 082f9870 - offset table with names

            s.DoAt(new Pointer(0x08267940, Offset.File), () => MenuPack = s.SerializeObject<MenuPack_ArchiveFile>(MenuPack, name: nameof(MenuPack)));
            s.DoAt(new Pointer(0x08399f10, Offset.File), () => ObjectAnimationsPack = s.SerializeObject<ArchiveFile<Animation_File>>(ObjectAnimationsPack, x => x.Pre_Type = ArchiveFileType.KH_TP, name: nameof(ObjectAnimationsPack)));
            s.DoAt(new Pointer(0x088fda70, Offset.File), () => GameplayPack = s.SerializeObject<GameplayPack_ArchiveFile>(GameplayPack, name: nameof(GameplayPack)));
            s.DoAt(new Pointer(0x0825c880, Offset.File), () => ItemsPack = s.SerializeObject<ItemsPack_Archive>(ItemsPack, name: nameof(ItemsPack)));
            s.DoAt(new Pointer(0x0828fd20, Offset.File), () => UIPack = s.SerializeObject<UIPack_ArchiveFile>(UIPack, name: nameof(UIPack)));
            s.DoAt(new Pointer(0x0873d350, Offset.File), () => StoryPack = s.SerializeObject<StoryPack_ArchiveFile>(StoryPack, name: nameof(StoryPack)));
            s.DoAt(new Pointer(0x08b30fd0, Offset.File), () => MapsPack = s.SerializeObject<ArchiveFile<RawData_File>>(MapsPack, x =>
            {
                x.Pre_Type = ArchiveFileType.KH_KW;
                x.Pre_ArchivedFilesEncoder = new BytePairEncoder();
            }, name: nameof(MapsPack)));
        }
    }
}