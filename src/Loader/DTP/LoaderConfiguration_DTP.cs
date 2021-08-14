using System.Collections.Generic;
using BinarySerializer.Klonoa.DTP;

namespace BinarySerializer.Klonoa
{
    public abstract class LoaderConfiguration_DTP : LoaderConfiguration
    {
        public virtual int BLOCK_Fix => 0;
        public virtual int BLOCK_Menu => 1;
        public virtual int BLOCK_FirstLevel => 3;

        public virtual string FilePath_BIN => "FILE.BIN";
        public virtual string FilePath_IDX => "FILE.IDX";
        public virtual string FilePath_EXE => "KLONOA.BIN";
        public virtual uint Address_EXE => 0x80011000;

        public abstract Dictionary<uint, uint> FileAddresses { get; }
        public abstract Dictionary<uint, IDXLoadCommand.FileType> FileTypes { get; }

        public abstract uint Address_LevelData3DFunction { get; }
        public abstract uint Address_LevelData2DPointerTable { get; }
    }
}