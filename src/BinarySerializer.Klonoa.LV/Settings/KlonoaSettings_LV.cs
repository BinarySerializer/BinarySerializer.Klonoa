using System;

namespace BinarySerializer.Klonoa.LV
{
    public abstract class KlonoaSettings_LV : KlonoaSettings
    {
        public virtual int LanguagesCount => 1;
        public bool HasMultipleLanguages => LanguagesCount > 1;

        public virtual string FilePath_HEAD => "HEADPACK.BIN";

        public virtual string GetFilePath(Loader.BINType bin, int languageIndex = 0)
        {
            return bin switch
            {
                Loader.BINType.KL => HasMultipleLanguages ? $"KLDATA{languageIndex + 1}.BIN" : "KLDATA.BIN",
                Loader.BINType.BGM => "BGMPACK.BIN",
                Loader.BINType.PPT => "PPTPACK.BIN",
                _ => throw new ArgumentOutOfRangeException(nameof(bin), bin, null)
            };
        }
    }
}