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

**Common**

* [File (format)](docs/Format_FILE.md)
* [Compression](docs/Compression.md)

**Door to Phantomile**

* [IDX (format)](docs/DTP/Format_IDX.md)
* [BIN (format)](docs/DTP/Format_BIN.md)
* [Level](docs/DTP/Level.md)
* [Object](docs/DTP/Object.md)

**Lunatea's Veil**

N/A