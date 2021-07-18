using BinarySerializer.PS1;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BinarySerializer.KlonoaDTP
{
    /// <summary>
    /// Handles loading data from the game BIN
    /// </summary>
    public class Loader
    {
        #region Constructor

        protected Loader(Context context, IDX idx, LoaderConfiguration config)
        {
            Context = context;
            IDX = idx;
            Config = config;
            VRAM = new PS1_VRAM();
            SpriteFrames = new Sprites_ArchiveFile[70];
        }

        #endregion

        #region Constants

        /// <summary>
        /// The key in the context for the current loader
        /// </summary>
        private const string Key = "KLONOA_LOADER";

        /// <summary>
        /// The file path for the BIN file
        /// </summary>
        public const string FilePath_BIN = "FILE.BIN";

        /// <summary>
        /// The file path for the IDX file
        /// </summary>
        public const string FilePath_IDX = "FILE.IDX";

        #endregion

        #region Public Properties

        /// <summary>
        /// The context
        /// </summary>
        public Context Context { get; }
        
        /// <summary>
        /// The serialized IDX
        /// </summary>
        public IDX IDX { get; }
        
        /// <summary>
        /// The loader config, used for version specific properties and functionality
        /// </summary>
        public LoaderConfiguration Config { get; }

        /// <summary>
        /// The current IDX entry, determined by the <see cref="BINBlock"/>
        /// </summary>
        public IDXEntry IDXEntry => IDX.Entries[BINBlock];

        /// <summary>
        /// The current BIN block being loaded
        /// </summary>
        public int BINBlock { get; protected set; }
        
        /// <summary>
        /// Indicates if the current BIN block is a level
        /// </summary>
        public bool IsLevel => BINBlock >= Config.BLOCK_FirstLevel;
        
        /// <summary>
        /// Indicates if the current BIN block is a boss level
        /// </summary>
        public bool IsBossFight => IsLevel && BINBlock % 3 == 2;

        /// <summary>
        /// The sector to serialize when serializing the level data, or -1 to serialize all of them
        /// </summary>
        public int SectorToParse { get; set; } = -1;

        #endregion

        #region Loaded Data

        /// <summary>
        /// The VRAM. This gets allocated to when processing TIM files.
        /// </summary>
        public PS1_VRAM VRAM { get; }

        /// <summary>
        /// The sprite frames array
        /// </summary>
        public Sprites_ArchiveFile[] SpriteFrames { get; }

        /// <summary>
        /// The backgrounds
        /// </summary>
        public BackgroundPack_ArchiveFile BackgroundPack { get; set; }

        /// <summary>
        /// The sound bank
        /// </summary>
        public OA05_File OA05 { get; set; }

        /// <summary>
        /// The level pack
        /// </summary>
        public LevelPack_ArchiveFile LevelPack { get; set; }

        /// <summary>
        /// The hard-coded level data
        /// </summary>
        public CodeLevelData CodeLevelData { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Changes which block is currently being loaded
        /// </summary>
        /// <param name="blockIndex">The new block to load</param>
        public void SwitchBlocks(int blockIndex)
        {
            if (blockIndex >= IDX.Entries.Length)
                throw new Exception($"Block index {blockIndex} is out of range. Should be between 0-{IDX.Entries.Length - 1}");

            BINBlock = blockIndex;
        }

        /// <summary>
        /// Loads and processes every file in the current BIN block
        /// </summary>
        /// <param name="logAction">An optional action for logging</param>
        public void LoadAndProcessBINBlock(Action<string> logAction = null)
        {
            // Null out previous data. This should get overwritten below when loading the new level block, but if a menu is loaded instead they will not.
            BackgroundPack = null;
            OA05 = null;
            LevelPack = null;

            LoadBINFiles((cmd, i) =>
            {
                logAction?.Invoke($"Loading BIN {BINBlock} file {i} of type {cmd.FILE_Type}");

                ProcessBINFile(i);
            });
        }

        /// <summary>
        /// Loads and processes every file in the current BIN block, with async logging
        /// </summary>
        /// <param name="logAction">An optional action for logging</param>
        /// <returns>The task</returns>
        public async Task LoadAndProcessBINBlockAsync(Func<string, Task> logAction = null)
        {
            // Null out previous data. This should get overwritten below when loading the new level block, but if a menu is loaded instead they will not.
            BackgroundPack = null;
            OA05 = null;
            LevelPack = null;

            await LoadBINFilesAsync(async (cmd, i) =>
            {
                if (logAction != null)
                    await logAction($"Loading BIN {BINBlock} file {i} of type {cmd.FILE_Type}");

                ProcessBINFile(i);
            });
        }

        /// <summary>
        /// Processes the BIN file in the current BIN block
        /// </summary>
        /// <param name="fileIndex">The file to process</param>
        public void ProcessBINFile(int fileIndex)
        {
            // Load the file
            var binFile = LoadBINFile(fileIndex);
            var cmd = IDXEntry.LoadCommands[fileIndex];

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

                // Memory map code files
                case IDXLoadCommand.FileType.Code:
                    var rawData = ((RawData_File)binFile).Data;
                    var f = new MemoryMappedByteArrayFile(Context, $"CODE_{BINBlock}_{fileIndex}", cmd.GetFileDestinationAddress(this), rawData);
                    Context.AddFile(f);
                    break;

                case IDXLoadCommand.FileType.Archive_Unk0:
                case IDXLoadCommand.FileType.Archive_Unk4:
                case IDXLoadCommand.FileType.Archive_Unk5:
                    // TODO: Save once parsed
                    break;

                case IDXLoadCommand.FileType.Unknown:
                default:
                    // Do nothing
                    break;
            }
        }

        /// <summary>
        /// Loads the BIN file in the current BIN block
        /// </summary>
        /// <param name="fileIndex">The file to load</param>
        /// <returns>The loaded file</returns>
        public BaseFile LoadBINFile(int fileIndex)
        {
            var cmd = IDXEntry.LoadCommands[fileIndex];

            switch (cmd.FILE_Type)
            {
                case IDXLoadCommand.FileType.Archive_TIM_Generic:
                case IDXLoadCommand.FileType.Archive_TIM_SongsText:
                case IDXLoadCommand.FileType.Archive_TIM_SaveText:
                case IDXLoadCommand.FileType.Archive_TIM_SpriteSheets:
                    return LoadBINFile<TIM_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.OA05:
                    return LoadBINFile<OA05_File>(fileIndex);

                case IDXLoadCommand.FileType.SEQ:
                    // TODO: Parse SEQ
                    return null;

                case IDXLoadCommand.FileType.Archive_BackgroundPack:
                    return LoadBINFile<BackgroundPack_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.FixedSprites:
                    return LoadBINFile<Sprites_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Archive_SpritePack:
                    return LoadBINFile<LevelSpritePack_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Archive_LevelPack:
                    return LoadBINFile<LevelPack_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Archive_Unk0:
                    return LoadBINFile<Unk0_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Archive_Unk4:
                case IDXLoadCommand.FileType.Archive_Unk5:
                    return LoadBINFile<RawData_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Code:
                    return LoadBINFile<RawData_File>(fileIndex);

                case IDXLoadCommand.FileType.Unknown:
                default:
                    Context.Logger.LogWarning($"Unsupported file format for file {fileIndex} parsed at 0x{cmd.FILE_FunctionPointer:X8}");
                    return null;
            }
        }

        /// <summary>
        /// Loads the BIN file of a generic type in the current BIN block
        /// </summary>
        /// <typeparam name="T">The type of file to load</typeparam>
        /// <param name="fileIndex">The file to load</param>
        /// <returns>The loaded file</returns>
        public T LoadBINFile<T>(int fileIndex)
            where T : BaseFile, new()
        {
            var s = Context.Deserializer;
            var cmd = IDXEntry.LoadCommands[fileIndex];

            return s.SerializeObject<T>(null, x =>
            {
                x.Pre_FileSize = cmd.FILE_Length;
                x.Pre_IsCompressed = false;
            }, name: $"BIN_File_{BINBlock}_{fileIndex}");
        }

        /// <summary>
        /// Load the BIN files in the current BIN block
        /// </summary>
        /// <param name="loadAction">The action used to load each BIN file</param>
        public void LoadBINFiles(Action<IDXLoadCommand, int> loadAction)
        {
            var s = Context.Deserializer;

            // Enumerate every load command
            for (int cmdIndex = 0; cmdIndex < IDXEntry.LoadCommands.Length; cmdIndex++)
            {
                var cmd = IDXEntry.LoadCommands[cmdIndex];

                if (cmd.Type == 2)
                {
                    s.Goto(cmd.FILE_Pointer);
                    loadAction(cmd, cmdIndex);
                }
            }
        }

        /// <summary>
        /// Load the BIN files in the current BIN block with the load action being async
        /// </summary>
        /// <param name="loadAction">The action used to load each BIN file</param>
        /// <returns>The task</returns>
        public async Task LoadBINFilesAsync(Func<IDXLoadCommand, int, Task> loadAction)
        {
            var s = Context.Deserializer;

            // Enumerate every load command
            for (int cmdIndex = 0; cmdIndex < IDXEntry.LoadCommands.Length; cmdIndex++)
            {
                var cmd = IDXEntry.LoadCommands[cmdIndex];

                if (cmd.Type == 2)
                {
                    s.Goto(cmd.FILE_Pointer);
                    await loadAction(cmd, cmdIndex);
                }
            }
        }

        /// <summary>
        /// Process the hard-coded level data from the processed code files
        /// </summary>
        public void ProcessCodeLevelData()
        {
            var s = Context.Deserializer;
            var funcAddr = Config.CodeLevelDataFunctionAddress;

            // Helper for finding the file the pointer points to
            BinaryFile findFile(uint addr)
            {
                var files = Context.MemoryMap.Files.OfType<MemoryMappedByteArrayFile>().ToList();
                files.Sort((a, b) => b.BaseAddress.CompareTo(a.BaseAddress));
                var file = files.FirstOrDefault(f => addr >= f.BaseAddress && addr <= f.BaseAddress + f.Length);

                if (file == null)
                    throw new Exception($"Code file for parsing the code level data has not been loaded");

                return file;
            }

            var funcPointer = new Pointer(funcAddr, findFile(funcAddr));

            var dataAddr = s.DoAt(funcPointer, () =>
            {
                // Parse the MIPS instructions and get the pointer for the level data. Hopefully this function is always structured the same.
                var instructions = s.SerializeObjectArrayUntil<MIPS_Instruction>(default, x => x.Mnemonic == MIPS_Instruction.InstructionMnemonic.jr, name: $"CodeLevelDataFunction");

                if (instructions.Length != 5)
                    throw new Exception($"Unknown function structure. Expected 5 instructions, got {instructions.Length}.");

                var registers = new uint[32];

                foreach (var instr in instructions)
                {
                    if (instr.Mnemonic == MIPS_Instruction.InstructionMnemonic.jr)
                        break;

                    switch (instr.Mnemonic)
                    {
                        case MIPS_Instruction.InstructionMnemonic.sll:
                            registers[instr.RD] = registers[instr.RT] << instr.Shift;
                            s.Log($"R{instr.RD}: 0x{registers[instr.RD]:X8}");
                            break;

                        case MIPS_Instruction.InstructionMnemonic.addu:
                            registers[instr.RD] = registers[instr.RS] + registers[instr.RT];
                            s.Log($"R{instr.RD}: 0x{registers[instr.RD]:X8}");
                            break;

                        case MIPS_Instruction.InstructionMnemonic.lui:
                            registers[instr.RT] = (uint)(instr.IMM << 16);
                            s.Log($"R{instr.RT}: 0x{registers[instr.RT]:X8}");
                            break;

                        case MIPS_Instruction.InstructionMnemonic.lw:
                            registers[instr.RT] = (uint)(instr.IMM + registers[instr.RS]);
                            s.Log($"R{instr.RT}: 0x{registers[instr.RT]:X8}");
                            break;

                        case MIPS_Instruction.InstructionMnemonic.Unknown:
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                // The address will be stored in r2
                return registers[2];
            });

            var dataPointer = new Pointer(dataAddr, findFile(dataAddr));

            // Read the data
            CodeLevelData = s.DoAt(dataPointer, () => s.SerializeObject<CodeLevelData>(default, x =>
            {
                x.Pre_SectorsCount = LevelPack?.Sectors?.Length ?? 0;
                x.Pre_ObjectModelsDataPack = LevelPack?.ObjectModelsDataPack;
            }, name: nameof(CodeLevelData)));
        }

        /// <summary>
        /// Adds the TIM file data to the VRAM
        /// </summary>
        /// <param name="tim">The TIM file to add</param>
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

        /// <summary>
        /// Gets the loader associated with the specified context
        /// </summary>
        /// <param name="context">The context the loader was created for</param>
        /// <returns>The loader</returns>
        public static Loader GetLoader(Context context) => context.GetStoredObject<Loader>(Key);
        
        /// <summary>
        /// Creates a new loader for a specific context
        /// </summary>
        /// <param name="context">The context to create the loader for</param>
        /// <param name="idx">The serialized IDX</param>
        /// <param name="config">The loader configuration</param>
        /// <returns>The loader</returns>
        public static Loader Create(Context context, IDX idx, LoaderConfiguration config)
        {
            // Create the loader
            var loader = new Loader(context, idx, config);

            // Store in the context so it can be accessed
            context.StoreObject(Key, loader);

            return loader;
        }

        #endregion
    }
}