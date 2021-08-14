namespace BinarySerializer.Klonoa
{
    public abstract class LoaderConfiguration
    {
        public abstract GameVersion Version { get; }

        public enum GameVersion
        {
            DTP_Prototype_19970717,
            DTP,

            LV,
        }
    }
}