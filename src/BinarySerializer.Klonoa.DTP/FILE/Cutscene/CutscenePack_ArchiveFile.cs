namespace BinarySerializer.Klonoa.DTP
{
    public class CutscenePack_ArchiveFile : ArchiveFile
    {
        // Normal cutscene
        public SpriteAnimations_File SpriteAnimations { get; set; }
        public Sprites_ArchiveFile Sprites { get; set; } // Cutscene sprites
        public ArchiveFile<CutscenePlayerSprite_File> PlayerFramesImgData { get; set; } // Klonoa sprites
        public RawData_File CharacterNamesImgData { get; set; } // Raw image data, gets loaded at (0x3f4, 0x180, 0xc, 0x50)
        public ArchiveFile CutsceneAssets { get; set; } // Used by hard-coded functions for each cutscene. Contains obj models etc.
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
                Loader loader = Loader.GetLoader(s.Context);
                KlonoaGameVersion version = loader.GameVersion;
                bool isJulyProto = version == KlonoaGameVersion.DTP_Prototype_19970717;
                int cutsceneFilesCount = isJulyProto ? 5 : 3;
                int cutscenesStartFile = 5;

                SpriteAnimations = SerializeFile<SpriteAnimations_File>(s, SpriteAnimations, 0, name: nameof(SpriteAnimations));
                Sprites = SerializeFile<Sprites_ArchiveFile>(s, Sprites, 1, name: nameof(Sprites));

                if (!(isJulyProto && loader.BINBlock == 21))
                {
                    PlayerFramesImgData = SerializeFile<ArchiveFile<CutscenePlayerSprite_File>>(s, PlayerFramesImgData, 2, name: nameof(PlayerFramesImgData));
                    CharacterNamesImgData = SerializeFile<RawData_File>(s, CharacterNamesImgData, 3, name: nameof(CharacterNamesImgData));
                    CutsceneAssets = SerializeFile<ArchiveFile>(s, CutsceneAssets, 4, name: nameof(CutsceneAssets));
                }
                else
                {
                    CutsceneAssets = SerializeFile<ArchiveFile>(s, CutsceneAssets, 2, name: nameof(CutsceneAssets));

                    // The cutscene files are packed incorrectly here, so shift back by 2
                    cutscenesStartFile = 3;
                }


                // Make sure the files count if valid
                if ((OffsetTable.FilesCount - cutscenesStartFile) % cutsceneFilesCount != 0)
                {
                    s.SystemLog?.LogWarning($"Cutscene pack is invalid. Files count is {OffsetTable.FilesCount}");
                    return;
                }

                int count = (OffsetTable.FilesCount - cutscenesStartFile) / cutsceneFilesCount;

                Cutscenes ??= new Cutscene[count];

                for (int i = 0; i < count; i++)
                {
                    var fileIndex = cutscenesStartFile + i * cutsceneFilesCount;

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