namespace BinarySerializer.Klonoa.LV
{
    public class GIMIDs_File : BaseFile
    {
        public int Pre_Count { get; set; }

        public GIM_IDs[] IDs { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            IDs = s.SerializeArray<GIM_IDs>(IDs, Pre_Count, name: nameof(IDs));
        }
    }

    public enum GIM_IDs : ushort
    {
        Bubble,
        Pnd00,
        Pnd01,
        Pnd02,
        Dim1,
        Ball,
        Crescent,
        Cube,
        Diamond,
        Star,
        Triangle,
        Wave,
        Explosion1,
        Explosion2,
        ExplosionTL,
        Fire1,
        Firework,
        ItemLight,
        Lightning,
        Fire,
        FireA,
        Smoke1,
        SyouAir,
        SyouCircle,
        SyouGround,
        Pnd02A,
        Grass,
        Gra01,
        Missile,
        ZeE1,
        ZeE2,
        ZeE3,
        ZeE4,
        ZeE5,
        ZeE6,
        Spec1,
        Leaf02,
        Leaf01,
        Leaf03,
        Aura02,
        Aura01,
        Aura02A,
        Aura02O,
        Aura02B,
        BupC01,
        BupC02,
        BupC03,
        BupS01,
        BupS02,
        BupS03,
        Water,
        Nuki,
        MizHNE,
        MizSizK,
        Spray,
        Wave1,
        Wave2,
        Wave3,
        Hamon,
        ZR01,
        Nuki02,
        Nuki03,
        FireCH01,
        FireCH02,
        Leaf2,
        Leaf1,
        Leaf3,
        Dim27,
        Dim_27,
        Lightning21,
        FireCH03,
        FireCH04,
        FireCH05,
        Pnd00B,
        Pnd01B,
        Pnd04,
        Pnd03,
        GrassE23,
        Win00,
        FireworkB
    }
}