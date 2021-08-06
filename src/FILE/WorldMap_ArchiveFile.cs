using BinarySerializer.PS1;

namespace BinarySerializer.KlonoaDTP
{
    public class WorldMap_ArchiveFile : ArchiveFile
    {
        public TIM_ArchiveFile SpriteSheets { get; set; }
        public RawData_ArchiveFile File_1 { get; set; } // TODO: Parse this - appears to only be used by a specific object?

        // Both contain a palette each, only one gets loaded at a time. 1 gets loaded if (((DAT_800cac56 == '\0') && (GlobalSectorIndex - 8 < 0x32))
        public PS1_TIM Palette1 { get; set; }
        public PS1_TIM Palette2 { get; set; }

        public AnimatedSprites_ArchiveFile AnimatedSprites { get; set; }
        public Sprites_ArchiveFile Proto_Sprites { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            SpriteSheets = SerializeFile<TIM_ArchiveFile>(s, SpriteSheets, 0, name: nameof(SpriteSheets));
            File_1 = SerializeFile<RawData_ArchiveFile>(s, File_1, 1, name: nameof(File_1));

            if (Loader.GetLoader(s.Context).Config.Version == LoaderConfiguration.GameVersion.DTP_Prototype_19970717)
            {
                Proto_Sprites = SerializeFile<Sprites_ArchiveFile>(s, Proto_Sprites, 2, name: nameof(Proto_Sprites));
            }
            else
            {
                Palette1 = SerializeFile<PS1_TIM>(s, Palette1, 2, name: nameof(Palette1));
                Palette2 = SerializeFile<PS1_TIM>(s, Palette2, 3, name: nameof(Palette2));
                AnimatedSprites = SerializeFile<AnimatedSprites_ArchiveFile>(s, AnimatedSprites, 4, name: nameof(AnimatedSprites));
            }
        }
    }
}