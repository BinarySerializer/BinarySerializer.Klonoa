using System.Collections.Generic;

namespace BinarySerializer.KlonoaDTP
{
    public abstract class LoaderConfiguration
    {
        public virtual int BLOCK_Fix => 0;
        public virtual int BLOCK_Menu => 1;
        public virtual int BLOCK_FirstLevel => 3;

        public virtual string FilePath_EXE => "KLONOA.BIN";
        public virtual uint Address_EXE => 0x8000b070;

        public abstract Dictionary<uint, uint> FileAddresses { get; }

        public abstract uint Address_CodeLevelDataFunction { get; }
        public abstract uint Address_LevelPointerTable { get; }
    }
}