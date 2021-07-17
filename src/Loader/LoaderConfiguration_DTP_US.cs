﻿using System.Collections.Generic;

namespace BinarySerializer.KlonoaDTP
{
    public class LoaderConfiguration_DTP_US : LoaderConfiguration
    {
        public override Dictionary<uint, uint> FileAddresses => new Dictionary<uint, uint>()
        {
            [0x1000000] = 0x801108f8,
            [0x2000000] = 0x801100b8,
            [0x3000000] = 0x8016a790,
        };
        public override uint CodeLevelDataFunctionAddress => 0x80110488;
    }
}