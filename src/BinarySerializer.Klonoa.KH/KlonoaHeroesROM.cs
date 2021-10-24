using System;
using BinarySerializer.GBA;

namespace BinarySerializer.Klonoa.KH
{
    public class KlonoaHeroesROM : GBA_ROMBase
    {
        /// <summary>
        /// Optional flags for determining which data to serialize from the ROM. Defaults to <see cref="SerializeDataFlags.All"/>
        /// </summary>
        public SerializeDataFlags Pre_SerializeFlags { get; set; } = SerializeDataFlags.All;

        public MenuPack_ArchiveFile MenuPack { get; set; }
        public ArchiveFile<Animation_File> EnemyAnimationsPack { get; set; }
        public GameplayPack_ArchiveFile GameplayPack { get; set; }
        public ItemsPack_Archive ItemsPack { get; set; }
        public UIPack_ArchiveFile UIPack { get; set; }
        public StoryPack_ArchiveFile StoryPack { get; set; }
        public MapsPack_ArchiveFile MapsPack { get; set; }

        public EnemyObjectTypeDefinition[] EnemyObjectDefinitions { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            base.SerializeImpl(s);

            // TODO: Move pointers to pointer table to avoid hard-coding them

            // TODO: Parse remaining data
            // 082f9870 - offset table with names

            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.MenuPack))
                s.DoAt(new Pointer(0x08267940, Offset.File), () => MenuPack = s.SerializeObject<MenuPack_ArchiveFile>(MenuPack, name: nameof(MenuPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.EnemyAnimationsPack))
                s.DoAt(new Pointer(0x08399f10, Offset.File), () => EnemyAnimationsPack = s.SerializeObject<ArchiveFile<Animation_File>>(EnemyAnimationsPack, x => x.Pre_Type = ArchiveFileType.KH_TP, name: nameof(EnemyAnimationsPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.GameplayPack))
                s.DoAt(new Pointer(0x088fda70, Offset.File), () => GameplayPack = s.SerializeObject<GameplayPack_ArchiveFile>(GameplayPack, name: nameof(GameplayPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.ItemsPack))
                s.DoAt(new Pointer(0x0825c880, Offset.File), () => ItemsPack = s.SerializeObject<ItemsPack_Archive>(ItemsPack, name: nameof(ItemsPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.UIPack))
                s.DoAt(new Pointer(0x0828fd20, Offset.File), () => UIPack = s.SerializeObject<UIPack_ArchiveFile>(UIPack, name: nameof(UIPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.StoryPack))
                s.DoAt(new Pointer(0x0873d350, Offset.File), () => StoryPack = s.SerializeObject<StoryPack_ArchiveFile>(StoryPack, name: nameof(StoryPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.MapsPack))
                s.DoAt(new Pointer(0x08b30fd0, Offset.File), () => MapsPack = s.SerializeObject<MapsPack_ArchiveFile>(MapsPack, name: nameof(MapsPack)));

            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.EnemyObjectDefinitions))
                s.DoAt(new Pointer(0x08068720, Offset.File), () => EnemyObjectDefinitions = s.SerializeObjectArray<EnemyObjectTypeDefinition>(EnemyObjectDefinitions, 182, name: nameof(EnemyObjectDefinitions)));
        }

        [Flags]
        public enum SerializeDataFlags
        {
            None = 0,

            MenuPack = 1 << 0,
            EnemyAnimationsPack = 1 << 1,
            GameplayPack = 1 << 2,
            ItemsPack = 1 << 3,
            UIPack = 1 << 4,
            StoryPack = 1 << 5,
            MapsPack = 1 << 6,
            EnemyObjectDefinitions = 1 << 7,

            Packs = MenuPack | EnemyAnimationsPack | GameplayPack | ItemsPack | UIPack | StoryPack | MapsPack,
            CodeData = EnemyObjectDefinitions,
            All = Packs | CodeData,
        }
    }
}