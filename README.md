# BinarySerializer.KlonoaDTP
BinarySerializer.KlonoaDTP is an extension library to [BinarySerializer](https://github.com/RayCarrot/BinarySerializer) for serializing the BIN file data in Klonoa: Door to Phantomile. This can be used for reverse engineering and to read any of its files, export the data (such as the graphics, models, sounds etc.) and more.

# Get Started
To use this library in your own project you must first reference [BinarySerializer](https://github.com/RayCarrot/BinarySerializer) and [BinarySerializer.PS1](https://github.com/RayCarrot/BinarySerializer.PS1) as it relies on these libraries. After that you can quickly get started using the `Loader` class.

```cs
// First create a context for the data serialization
using Context context = new Context(basePath);

// Add the IDX and BIN to the context. The BIN gets added as a linear file while the IDX has to be memory mapped.
context.AddFile(new LinearSerializedFile(context, Loader.FilePath_BIN));
context.AddFile(new MemoryMappedFile(context, Loader.FilePath_IDX, 0x80010000));

// Load the IDX
IDX idx = Load_IDX(context);

// Get the entry you want to load. Entry 3 is Vision 1-1 for example.
IDXEntry entry = idx.Entries[3];

// Create the loader
Loader loader = Loader.Create(context);

// Load the BIN
loader.Load_BIN(entry, 3);

// The data is now stored in the loader and can be accessed
LevelPack_ArchiveFile level = loader.LevelPack;
Sprites_ArchiveFile[] spriteFrames = loader.SpriteFrames;
```

# Documentation
All of the data for the game is stored in the `FILE.BIN` file with the `FILE.IDX` telling us how to parse it. The BIN is split into 25 blocks, each representing a level (except the first 3).

Why is everything stored in a single file? Well, primarily it's for performance when loading. Now when the game wants to load a level all of its data gets read from the same place, thus is doesn't need to seek around to different sectors on the disc! This inevitably means data gets duplicated across levels, but that's not an issue since discs can store a lot of data (a lot more than the PS1 can store in memory).

## IDX
The starts with an 8-byte header followed by a pointer array. Each of these pointers leads to the load commands used to load the block we want to load. As mnetioned earlier each block represents a level except the first 3. Here are some of the blocks as an example:

- 0: Fixed data (stays in memory throughout the game)
- 1: Menu (the main menu)
- 2: Code (multiple code files)
- 3: Vision 1-1
- 4: Vision 1-2

etc.

### Load Commands
The load commands tells the game how to load the block. Each block consists of sub-blocks which we can call files (so each level has multiple files that get loaded - there are however no file names or such). The first command is always a seek command which has the game seek to the start of the block on the disc (it does so using a supplied sector index, `LBA`, relative to where the BIN is on the disc).

The following commands are responsible for loading the files in the block. Files have different formats, so how do we know how to parse them? Well the game does it using a function pointer which is included with each file. This points to a function in the exe which then parses the file. We can use this function pointer as a way to identify the file type since it will always stay the same for the same type of file.

## BIN
As explained in the previous section each block in the BIN consists of multiple files. There are many different types of files, but primarily we can split them into two categories; `normal files` and `archives`. Archives contain an array of additional files (such as a folder).

Now let's check out some of the file types in the BIN and what they are for.

### TIM
These files are archives which contain `.TIM` (graphics) files. This is used to load textures and palettes into the VRAM.

### OA05
Contains multiple `.VAB` and `.SEQ` files. Most likely the sound effects used for the level.

### Code
Blocks of compiled code. This doesn't get parsed and instead just loaded into memory. Mosty likely used for level specific functionality, such as bosses and cutscenes.

### SEQ
Just a `.SEQ` file. Most likely the level music.

### Backgrounds
An archive with the backgrounds for the level. Contains `.TIM` (the tilesets), `.CEL` (the tiles) and `.BGD` (the maps) files as well as some unknown files (most likely for animations).

### Sprites
The sprites for the 2D animations used in the level.

### Level
The primary level data. Contains the object models, sector data (such as the level models and movement paths) and more.

As an example, here are all the files in Vision 1-1:
- 1: OA05 (sounds)
- 2: TIM (sprite sheets)
- 3: TIM (textures)
- 4: Backgrounds
- 6: Code
- 7: Code
- 8: Sprites
- 9: Level
- 10: SEQ

## Compression
The game uses multiple compression types. So far I've found 3 of them, with 2 being implemented.

### ULZ
A variant of LZSS. It has 4 types, with only 2 of them being used in the game.

### Raw texture block
A compression used on certain raw texture data blocks.

### Unknown
An unknown compression type used on the level files which has not been implemented.