namespace BinarySerializer.Klonoa.DTP
{
    public enum PrimaryObjectType : short
    {
        Invalid = -1,
        None = 0,
        
        Enemy_2D = 1,

        Collectible_2D = 4,

        // Handled almost the same. 40 seems to be more for moving platforms and such that you can interact with.
        Object_3D_40 = 40,
        Object_3D_41 = 41,

        CutsceneObject = 45,
    }
}