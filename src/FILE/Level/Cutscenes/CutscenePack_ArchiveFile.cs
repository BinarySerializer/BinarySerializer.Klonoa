﻿namespace BinarySerializer.KlonoaDTP
{
    public class CutscenePack_ArchiveFile : ArchiveFile
    {
        // Normal cutscene
        public SpriteAnimations_File SpriteAnimations { get; set; }
        public Sprites_ArchiveFile Sprites { get; set; } // TODO: What is this?
        public ArchiveFile<CutsceneRawTextureData> File_2 { get; set; } // TODO: What is this? Seems to be Klonoa frames (and other characters?)
        public RawData_File CharacterNamesImgData { get; set; } // Raw image data, gets loaded at (0x3f4, 0x180, 0xc, 0x50)
        public RawData_ArchiveFile File_4 { get; set; } // TODO: Parse - seems to be raw image data
        public Cutscene[] Cutscenes { get; set; }

        // Transition cutscene
        public RawData_File Transition_File_0 { get; set; } // Raw image data of Klonoa frames
        public RawData_File Transition_File_1 { get; set; } // Raw image data of the door
        public RawData_File Transition_File_2 { get; set; } // Palette

        protected override void SerializeFiles(SerializerObject s)
        {
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
        }
    }
}