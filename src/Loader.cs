using System;
using BinarySerializer.PS1;

namespace BinarySerializer.KlonoaDTP
{
    /// <summary>
    /// Handles loading data from the game BIN
    /// </summary>
    public class Loader
    {
        #region Constructor

        protected Loader(Context context)
        {
            Context = context;
            VRAM = new PS1_VRAM();
            SpriteFrames = new Sprites_ArchiveFile[70];
        }

        #endregion

        #region Constants

        private const string Key = "KLONOA_LOADER";

        public const string FilePath_BIN = "FILE.BIN";
        public const string FilePath_IDX = "FILE.IDX";

        #endregion

        #region Public Properties

        // The currently loaded data
        public PS1_VRAM VRAM { get; }
        public Sprites_ArchiveFile[] SpriteFrames { get; }
        public BackgroundPack_ArchiveFile BackgroundPack { get; set; }
        public OA05_File OA05 { get; set; }
        public LevelPack_ArchiveFile LevelPack { get; set; }

        // Properties
        public Context Context { get; }
        public int BINBlock { get; protected set; }
        public bool IsLevel => BINBlock >= 3;
        public bool IsBossFight => IsLevel && BINBlock % 3 == 2;

        #endregion

        #region Public Methods

        public void Load_BIN(IDXEntry entry, int blockIndex)
        {
            BINBlock = blockIndex;

            // Null out previous data. This should get overwritten below when loading the new level block, but if a menu is loaded instead they will not.
            BackgroundPack = null;
            OA05 = null;
            LevelPack = null;

            ProcessBINFiles(entry, blockIndex, (cmd, i) =>
            {
                // Load the file
                var binFile = Load_BINFile(cmd, blockIndex, i);

                switch (cmd.FILE_Type)
                {
                    // Copy the TIM files data to VRAM
                    case IDXLoadCommand.FileType.Archive_TIM_Generic:
                    case IDXLoadCommand.FileType.Archive_TIM_SpriteSheets:
                        foreach (PS1_TIM tim in ((TIM_ArchiveFile)binFile).Files)
                            AddToVRAM(tim);
                        break;

                    case IDXLoadCommand.FileType.Archive_TIM_SongsText:
                    case IDXLoadCommand.FileType.Archive_TIM_SaveText:
                        // TODO: Save these, but don't load into VRAM
                        break;

                    // Save for later
                    case IDXLoadCommand.FileType.OA05:
                        OA05 = (OA05_File)binFile;
                        break;

                    case IDXLoadCommand.FileType.SEQ:
                        // TODO: Save once parsed
                        break;

                    // Save for later and copy the TIM files data to VRAM
                    case IDXLoadCommand.FileType.Archive_BackgroundPack:
                        BackgroundPack = (BackgroundPack_ArchiveFile)binFile;

                        foreach (PS1_TIM tim in BackgroundPack.TIMFiles.Files)
                            AddToVRAM(tim);
                        break;

                    // The fixed sprites are always the last set of sprite frames
                    case IDXLoadCommand.FileType.FixedSprites:
                        SpriteFrames[69] = (Sprites_ArchiveFile)binFile;
                        break;

                    // Add the level sprite frames
                    case IDXLoadCommand.FileType.Archive_SpritePack:
                        for (int j = 0; j < 69; j++)
                            SpriteFrames[j] = ((LevelSpritePack_ArchiveFile)binFile).Sprites[j];
                        break;

                    // Save for later
                    case IDXLoadCommand.FileType.Archive_LevelPack:
                        LevelPack = (LevelPack_ArchiveFile)binFile;
                        break;

                    case IDXLoadCommand.FileType.Archive_Unk0:
                    case IDXLoadCommand.FileType.Archive_Unk4:
                        // TODO: Save once parsed
                        break;

                    case IDXLoadCommand.FileType.Code:
                    case IDXLoadCommand.FileType.Unknown:
                    default:
                        // Do nothing
                        break;
                }
            });
        }

