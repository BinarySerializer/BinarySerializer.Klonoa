using System;
using BinarySerializer.Klonoa.LV;

namespace BinarySerializer.Klonoa
{
    public class Loader_LV : Loader
    {
        #region Constructor

        protected Loader_LV(Context context, HeadPack_ArchiveFile headPack, LoaderConfiguration_LV config) : base(context, config)
        {
            HeadPack = headPack;
        }

        #endregion

        #region Constants

        /// <summary>
        /// The key in the context for the current loader
        /// </summary>
        private const string Key = "KLONOA_LV_LOADER";

        #endregion

        #region Protected Properties

        protected override string LoaderKey => Key;

        #endregion

        #region Public Properties

        public HeadPack_ArchiveFile HeadPack { get; }

        /// <summary>
        /// The loader config, used for version specific properties and functionality
        /// </summary>
        public LoaderConfiguration_LV Config => (LoaderConfiguration_LV)LoaderConfig;

        #endregion

        #region Protected Methods

        protected override void InitializeBIN()
        {
            if (Config.HasMultipleLanguages)
            {
                for (int langIndex = 0; langIndex < HeadPack.KLDATA_Multi.OffsetTable.FilesCount; langIndex++)
                    InitializeBIN(BINType.KL, HeadPack.KLDATA_Multi.Files[langIndex], langIndex);
            }
            else
            {
                InitializeBIN(BINType.KL, HeadPack.KLDATA_Single);
            }

            InitializeBIN(BINType.BGM, HeadPack.BGMPACK);
            InitializeBIN(BINType.PPT, HeadPack.PPTPACK);
        }

        protected void InitializeBIN(BINType bin, BINHeader_File header, int languageIndex = 0)
        {
            var binFile = Context.GetFile(Config.GetFilePath(bin, languageIndex));

            if (binFile == null)
                return;

            for (int fileIndex = 0; fileIndex < header.FilesCount; fileIndex++)
            {
                var fileDescriptor = header.FileDescriptors[fileIndex];

                // Set the pointer
                fileDescriptor.FilePointer = new Pointer(fileDescriptor.FileOffset, binFile);

                // Add a region for nicer pointer logging
                var regionName = $"File_{bin}{(Config.HasMultipleLanguages ? languageIndex.ToString() : null)}_{fileIndex}";
                binFile.AddRegion(fileDescriptor.FileOffset, fileDescriptor.FileLength, regionName);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the BIN file of a generic type in the specified BIN
        /// </summary>
        /// <typeparam name="T">The type of file to load</typeparam>
        /// <param name="bin">The BIN to load from</param>
        /// <param name="fileIndex">The BIN file to load</param>
        /// <param name="languageIndex">The language index</param>
        /// <returns>The loaded file</returns>
        public T LoadBINFile<T>(BINType bin, int fileIndex, int languageIndex = 0)
            where T : BaseFile, new()
        {
            var s = Deserializer;

            var header = bin switch
            {
                BINType.KL => Config.HasMultipleLanguages ? HeadPack.KLDATA_Multi.Files[languageIndex] : HeadPack.KLDATA_Single,
                BINType.BGM => HeadPack.BGMPACK,
                BINType.PPT => HeadPack.PPTPACK,
                _ => throw new ArgumentOutOfRangeException(nameof(bin), bin, null)
            };

            var fileDescriptor = header.FileDescriptors[fileIndex];

            s.Goto(fileDescriptor.FilePointer);

            // Serialize the file
            var file = s.SerializeObject<T>(null, x =>
            {
                x.Pre_FileSize = fileDescriptor.FileLength;
                x.Pre_IsCompressed = false;
            }, name: $"{bin}_{fileIndex}");

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
        /// <param name="config">The loader configuration</param>
        /// <returns>The loader</returns>
        public static Loader_LV Create(Context context, HeadPack_ArchiveFile headPack, LoaderConfiguration_LV config)
        {
            // Make sure a loader hasn't already been created for the context
            if (GetLoader(context) != null)
                throw new Exception($"A loader has already been created for the current context. Only one loader is allowed per context.");

            // Create the loader
            var loader = new Loader_LV(context, headPack, config);

            // Initialize the loader
            loader.Initialize();

            return loader;
        }

        #endregion
    }
}