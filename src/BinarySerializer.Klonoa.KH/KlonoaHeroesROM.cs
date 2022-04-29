using System;
using BinarySerializer.Nintendo.GBA;

namespace BinarySerializer.Klonoa.KH
{
    public class KlonoaHeroesROM : ROMBase
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
        public WorldMapPack_ArchiveFile WorldMapPack { get; set; }

        public EnemyObjectTypeDefinition[] EnemyObjectDefinitions { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            base.SerializeImpl(s);

            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.MenuPack))
                s.DoAt(s.GetPreDefinedPointer(DefinedPointer.MenuPack), () => 
                    MenuPack = s.SerializeObject<MenuPack_ArchiveFile>(MenuPack, name: nameof(MenuPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.EnemyAnimationsPack))
                s.DoAt(s.GetPreDefinedPointer(DefinedPointer.EnemyAnimationsPack), () => 
                    EnemyAnimationsPack = s.SerializeObject<ArchiveFile<Animation_File>>(EnemyAnimationsPack, x => x.Pre_Type = ArchiveFileType.KH_TP, name: nameof(EnemyAnimationsPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.GameplayPack))
                s.DoAt(s.GetPreDefinedPointer(DefinedPointer.GameplayPack), () => 
                    GameplayPack = s.SerializeObject<GameplayPack_ArchiveFile>(GameplayPack, name: nameof(GameplayPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.ItemsPack))
                s.DoAt(s.GetPreDefinedPointer(DefinedPointer.ItemsPack), () => 
                    ItemsPack = s.SerializeObject<ItemsPack_Archive>(ItemsPack, name: nameof(ItemsPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.UIPack))
                s.DoAt(s.GetPreDefinedPointer(DefinedPointer.UIPack), () => 
                    UIPack = s.SerializeObject<UIPack_ArchiveFile>(UIPack, name: nameof(UIPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.StoryPack))
                s.DoAt(s.GetPreDefinedPointer(DefinedPointer.StoryPack), () => 
                    StoryPack = s.SerializeObject<StoryPack_ArchiveFile>(StoryPack, name: nameof(StoryPack)));
            
            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.MapsPack))
                s.DoAt(s.GetPreDefinedPointer(DefinedPointer.MapsPack), () => 
                    MapsPack = s.SerializeObject<MapsPack_ArchiveFile>(MapsPack, name: nameof(MapsPack)));

            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.WorldMapPack))
                s.DoAt(s.GetPreDefinedPointer(DefinedPointer.WorldMapPack), () =>
                    WorldMapPack = s.SerializeObject<WorldMapPack_ArchiveFile>(WorldMapPack, name: nameof(WorldMapPack)));

            if (Pre_SerializeFlags.HasFlag(SerializeDataFlags.EnemyObjectDefinitions))
                s.DoAt(s.GetPreDefinedPointer(DefinedPointer.EnemyObjectDefinitions), () => 
                    EnemyObjectDefinitions = s.SerializeObjectArray<EnemyObjectTypeDefinition>(EnemyObjectDefinitions, 182, name: nameof(EnemyObjectDefinitions)));
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
            WorldMapPack = 1 << 7,
            EnemyObjectDefinitions = 1 << 8,

            Packs = MenuPack | EnemyAnimationsPack | GameplayPack | ItemsPack | UIPack | StoryPack | MapsPack | WorldMapPack,
            CodeData = EnemyObjectDefinitions,
            All = Packs | CodeData,
        }
    }
}