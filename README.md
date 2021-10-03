# BinarySerializer.Klonoa
BinarySerializer.Klonoa is an extension library to [BinarySerializer](https://github.com/RayCarrot/BinarySerializer) for serializing the game data in the console Klonoa games. This can be used for reverse engineering and to read any of its files, export the data (such as the graphics, models, sounds etc.) and more.

To view the parsed data directly from the files the [Binary Data Explorer](https://github.com/RayCarrot/BinaryDataExplorer) tool can be used.

![Map Example](img/map_example.png)
Example image is from this library being used in [Ray1Map](https://github.com/Adsolution/Ray1Map) to display the maps.

# Supported Games and Versions
## Currently Supported Versions
* Klonoa: Door to Phantomile (Prototype 1997/07/17, NTSC)
* Klonoa 2: Lunatea's Veil (NTSC, PAL - _currently limited support only_)

## Planned Versions
* Klonoa: Door to Phantomile (Prototype 1997/12/18, NTSC-J, PAL, Demo)
* Klonoa: Beach Volleyball
* Klonoa (Wii)

_Note:_ The first two GBA games are currently only supported directly in [Ray1Map](https://github.com/Adsolution/Ray1Map/tree/master/Assets/Scripts/DataTypes/GBAKlonoa). They might be migrated to this library in the future.

# Get Started
To use this library in your own project you must first reference [BinarySerializer](https://github.com/RayCarrot/BinarySerializer) and [BinarySerializer.PS1](https://github.com/RayCarrot/BinarySerializer.PS1) as it relies on these libraries. After that you can quickly get started using the `Loader` class.

## Door to Phantomile

```cs
// First create a context for the data serialization
using Context context = new Context(basePath);

// Create a configuration
LoaderConfiguration_DTP_US config = new LoaderConfiguration_DTP_US();

// Add the IDX and BIN to the context. The BIN gets added as a linear file while the IDX has to be memory mapped. If the level data will be parsed then the exe needs to be added too.
context.AddFile(new LinearFile(context, config.FilePath_BIN));
context.AddFile(new MemoryMappedFile(context, config.FilePath_IDX, config.Address_IDX));

// Load the IDX and pass in the configuration to it
IDX idx = FileFactory.Read<IDX>(config.FilePath_IDX, context, (s, idxObj) => idxObj.Pre_LoaderConfig = config);

// Create the loader, passing in the context, IDX and configuration
Loader_DTP loader = Loader_DTP.Create(context, idx, config);

// Switch to the BIN block you want to load. Block 3 is Vision 1-1 for example, while block 0 is the fixed block.
loader.SwitchBlocks(3);

// Load and process the BIN block
loader.LoadAndProcessBINBlock();

// The data is now stored in the loader and can be accessed
LevelPack_ArchiveFile level = loader.LevelPack;
Sprites_ArchiveFile[] spriteSets = loader.SpriteSets;

// For example the level model can be accessed in the level pack
PS1_TMD levelModel = level.Sectors[0].LevelModel;

// The textures and palettes are stored in the virtual VRAM in the loader
PS1_VRAM vram = loader.VRAM;
```

## Lunatea's Veil
```cs
// First create a context for the data serialization
using Context context = new Context(basePath);

// Create a configuration
LoaderConfiguration_LV_US config = new LoaderConfiguration_LV_US();

// Add the header pack file to the context
context.AddFile(new LinearFile(context, config.FilePath_HEAD));

// Add the BIN files to the context
for (int i = 0; i < 3; i++)
{
    var bin = (Loader_LV.BINType)i;

    // The KL file can have multiple language BINs depending on the version
    if (bin == Loader_LV.BINType.KL)
    {
        for (int lang = 0; lang < config.LanguagesCount; lang++)
            context.AddFile(new LinearFile(context, config.GetFilePath(bin, languageIndex: lang)));
    }
    else
    {
        context.AddFile(new LinearFile(context, config.GetFilePath(bin)));
    }
}

// Load the header pack
HeadPack_ArchiveFile headPack = FileFactory.Read<HeadPack_ArchiveFile>(config.FilePath_HEAD, context, (_, head) => head.Pre_HasMultipleLanguages = config.HasMultipleLanguages);

// Create the loader, passing in the context, header pack and configuration
Loader_LV loader = Loader_LV.Create(context, headPack, config);

// Load data from the BIN files
LevelPack_ArchiveFile levelPack = loader.LoadBINFile<LevelPack_ArchiveFile>(Loader_LV.BINType.KL, 10);
```

# Documentation

**BIN Block**

A BIN block is a block of data in the game data in DTP. Each block consists of multiple load commands with function pointers for parsing the data. Loading the data from a level requires loading its BIN block.

**Archive**

A collection of files. An archive is defined by having a header with offsets leading to each file it contains. Although there is no defined length for each file we can determine it using all the available offsets (this is harder in LV due to its data aligning).

When an archive doesn't have any files it has the count set to -1. Multiple file offsets can point to the same file, although this is rare and mostly used for dummy data.

The order of each file is very important. The game parses these archives in two ways. The first is where each file is indexed at a specific place. The second way is where each file is of the same type and the game enumerated through it (easily handled with the generic `ArchiveFile<T>`).

It is worth noting an archive can contain multiple archives! Because of thise `ArchiveFile` inherits from `BaseFile`.

**File**

A file is any data type which is contained in an archive. Parsing a file from an archive will automatically fill out its size and decompress it if it's compressed.

**Sector**

A level is split into multiple parts, which in the library are named sectors. The game keeps track of the global sector ID calulated as `8 + (10 * Level) + Sector` where the level will usually be the block starting from the third one.

**Modifier**

The game has multiple lists of objects in memory. The most important ones are the primary and background objects. An object is defined as anything which updates every frame, with an optional function for drawing content to the buffer. Because of this not all objects correspond to in-game objects and can be for things such as VRAM animations, timers etc. Due to this they are named modifiers in the library as they simply modify some part of the game. The background objects work the same, where most are background layers and others can be things such as palette animations.

**Code**

Certain data has to be parsed from the game code, such as the objects. This is separated from the BIN loading as it's parsed differently. The BIN however had to be loaded first as the code data relies on it. Several archives are generic asset packs where the data type can only be determined from the code data.

**Cutscenes**

The cutscenes in the game are handled based on commands which are processed for specific frames. Some of these are cutscene specific while others are more generic. The text is drawn using font indices, where each cutscene has its own font. Skipping a cutscene will run a smaller cutscene command collection making sure all the objects are positioned correctly.

# Game Documentation (Door to Phantomile)
All of the data for the game is stored in the `FILE.BIN` file with the `FILE.IDX` telling us how to parse it. The BIN is split into 25 blocks, each representing a level (except the first 3).

Why is everything stored in a single file? Well, primarily it's for performance when loading. Now when the game wants to load a level all of its data gets read from the same place, thus is doesn't need to seek around to different sectors on the disc! This inevitably means data gets duplicated across levels, but that's not an issue since discs can store a lot of data (a lot more than the PS1 can store in memory).

## IDX
The IDX file starts with an 8-byte header followed by a pointer array. Each of these pointers leads to the load commands (which are stored further down in the file) used to load the block we want to load. As mentioned earlier each block represents a level except the first 3. Here are the first couple of blocks as an example:

- 0: Fixed data (stays in memory throughout the game)
- 1: Menu (the main menu)
- 2: Code (multiple code files)
- 3: Vision 1-1
- 4: Vision 1-2

etc.

### Load Commands
The load commands tells the game how to load the block. Each block consists of sub-blocks which we can refer to as files (so each level has multiple files that get loaded - there are however no file names or such). The first command is always a seek command which has the game seek to the start of the block on the disc. It does so using a supplied sector index, `LBA`, relative to where the BIN is on the disc. For example the third block has the LBA offset 573. To get the offset in the BIN file we do `573 * 2048 = 1173504` (each sector is 2048 bytes).

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
- 1: OA05 (sound bank)
- 2: TIM (sprite sheets)
- 3: TIM (textures)
- 4: Backgrounds
- 6: Code
- 7: Code
- 8: Sprites
- 9: Level
- 10: SEQ

## Compression
The game uses multiple compression types.

### ULZ
A variant of LZSS. It has 4 types, with only 2 of them being used in the game.

### Raw texture block
A compression used on certain raw texture data blocks.

### Level sector
An unknown compression type used for the level sectors.