namespace BinarySerializer.Klonoa.KH
{
    public enum WorldMapFileType
    {
        Unknown,

        // Background 0 (the overlay) - FUN_08029188, FUN_08029720
        BG0B, // Background?
        BG0P, // Palette
        BG0W, // World?
        BG0V, // Vision?

        // ?
        T2S4,
        T2VA,

        OBJ4, // World map objects
        OBJP, // Object palettes

        TEST, // Unused? Sky box graphic.
        WMAP, // The affine graphics for the main world map
        VMAP, // Vision maps?
    }
}