# IDX (DTP)

All of the assets for the game is stored in the `FILE.BIN` file with the `FILE.IDX` specifying how to parse it. The BIN is split into 25 blocks, each representing a level (except the first 3).

Why is everything stored in a single file? Well, primarily it's for performance when loading. Now when the game wants to load a level all of its data gets read from the same place, thus is doesn't need to seek around to different sectors on the disc! This inevitably means data gets duplicated across levels, but that's not an issue since discs can store a lot of data (a lot more than the PS1 can store in memory).

The IDX file starts with an 8-byte header followed by a pointer array. Each of these pointers leads to the load commands (which are stored further down in the file) used to load the block we want to load. As mentioned earlier each block represents a level except the first 3. Here are the first couple of blocks as an example:

- 0: Fixed data (stays in memory throughout the game)
- 1: Menu (the main menu)
- 2: Unknown (only code files, appears to be for the intro)
- 3: Vision 1-1
- 4: Vision 1-2

etc.

The load commands tells the game how to load the block. Each block consists of sub-blocks which we can refer to as files (so each level has multiple files that get loaded - there are however no file names or such). The first command is always a seek command which has the game seek to the start of the block on the disc. It does so using a supplied sector index, `LBA`, relative to where the BIN is on the disc. For example the third block has the LBA offset 0x23D. To get the offset in the BIN file we do `0x23D * 0x800 = 0x11E800` (each sector is 0x800 bytes).

The following commands are responsible for loading the files in the block. Files have different formats, so how do we know how to parse them? Well the game does it using a function pointer which is included with each file. This points to a function in the exe which then parses the file. We can use this function pointer as a way to identify the file type since it will always stay the same for the same type of file.