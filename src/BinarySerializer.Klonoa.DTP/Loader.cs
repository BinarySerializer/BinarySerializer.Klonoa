using BinarySerializer.PS1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// Handles loading data from the game BIN
    /// </summary>
    public class Loader : BaseLoader
    {
        #region Constructor

        protected Loader(Context context, IDX idx) : base(context)
        {
            IDX = idx;
            MemoryFiles = new HashSet<MemoryMappedByteArrayFile>();
            VRAM = new PS1_VRAM();
            SpriteSets = new Sprites_ArchiveFile[70];
            LoadedFiles = new BaseFile[idx.Entries.Length][];

            for (int i = 0; i < LoadedFiles.Length; i++)
                LoadedFiles[i] = new BaseFile[idx.Entries[i].LoadCommands.Length];
        }

        #endregion

        #region Protected Properties

        protected HashSet<MemoryMappedByteArrayFile> MemoryFiles { get; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The serialized IDX
        /// </summary>
        public IDX IDX { get; }

        /// <summary>
        /// The Klonoa settings, used for version specific properties and functionality
        /// </summary>
        public KlonoaSettings_DTP Settings => (KlonoaSettings_DTP)KlonoaSettings;

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
        public bool IsLevel => BINBlock >= Settings.BLOCK_FirstLevel;
        
        /// <summary>
        /// Indicates if the current BIN block is a boss level
        /// </summary>
        public bool IsBossFight => IsLevel && BINBlock % 3 == 2;

        /// <summary>
        /// The global sector index, or -1 if not available
        /// </summary>
        public int GlobalSectorIndex => !IsLevel ? -1 : 8 + (10 * (BINBlock - Settings.BLOCK_FirstLevel)) + (LevelSector == -1 ? 0 : LevelSector);

        /// <summary>
        /// The global level index, or -1 if not available
        /// </summary>
        public int GlobalLevelIndex => !IsLevel ? -1 : (GlobalSectorIndex - 8) / 10;

        /// <summary>
        /// The amount of available levels
        /// </summary>
        public int LevelsCount => IDX.Entries.Length - Settings.BLOCK_FirstLevel;

        /// <summary>
        /// The sector to serialize when serializing the level data, or -1 to serialize all of them
        /// </summary>
        public int LevelSector { get; set; } = -1;

        #endregion

        #region Loaded Data

        /// <summary>
        /// The VRAM. This gets allocated to when processing TIM files.
        /// </summary>
        public PS1_VRAM VRAM { get; }

        /// <summary>
        /// The sprite sets array
        /// </summary>
        public Sprites_ArchiveFile[] SpriteSets { get; }

        /// <summary>
        /// The loaded BIN files
        /// </summary>
        public BaseFile[][] LoadedFiles { get; }

        /// <summary>
        /// The backgrounds
        /// </summary>
        public BackgroundPack_ArchiveFile BackgroundPack => GetLoadedFile<BackgroundPack_ArchiveFile>();

        /// <summary>
        /// The level pack
        /// </summary>
        public LevelPack_ArchiveFile LevelPack => GetLoadedFile<LevelPack_ArchiveFile>();

        /// <summary>
        /// The hard-coded level data (3D)
        /// </summary>
        public LevelData3D LevelData3D { get; set; }

        /// <summary>
        /// The hard-coded level data (2D)
        /// </summary>
        public LevelData2D LevelData2D { get; set; }

        #endregion

        #region Protected Methods

        protected override void InitializeBIN()
        {
            // Set the file pointers
            Pointer p = null;
            var s = Deserializer;
            var binFile = s.Context.GetFile(Settings.FilePath_BIN);

            for (int binBlockIndex = 0; binBlockIndex < IDX.Entries.Length; binBlockIndex++)
            {
                for (var loadCmdIndex = 0; loadCmdIndex < IDX.Entries[binBlockIndex].LoadCommands.Length; loadCmdIndex++)
                {
                    var cmd = IDX.Entries[binBlockIndex].LoadCommands[loadCmdIndex];

                    // Seek
                    if (cmd.Type == 1)
                    {
                        p = cmd.BIN_Pointer;
                    }
                    // File
                    else if (cmd.Type == 2)
                    {
                        if (p == null)
                            throw new Exception($"File load command can not appear before a seek commands");

                        // Set the pointer
                        cmd.FILE_Pointer = p;

                        // Add a region for nicer pointer logging
                        binFile.AddRegion(p.FileOffset, cmd.FILE_Length, $"File_{binBlockIndex}_{loadCmdIndex}");

                        // Increment pointer by the file size
                        p += cmd.FILE_Length;
                    }
                }
            }
        }

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

            // Null out previous data
            LevelData3D = null;
            LevelData2D = null;

            // Remove memory mapped code files
            foreach (var f in MemoryFiles)
                Context.RemoveFile(f);
            MemoryFiles.Clear();
        }

        /// <summary>
        /// Loads and processes every file in the current BIN block
        /// </summary>
        /// <param name="logAction">An optional action for logging</param>
        public void LoadAndProcessBINBlock(Action<string> logAction = null)
        {
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
        /// <returns>The loaded file</returns>
        public BaseFile ProcessBINFile(int fileIndex)
        {
            // Load the file
            var binFile = LoadBINFile(fileIndex);
            var cmd = IDXEntry.LoadCommands[fileIndex];

            switch (cmd.FILE_Type)
            {
                // Copy the TIM files data to VRAM
                case IDXLoadCommand.FileType.Archive_TIM_Generic:
                case IDXLoadCommand.FileType.Archive_TIM_SpriteSheets:
                    int invalidIndex = GameVersion == KlonoaGameVersion.DTP_Prototype_19970717 ? 107 : 106;

                    for (var i = 0; i < ((TIM_ArchiveFile)binFile).Files.Length; i++)
                    {
                        // Due to a dev oversight an unused texture (106) gets loaded in and replaces actual
                        // textures! This isn't visible in-game due to you only ever seen the low-res variants of it
                        // due to the camera never getting close enough. However we usually only want to deal with the
                        // high-res textures, so it's better to skip this and have it be shown correctly.
                        if (BINBlock == 19 && i == invalidIndex)
                            continue;

                        PS1_TIM tim = ((TIM_ArchiveFile)binFile).Files[i];
                        AddToVRAM(tim);
                    }

                    break;

                // Copy the TIM files data to VRAM
                case IDXLoadCommand.FileType.Archive_BackgroundPack:
                    var bg = (BackgroundPack_ArchiveFile)binFile;
                    for (int tileSetIndex = 0; tileSetIndex < bg.TIMFiles.Files.Length; tileSetIndex++)
                    {
                        var tim = bg.TIMFiles.Files[tileSetIndex];

                        // The game hard-codes this
                        if (tileSetIndex == 0)
                        {
                            tim.Clut.Region.XPos = 0x130;
                            tim.Clut.Region.YPos = 0x1F0;
                            tim.Clut.Region.Width = 0x10;
                            tim.Clut.Region.Height = 0x10;
                        }
                        else if (tileSetIndex == 1)
                        {
                            tim.Region.XPos = 0x1C0;
                            tim.Region.YPos = 0x100;
                            tim.Region.Width = 0x40;
                            tim.Region.Height = 0x100;

                            tim.Clut.Region.XPos = 0x120;
                            tim.Clut.Region.YPos = 0x1F0;
                            tim.Clut.Region.Width = 0x10;
                            tim.Clut.Region.Height = 0x10;
                        }

                        AddToVRAM(tim);
                    }
                    break;

                // The fixed sprites are always the last set of sprite frames
                case IDXLoadCommand.FileType.FixedSprites:
                    SpriteSets[69] = (Sprites_ArchiveFile)binFile;
                    break;

                // Add the level sprite frames
                case IDXLoadCommand.FileType.Archive_SpritePack:
                    for (int j = 0; j < 69; j++)
                        SpriteSets[j] = ((LevelSpritePack_ArchiveFile)binFile).Sprites[j];
                    break;

                // Copy the TIM files data to VRAM
                case IDXLoadCommand.FileType.Archive_WorldMap:
                    foreach (var tim in ((WorldMap_ArchiveFile)binFile).SpriteSheets.Files)
                        AddToVRAM(tim);

                    if (((WorldMap_ArchiveFile)binFile).Palette1 != null)
                        AddToVRAM(((WorldMap_ArchiveFile)binFile).Palette1);
                    break;
                
                // Memory map code files
                case IDXLoadCommand.FileType.Code:
                    var rawData = ((RawData_File)binFile).Data;
                    var f = new MemoryMappedByteArrayFile(Context, $"CODE_{BINBlock}_{fileIndex}", cmd.FILE_Destination, rawData);
                    Context.AddFile(f);
                    MemoryFiles.Add(f);
                    break;

                // Memory map code files
                case IDXLoadCommand.FileType.CodeNoDest:
                    // TODO: Load to memory. In the NTSC version it gets loaded to 0x8016a790 with pointer at 0x80011808.
                    break;
            }

            return binFile;
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
                    return LoadBINFile<RawData_File>(fileIndex);

                case IDXLoadCommand.FileType.Archive_BackgroundPack:
                    return LoadBINFile<BackgroundPack_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.FixedSprites:
                    return LoadBINFile<Sprites_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Archive_SpritePack:
                    return LoadBINFile<LevelSpritePack_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Archive_LevelMenuSprites:
                    return LoadBINFile<LevelMenuSprites_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Archive_LevelPack:
                    return LoadBINFile<LevelPack_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Archive_WorldMap:
                    return LoadBINFile<WorldMap_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Archive_MenuSprites:
                    return LoadBINFile<MenuSprites_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Proto_Archive_MenuSprites_0:
                case IDXLoadCommand.FileType.Proto_Archive_MenuSprites_1:
                case IDXLoadCommand.FileType.Proto_Archive_MenuSprites_2:
                    return LoadBINFile<Sprites_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Font:
                    return LoadBINFile<Font_File>(fileIndex);

                case IDXLoadCommand.FileType.Archive_MenuBackgrounds:
                    return LoadBINFile<ArchiveFile<TIM_ArchiveFile>>(fileIndex);

                case IDXLoadCommand.FileType.Archive_Unk0:
                    return LoadBINFile<Unk0_ArchiveFile>(fileIndex);

                case IDXLoadCommand.FileType.Unk1:
                    return LoadBINFile<RawData_File>(fileIndex);

                case IDXLoadCommand.FileType.Code:
                case IDXLoadCommand.FileType.CodeNoDest:
                    return LoadBINFile<RawData_File>(fileIndex);

                case IDXLoadCommand.FileType.Unknown:
                default:
                    Context.Logger.LogWarning($"Unsupported file format for file {fileIndex} parsed at 0x{cmd.FILE_FunctionPointer:X8}");
                    return LoadBINFile<RawData_File>(fileIndex);
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
            var s = Deserializer;
            var cmd = IDXEntry.LoadCommands[fileIndex];

            s.Goto(cmd.FILE_Pointer);

            // Serialize the file
            var file = s.SerializeObject<T>(null, x => x.Pre_FileSize = cmd.FILE_Length, name: $"BIN_File_{BINBlock}_{fileIndex}");

            // Store the loaded file
            LoadedFiles[BINBlock][fileIndex] = file;

            // Return the file
            return file;
        }

        /// <summary>
        /// Prepares to read the files in the current block by filling the cache for it
        /// </summary>
        /// <returns>The task</returns>
        public async Task FillCacheForBlockReadAsync()
        {
            var s = Deserializer;

            foreach (IDXLoadCommand loadCmd in IDXEntry.LoadCommands.Where(x => x.Type == 1))
            {
                s.Goto(loadCmd.BIN_Pointer);
                await s.FillCacheForReadAsync(loadCmd.BIN_Length);
            }
        }

        /// <summary>
        /// Load the BIN files in the current BIN block
        /// </summary>
        /// <param name="loadAction">The action used to load each BIN file</param>
        public void LoadBINFiles(Action<IDXLoadCommand, int> loadAction)
        {
            // Enumerate every load command
            for (int cmdIndex = 0; cmdIndex < IDXEntry.LoadCommands.Length; cmdIndex++)
            {
                var cmd = IDXEntry.LoadCommands[cmdIndex];

                if (cmd.Type == 2)
                    loadAction(cmd, cmdIndex);
            }
        }

        /// <summary>
        /// Load the BIN files in the current BIN block with the load action being async
        /// </summary>
        /// <param name="loadAction">The action used to load each BIN file</param>
        /// <returns>The task</returns>
        public async Task LoadBINFilesAsync(Func<IDXLoadCommand, int, Task> loadAction)
        {
            // Enumerate every load command
            for (int cmdIndex = 0; cmdIndex < IDXEntry.LoadCommands.Length; cmdIndex++)
            {
                var cmd = IDXEntry.LoadCommands[cmdIndex];

                if (cmd.Type == 2)
                    await loadAction(cmd, cmdIndex);
            }
        }

        /// <summary>
        /// Process the hard-coded level data from the processed code files
        /// </summary>
        public void ProcessLevelData()
        {
            var s = Deserializer;
            var funcAddr = Settings.Address_LevelData3DFunction;

            var funcPointer = new Pointer(funcAddr, FindCodeFile(funcAddr));

            var dataAddr = s.DoAt(funcPointer, () =>
            {
                // Parse the MIPS instructions and get the pointer for the level data. Hopefully this function is always structured the same.
                var instructions = s.SerializeObjectArrayUntil<MIPS_Instruction>(default, x => x.Mnemonic == MIPS_Instruction.InstructionMnemonic.jr, name: $"LevelData2DFunction");

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

            var dataPointer = new Pointer(dataAddr, FindCodeFile(dataAddr));

            // Read the data
            LevelData3D = s.DoAt(dataPointer, () => s.SerializeObject<LevelData3D>(default, x =>
            {
                x.Pre_SectorsCount = LevelPack?.Sectors?.Length ?? 0;
                x.Pre_ObjectAssets = LevelPack?.ObjectAssets;
            }, name: nameof(LevelData3D)));

            // Process the data only if we're loading a specific sector
            if (LevelSector != -1)
            {
                // Add TIM data to VRAM for each object which references a single TIM file. If there are multiple then it's animated and should get added later.
                foreach (var tim in LevelData3D.SectorGameObjectDefinition[LevelSector].ObjectsDefinitions.Select(x => x.Data?.TIM).Where(x => x != null))
                    AddToVRAM(tim);

                foreach (var tim in LevelData3D.SectorGameObjectDefinition[LevelSector].ObjectsDefinitions.Select(x => x.Data?.TIMArchive).Where(x => x != null).SelectMany(x => x.Files))
                    AddToVRAM(tim);
            }

            // Serialize level data 2D if a level index is specified
            if (GlobalLevelIndex != -1)
            {
                var pointerTablePointer = new Pointer(Settings.Address_LevelData2DPointerTable, Context.GetFile(Settings.FilePath_EXE));
                var levelDataPointer = s.DoAt(pointerTablePointer + (GlobalLevelIndex * 4), () => s.SerializePointer(default, name: "LevelData2DPointer"));
                LevelData2D = s.DoAt(levelDataPointer, () => s.SerializeObject<LevelData2D>(default, name: nameof(LevelData2D)));
            }
        }

        /// <summary>
        /// Adds the TIM file data to the VRAM
        /// </summary>
        /// <param name="tim">The TIM file to add</param>
        public void AddToVRAM(PS1_TIM tim)
        {
            if (tim == null) 
                throw new ArgumentNullException(nameof(tim));

            // Add the palette if available
            if (tim.Clut != null)
                VRAM.AddPalette(tim.Clut.Palette, 0, 0, tim.Clut.Region.XPos * 2, tim.Clut.Region.YPos, tim.Clut.Region.Width * 2, tim.Clut.Region.Height);

            // Add the image data
            if (!(tim.Region.XPos == 0 && tim.Region.YPos == 0) && tim.Region.Width != 0 && tim.Region.Height != 0)
                VRAM.AddDataAt(0, 0, tim.Region.XPos * 2, tim.Region.YPos, tim.ImgData, tim.Region.Width * 2, tim.Region.Height);
        }

        /// <summary>
        /// Adds the data to the VRAM
        /// </summary>
        /// <param name="data">The data to add</param>
        /// <param name="region">The region to add to</param>
        public void AddToVRAM(byte[] data, PS1_VRAMRegion region)
        {
            VRAM.AddDataAt(0, 0, region.XPos * 2, region.YPos, data, region.Width * 2, region.Height);
        }

        /// <summary>
        /// Gets the first loaded file of the specified type, or null if none was found
        /// </summary>
        /// <typeparam name="T">The file type</typeparam>
        /// <param name="binBlock">The bin block, or null for the current one</param>
        /// <returns>The found file, or null if none was found</returns>
        public T GetLoadedFile<T>(int? binBlock = null)
            where T : BaseFile
        {
            binBlock ??= BINBlock;

            return LoadedFiles[binBlock.Value].OfType<T>().FirstOrDefault();
        }

        public BinaryFile FindCodeFile(uint address)
        {
            var files = Context.MemoryMap.Files.OfType<MemoryMappedByteArrayFile>().ToList();
            files.Sort((a, b) => b.BaseAddress.CompareTo(a.BaseAddress));
            var file = files.FirstOrDefault(f => address >= f.BaseAddress && address <= f.BaseAddress + f.Length);

            if (file == null)
                throw new Exception($"No code BIN file loaded at the specified address of 0x{address:X8}");

            return file;
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
        /// <returns>The loader</returns>
        public static Loader Create(Context context, IDX idx)
        {
            // Make sure a loader hasn't already been created for the context
            if (GetLoader(context) != null)
                throw new Exception($"A loader has already been created for the current context. Only one loader is allowed per context.");

            // Create the loader
            var loader = new Loader(context, idx);

            // Initialize the loader
            loader.Initialize();

            return loader;
        }

        #endregion
    }
}