# Level (DTP)

A level is split into multiple parts, which in the library are named sectors. The game keeps track of the global sector ID calulated as `8 + (10 * Level) + Sector` where the level will usually be the block starting from the third one.