# BIN (DTP)

The BIN file is split into multiple blocks where each block (except the first 3) represents a level. Although each block contains different file types, and in a different order, these are the most common ones.

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

## TIM Archive
These files are archives which contain `.TIM` (graphics) files. This is used to load textures and palettes into the VRAM.

## Backgrounds
An archive with the backgrounds for the level. Contains `.TIM` (the tilesets), `.CEL` (the tiles) and `.BGD` (the maps) files as well as the background modifiers for each sector. The modifiers define the layers, camera clear, palette animations etc.

## Sprites
The sprites for the 2D animations used in the level. These are stored in a global array with 70 entries. The last entry contains fixed sprites while the rest are the ones used in the current level.

## OA05
Contains multiple `.VAB` and `.SEQ` files. These are sounds used in the level.

## SEQ
Just a `.SEQ` file. Most likely the level music.

## Code
Blocks of compiled code. This doesn't get parsed and instead just loaded into memory. These define the object functions and cutscenes among other level-specific functionality.

## Level
The primary level data. Contains the object models, sector data (such as the level models and movement paths) and more.