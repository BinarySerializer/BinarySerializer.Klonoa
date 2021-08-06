using System.Collections.Generic;

namespace BinarySerializer.KlonoaDTP
{
    public abstract class LoaderConfiguration
    {
        public abstract GameVersion Version { get; }

        public virtual int BLOCK_Fix => 0;
        public virtual int BLOCK_Menu => 1;
        public virtual int BLOCK_FirstLevel => 3;

        public virtual string FilePath_EXE => "KLONOA.BIN";
        public virtual uint Address_EXE => 0x80011000;

        public abstract Dictionary<uint, uint> FileAddresses { get; }
        public abstract Dictionary<uint, IDXLoadCommand.FileType> FileTypes { get; }

        public abstract uint Address_LevelData3DFunction { get; }
        public abstract uint Address_LevelData2DPointerTable { get; }

        public enum GameVersion
        {
            DTP_Prototype_19970717,
            DTP
        }
    }
}