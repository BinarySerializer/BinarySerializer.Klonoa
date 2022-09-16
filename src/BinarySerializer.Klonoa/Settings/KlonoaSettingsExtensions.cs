namespace BinarySerializer.Klonoa
{
    public static class KlonoaSettingsExtensions
    {
        public static void AddKlonoaSettings<Settings>(this Context context, Settings settings)
            where Settings : KlonoaSettings
        {
            context.AddSettings<KlonoaSettings>(settings);
        }

        public static KlonoaSettings GetKlonoaSettings(this Context context) => 
            context.GetRequiredSettings<KlonoaSettings>();
        public static KlonoaSettings TryGetKlonoaSettings(this Context context) => 
            context.GetSettings<KlonoaSettings>();

        public static Settings GetKlonoaSettings<Settings>(this Context context)
            where Settings : KlonoaSettings
        {
            return (Settings)context.GetRequiredSettings<KlonoaSettings>();
        }
        public static Settings TryGetKlonoaSettings<Settings>(this Context context)
            where Settings : KlonoaSettings
        {
            KlonoaSettings settings = context.GetSettings<KlonoaSettings>();

            if (settings == null)
                return null;

            return (Settings)settings;
        }
    }
}