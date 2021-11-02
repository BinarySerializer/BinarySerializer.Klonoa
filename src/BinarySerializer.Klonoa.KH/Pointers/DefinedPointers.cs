using System.Collections.Generic;

namespace BinarySerializer.Klonoa.KH
{
    public static class DefinedPointers
    {
        public static Dictionary<DefinedPointer, long> GBA_JP => new Dictionary<DefinedPointer, long>()
        {
            [DefinedPointer.MenuPack] = 0x08267940,
            [DefinedPointer.EnemyAnimationsPack] = 0x08399f10,
            [DefinedPointer.GameplayPack] = 0x088fda70,
            [DefinedPointer.ItemsPack] = 0x0825c880,
            [DefinedPointer.UIPack] = 0x0828fd20,
            [DefinedPointer.StoryPack] = 0x0873d350,
            [DefinedPointer.MapsPack] = 0x08b30fd0,
            [DefinedPointer.WorldMapPack] = 0x082f9870,
            [DefinedPointer.EnemyObjectDefinitions] = 0x08068720,
        };
    }
}