        public BaseFile Load_BINFile(IDXLoadCommand cmd, int blockIndex, int cmdIndex)
        {
            BINBlock = blockIndex;

            switch (cmd.FILE_Type)
            {
                case IDXLoadCommand.FileType.Archive_TIM_Generic:
                case IDXLoadCommand.FileType.Archive_TIM_SongsText:
                case IDXLoadCommand.FileType.Archive_TIM_SaveText:
                case IDXLoadCommand.FileType.Archive_TIM_SpriteSheets:
                    return Load_BINFile<TIM_ArchiveFile>(cmd, blockIndex, cmdIndex);

                case IDXLoadCommand.FileType.OA05:
                    return Load_BINFile<OA05_File>(cmd, blockIndex, cmdIndex);

                case IDXLoadCommand.FileType.SEQ:
                    // TODO: Parse SEQ
                    return null;

                case IDXLoadCommand.FileType.Archive_BackgroundPack:
                    return Load_BINFile<BackgroundPack_ArchiveFile>(cmd, blockIndex, cmdIndex);

                case IDXLoadCommand.FileType.FixedSprites:
                    return Load_BINFile<Sprites_ArchiveFile>(cmd, blockIndex, cmdIndex);

                case IDXLoadCommand.FileType.Archive_SpritePack:
                    return Load_BINFile<LevelSpritePack_ArchiveFile>(cmd, blockIndex, cmdIndex);

                case IDXLoadCommand.FileType.Archive_LevelPack:
                    return Load_BINFile<LevelPack_ArchiveFile>(cmd, blockIndex, cmdIndex);

                case IDXLoadCommand.FileType.Archive_Unk0:
                    return Load_BINFile<Unk0_ArchiveFile>(cmd, blockIndex, cmdIndex);

                case IDXLoadCommand.FileType.Archive_Unk4:
                    return Load_BINFile<RawData_ArchiveFile>(cmd, blockIndex, cmdIndex);

                case IDXLoadCommand.FileType.Code:
                    // Ignore compiled code
                    return null;

                case IDXLoadCommand.FileType.Unknown:
                default:
                    Context.Logger.LogWarning($"Unsupported file format for file at command {cmdIndex} parsed at 0x{cmd.FILE_FunctionPointer:X8}");
                    return null;
            }
        }

        public T Load_BINFile<T>(IDXLoadCommand cmd, int blockIndex, int cmdIndex)
            where T : BaseFile, new()
        {
            BINBlock = blockIndex;

            var s = Context.Deserializer;

            return s.SerializeObject<T>(null, x =>
            {
                x.Pre_FileSize = cmd.FILE_Length;
                x.Pre_IsCompressed = false;
            }, name: $"BIN_File_{blockIndex}_{cmdIndex}");
        }

        public void ProcessBINFiles(IDXEntry entry, int blockIndex, Action<IDXLoadCommand, int> action)
        {
            BINBlock = blockIndex;

            var s = Context.Deserializer;
            var binFile = Context.GetFile(FilePath_BIN);

            // Enumerate every load command
            for (int cmdIndex = 0; cmdIndex < entry.LoadCommands.Length; cmdIndex++)
            {
                var cmd = entry.LoadCommands[cmdIndex];

                // Seek
                if (cmd.Type == 1)
                {
                    s.Goto(binFile.StartPointer + cmd.BIN_Offset);
                }
                // Read file
                else if (cmd.Type == 2)
                {
                    var p = s.CurrentPointer;

                    // Add a region for nicer pointer logging
                    binFile.AddRegion(p.FileOffset, cmd.FILE_Length, $"File_{cmdIndex}");

                    // Process the file
                    action(cmd, cmdIndex);

                    // Go to the end of the file for the next file
                    s.Goto(p + cmd.FILE_Length);
                }
            }
        }

        public void AddToVRAM(PS1_TIM tim)
        {
            // Add the palette if available
            if (tim.Clut != null)
                VRAM.AddPalette(tim.Clut.Palette, 0, 0, tim.Clut.XPos * 2, tim.Clut.YPos, tim.Clut.Width * 2, tim.Clut.Height);

            // Add the image data
            if (!(tim.XPos == 0 && tim.YPos == 0) && tim.Width != 0 && tim.Height != 0)
                VRAM.AddDataAt(0, 0, tim.XPos * 2, tim.YPos, tim.ImgData, tim.Width * 2, tim.Height);
        }

        #endregion

        #region Public Static Methods

        public static Loader GetLoader(Context context) => context.GetStoredObject<Loader>(Key);
        public static Loader Create(Context context)
        {
            // Create the loader
            var loader = new Loader(context);

            // Store in the context so it can be accessed
            context.StoreObject(Key, loader);

            return loader;
        }

        #endregion
    }
}