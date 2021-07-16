using System.Collections.Generic;

namespace BinarySerializer.KlonoaDTP
{
    public abstract class LoaderConfiguration
    {
        public virtual int BLOCK_Fix => 0;
        public virtual int BLOCK_Menu => 1;
        public virtual int BLOCK_FirstLevel => 3;

        public abstract Dictionary<uint, uint> FileAddresses { get; }
        public abstract uint LevelTableFunctionAddress { get; }
    }
}