namespace BinarySerializer.KlonoaDTP
{
    public class CutscenePack_ArchiveFile : ArchiveFile
    {
        public SpriteAnimations_File SpriteAnimations { get; set; }
        public Sprites_ArchiveFile Sprites { get; set; } // TODO: What is this?
        public ArchiveFile<CutsceneRawTextureData> File_2 { get; set; } // TODO: What is this? Seems to be Klonoa frames (and other characters?)
        public RawData_File CharacterNamesImgData { get; set; } // Raw image data, gets loaded at (0x3f4, 0x180, 0xc, 0x50)
        public RawData_ArchiveFile File_4 { get; set; } // TODO: Parse
        public Cutscene[] Cutscenes { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            SpriteAnimations = SerializeFile<SpriteAnimations_File>(s, SpriteAnimations, 0, name: nameof(SpriteAnimations));
            Sprites = SerializeFile<Sprites_ArchiveFile>(s, Sprites, 1, name: nameof(Sprites));
            File_2 = SerializeFile<ArchiveFile<CutsceneRawTextureData>>(s, File_2, 2, name: nameof(File_2));
            CharacterNamesImgData = SerializeFile<RawData_File>(s, CharacterNamesImgData, 3, name: nameof(CharacterNamesImgData));
            File_4 = SerializeFile<RawData_ArchiveFile>(s, File_4, 4, name: nameof(File_4));

            var count = (OffsetTable.FilesCount - 5) / 3;

            Cutscenes ??= new Cutscene[count];

            for (int i = 0; i < count; i++)
            {
                var fileIndex = 5 + i * 3;

                // Goto to avoid caching
                s.Goto(OffsetTable.FilePointers[fileIndex]);

                Cutscenes[i] = s.SerializeObject<Cutscene>(Cutscenes[i], x =>
                {
                    x.Pre_Archive = this;
                    x.Pre_ArchiveFileIndex = fileIndex;
                }, name: $"{nameof(Cutscenes)}[{i}]");
            }
        }

        public class Cutscene : BinarySerializable
        {
            public ArchiveFile Pre_Archive { get; set; }
            public int Pre_ArchiveFileIndex { get; set; }

            // TODO: Why are there two sets of instructions? Second one always seems smaller, maybe the version when you skip the cutscene?
            public Cutscene_File Cutscene_0 { get; set; }
            public Cutscene_File Cutscene_1 { get; set; }
            public RawData_File File_2 { get; set; } // TODO: Parse - if this is null the font is not loaded, so text related?

            public override void SerializeImpl(SerializerObject s)
            {
                Cutscene_0 = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_0, Pre_ArchiveFileIndex + 0, name: $"{nameof(Cutscene_0)}");
                Cutscene_1 = Pre_Archive.SerializeFile<Cutscene_File>(s, Cutscene_1, Pre_ArchiveFileIndex + 1, name: $"{nameof(Cutscene_1)}");
                File_2 = Pre_Archive.SerializeFile<RawData_File>(s, File_2, Pre_ArchiveFileIndex + 2, name: $"{nameof(File_2)}");
            }
        }
    }
}