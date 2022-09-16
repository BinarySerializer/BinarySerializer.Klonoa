namespace BinarySerializer.Klonoa
{
    /// <summary>
    /// A base loader manager for managing BIN files
    /// </summary>
    public abstract class BaseLoader
    {
        protected BaseLoader(Context context)
        {
            Context = context;
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
        /// The Klonoa settings, used for version specific properties and functionality
        /// </summary>
        public KlonoaSettings KlonoaSettings => Context.GetRequiredSettings<KlonoaSettings>();

        public KlonoaGameVersion GameVersion => KlonoaSettings.Version;

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
    }
}