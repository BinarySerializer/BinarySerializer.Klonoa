# Object (DTP)
The game has a global array of 64 entries for the objects. A game object is defined as anything which updates every frame, so not necessarily something which draws to the image buffer. The objcts use a primary object type (0-50) where the functions for them are defined in a pointer table (at 0x800b1bbc in the NTSC version). Many of the object types have level specific implementations and secondary types.

Each object has a struct of 180 bytes. Most values differ based on the type, but the first 4 are always the same. The first value is a pointer to an optional function for the object. The next is an optional pointer to the draw function. The next two are pointers to additional data. The first data object usually contains drawing related data.

## Types
Below is a description of the most common object types.

**1. 2D object**

These are usually the enemies in the level. Most enemies spawn from portals (even if they're not visible). The enemy data can reference other data which contains data specific for that secondary type, such as the despawn distance and waypoints.

**4. 2D collectible**

These are the collectibles, such as Dream Stones and checkpoints. They are usually placed directly on a path without an absolute position defined.

**40/41. Generic & 3D object**

These contain the 3D objects as well as other generic object types such as VRAM scroll animations, timers etc. These stay in memory throughout the entire level (or until the object is destroyed). Objects Klonoa can interact with, such as moving platforms, are usually type 40. Each object specifies an array of asset indices to determine which BIN assets it uses.