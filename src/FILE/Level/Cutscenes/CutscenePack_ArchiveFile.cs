namespace BinarySerializer.KlonoaDTP
{
    public class CutscenePack_ArchiveFile : ArchiveFile
    {
        // Normal cutscene
        public SpriteAnimations_File SpriteAnimations { get; set; }
        public Sprites_ArchiveFile Sprites { get; set; } // Cutscene sprites
        public ArchiveFile<CutscenePlayerSprite_File> PlayerFramesImgData { get; set; } // Klonoa sprites
        public RawData_File CharacterNamesImgData { get; set; } // Raw image data, gets loaded at (0x3f4, 0x180, 0xc, 0x50)
        public RawData_ArchiveFile CutsceneAssets { get; set; } // Used by hard-coded functions for each cutscene. Contains obj models etc.
        public Cutscene[] Cutscenes { get; set; }

        // Transition cutscene
        public RawData_File Transition_File_0 { get; set; } // Raw image data of Klonoa frames
        public RawData_File Transition_File_1 { get; set; } // Raw image data of the door
        public RawData_File Transition_File_2 { get; set; } // Palette

        protected override void SerializeFiles(SerializerObject s)
        {
            if (OffsetTable.FilesCount == 0)
                return;

            if (OffsetTable.FilesCount == 3)
            {
                Transition_File_0 = SerializeFile<RawData_File>(s, Transition_File_0, 0, name: nameof(Transition_File_0));
                Transition_File_1 = SerializeFile<RawData_File>(s, Transition_File_1, 1, name: nameof(Transition_File_1));
                Transition_File_2 = SerializeFile<RawData_File>(s, Transition_File_2, 2, name: nameof(Transition_File_2));
            }
            else
            {
                SpriteAnimations = SerializeFile<SpriteAnimations_File>(s, SpriteAnimations, 0, name: nameof(SpriteAnimations));
                Sprites = SerializeFile<Sprites_ArchiveFile>(s, Sprites, 1, name: nameof(Sprites));
                PlayerFramesImgData = SerializeFile<ArchiveFile<CutscenePlayerSprite_File>>(s, PlayerFramesImgData, 2, name: nameof(PlayerFramesImgData));
                CharacterNamesImgData = SerializeFile<RawData_File>(s, CharacterNamesImgData, 3, name: nameof(CharacterNamesImgData));
                CutsceneAssets = SerializeFile<RawData_ArchiveFile>(s, CutsceneAssets, 4, name: nameof(CutsceneAssets));

                var cutsceneFilesCount = Loader.GetLoader(s.Context).Config.Version == LoaderConfiguration.GameVersion.DTP_Prototype_19970717 ? 5 : 3;

                if ((OffsetTable.FilesCount - 5) % cutsceneFilesCount != 0)
                {
                    s.LogWarning($"Cutscene pack is invalid. Files count is {OffsetTable.FilesCount}");
                    return;
                }

                var count = (OffsetTable.FilesCount - 5) / cutsceneFilesCount;

                Cutscenes ??= new Cutscene[count];

                for (int i = 0; i < count; i++)
                {
                    var fileIndex = 5 + i * cutsceneFilesCount;

                    // Goto to avoid caching
                    s.Goto(OffsetTable.FilePointers[fileIndex]);

                    Cutscenes[i] = s.SerializeObject<Cutscene>(Cutscenes[i], x =>
                    {
                        x.Pre_Archive = this;
                        x.Pre_ArchiveFileIndex = fileIndex;
                    }, name: $"{nameof(Cutscenes)}[{i}]");
                }
            }
        }
    }
}