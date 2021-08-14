namespace BinarySerializer.Klonoa
{
    public abstract class Loader
    {
        protected Loader(Context context, LoaderConfiguration loaderConfig)
        {
            Context = context;
            LoaderConfig = loaderConfig;
        }

        /// <summary>
        /// The key in the context for the current loader
        /// </summary>
        protected const string Key = "KLONOA_LOADER";

        /// <summary>
        /// The context
        /// </summary>
        public Context Context { get; }

        /// <summary>
        /// The loader config, used for version specific properties and functionality
        /// </summary>
        public LoaderConfiguration LoaderConfig { get; }

        public LoaderConfiguration.GameVersion GameVersion => LoaderConfig.Version;

        /// <summary>
        /// The deserializer object
        /// </summary>
        public BinaryDeserializer Deserializer => Context.Deserializer;

        protected abstract void InitializeBIN();

        /// <summary>
        /// Initializes the loader to be used with the current context. This should only be done once after construction.
        /// </summary>
        protected void Initialize()
        {
            // Store in the context so it can be accessed
            Context.StoreObject(Key, this);

            // Initialize the BIN files
            InitializeBIN();
        }

        public static LoaderConfiguration GetConfiguration(Context context) => context.GetStoredObject<Loader>(Key)?.LoaderConfig;
    }
}