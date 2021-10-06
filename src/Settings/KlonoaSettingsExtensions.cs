namespace BinarySerializer.Klonoa
{
    public static class KlonoaSettingsExtensions
    {
        public static void AddKlonoaSettings<Settings>(this Context context, Settings settings)
            where Settings : KlonoaSettings
        {
            context.AddSettings<KlonoaSettings>(settings);
        }

        public static KlonoaSettings GetKlonoaSettings(this Context context, bool throwIfNotFound = true)
        {
            return context.GetSettings<KlonoaSettings>(throwIfNotFound);
        }

        public static Settings GetKlonoaSettings<Settings>(this Context context, bool throwIfNotFound = true)
            where Settings : KlonoaSettings
        {
            var settings = context.GetSettings<KlonoaSettings>(throwIfNotFound);

            if (settings == null)
                return null;

            return (Settings)settings;
        }
    }
}