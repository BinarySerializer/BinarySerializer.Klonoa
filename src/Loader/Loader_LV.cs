using System;
using BinarySerializer.Klonoa.LV;

namespace BinarySerializer.Klonoa
{
    public class Loader_LV : Loader
    {
        #region Constructor

        protected Loader_LV(Context context, HeadPack_ArchiveFile headPack) : base(context)
        {
            HeadPack = headPack;
        }

        #endregion

        #region Public Properties

        public HeadPack_ArchiveFile HeadPack { get; }

        /// <summary>
        /// The Klonoa settings, used for version specific properties and functionality
        /// </summary>
        public KlonoaSettings_LV Settings => (KlonoaSettings_LV)KlonoaSettings;

        #endregion

        #region Protected Methods

        protected override void InitializeBIN()
        {
            if (Settings.HasMultipleLanguages)
            {
                for (int langIndex = 0; langIndex < HeadPack.KLDATA_Multi.OffsetTable.FilesCount; langIndex++)
                    InitializeBIN(BINType.KL, HeadPack.KLDATA_Multi.Files[langIndex].FileDescriptors, langIndex);
            }
            else
            {
                InitializeBIN(BINType.KL, HeadPack.KLDATA_Single.FileDescriptors);
            }

            InitializeBIN(BINType.BGM, HeadPack.BGMPACK.FileDescriptors);
            InitializeBIN(BINType.PPT, HeadPack.PPTPACK.FileDescriptors);
        }

        protected void InitializeBIN<T>(BINType bin, T[] files, int languageIndex = 0)
            where T : BINHeader_BaseFileDescriptor
        {
            var binFile = Context.GetFile(Settings.GetFilePath(bin, languageIndex));

            if (binFile == null)
                return;

            for (int fileIndex = 0; fileIndex < files.Length; fileIndex++)
            {
                var fileDescriptor = files[fileIndex];

                // Set the pointer
                fileDescriptor.FilePointer = new Pointer(fileDescriptor.FILE_Offset, binFile);

                // Add a region for nicer pointer logging
                var regionName = $"File_{bin}{(Settings.HasMultipleLanguages ? languageIndex.ToString() : null)}_{fileIndex}";
                binFile.AddRegion(fileDescriptor.FILE_Offset, fileDescriptor.FILE_Length, regionName);
            }
        }

        #endregion

        #region Public Methods

        public BINHeader_File GetBINHeader(BINType bin, int languageIndex = 0)
        {
            return bin switch
            {
                BINType.KL => Settings.HasMultipleLanguages ? HeadPack.KLDATA_Multi.Files[languageIndex] : HeadPack.KLDATA_Single,
                BINType.BGM => throw new Exception($"The BGM pack does not use {nameof(BINHeader_File)}"),
                BINType.PPT => HeadPack.PPTPACK,
                _ => throw new ArgumentOutOfRangeException(nameof(bin), bin, null)
            };
        }

        public BINHeader_BGM_File GetBINHeader_BGM() => HeadPack.BGMPACK;

        /// <summary>
        /// Loads the archive/file at the given index in the specified BIN file and automatically decides the type
        /// </summary>
        /// <param name="bin">The BIN to load from</param>
        /// <param name="fileIndex">The BIN file to load</param>
        /// <param name="languageIndex">The language index</param>
        /// <param name="bgmIndex">The BGM index if a BGM bin</param>
        /// <returns>The loaded file</returns>
        public BaseFile LoadBINFile(BINType bin, int fileIndex, int languageIndex = 0, int bgmIndex = 0)
        {
            switch (bin) {
                case BINType.KL:
                    if (fileIndex == 199) // Menu sprites
                        return LoadBINFile<MenuSpritesPack_ArchiveFile>(bin, fileIndex, languageIndex, bgmIndex);
                    
                    bool isPreload = fileIndex % 2 == 0; // Even archives are preload, odd archives are data
                    if (isPreload)
                        return LoadBINFile<LevelPreloadPack_ArchiveFile>(bin, fileIndex, languageIndex, bgmIndex);
                    else
                        return LoadBINFile<LevelDataPack_ArchiveFile>(bin, fileIndex, languageIndex, bgmIndex);
                case BINType.BGM:
                case BINType.PPT:
                default:
                    // No parser yet for BGM/PPT files
                    return LoadBINFile<RawData_File>(bin, fileIndex, languageIndex, bgmIndex);
            }
        }

        /// <summary>
        /// Loads the BIN file of a generic type in the specified BIN
        /// </summary>
        /// <typeparam name="T">The type of file to load</typeparam>
        /// <param name="bin">The BIN to load from</param>
        /// <param name="fileIndex">The BIN file to load</param>
        /// <param name="languageIndex">The language index</param>
        /// <param name="bgmIndex">The BGM index if a BGM bin</param>
        /// <returns>The loaded file</returns>
        public T LoadBINFile<T>(BINType bin, int fileIndex, int languageIndex = 0, int bgmIndex = 0)
            where T : BaseFile, new()
        {
            var s = Deserializer;

            long length;

            if (bin == BINType.BGM)
            {
                var header = GetBINHeader_BGM();
                var fileDescriptor = header.FileDescriptors[fileIndex];
                s.Goto(fileDescriptor.FilePointer + fileDescriptor.FileAbsoluteLength * bgmIndex);
                length = fileDescriptor.FileLength;
            }
            else
            {
                var header = GetBINHeader(bin, languageIndex);
                var fileDescriptor = header.FileDescriptors[fileIndex];
                s.Goto(fileDescriptor.FilePointer);
                length = fileDescriptor.FILE_Length;
            }

            // Serialize the file
            var file = s.SerializeObject<T>(null, x => x.Pre_FileSize = length, name: $"{bin}_{fileIndex}");

            // Return the file
            return file;
        }

        #endregion

        #region Data Types

        public enum BINType
        {
            KL = 0,
            BGM = 1,
            PPT = 2,
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the loader associated with the specified context
        /// </summary>
        /// <param name="context">The context the loader was created for</param>
        /// <returns>The loader</returns>
        public static Loader_LV GetLoader(Context context) => context.GetStoredObject<Loader_LV>(Key);

        /// <summary>
        /// Creates a new loader for a specific context
        /// </summary>
        /// <param name="context">The context to create the loader for</param>
        /// <param name="headPack">The serialized head pack</param>
        /// <returns>The loader</returns>
        public static Loader_LV Create(Context context, HeadPack_ArchiveFile headPack)
        {
            // Make sure a loader hasn't already been created for the context
            if (GetLoader(context) != null)
                throw new Exception($"A loader has already been created for the current context. Only one loader is allowed per context.");

            // Create the loader
            var loader = new Loader_LV(context, headPack);

            // Initialize the loader
            loader.Initialize();

            return loader;
        }

        #endregion
    }
}