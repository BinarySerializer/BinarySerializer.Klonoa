using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class LevelSectorConfig_File : BaseFile
    {
        // Only exists if file size is 0x30
        public short MapSpeedS { get; set; }
        public short MapSpeedT { get; set; }
        public short BackgroundSpeedS { get; set; }
        public short BackgroundSpeedT { get; set; }

        // Regular fields
        public KlonoaLV_IntColor FogColor { get; set; } // Fog only applies to level geometry
        public int FogStart { get; set; }
        public int FogEnd { get; set; }
        public PS2_RGBA8888Color BGT { get; set; }
        public PS2_RGBA8888Color BGU { get; set; }
        public bool IsDummy => Pre_FileSize == 0x10;

        public override void SerializeImpl(SerializerObject s)
        {
            if (IsDummy)
                return;
            if (Pre_FileSize == 0x30)
            {
                s.SerializeMagicString("MBSTVER1", 8);
                MapSpeedS = s.Serialize<short>(MapSpeedS, name: nameof(MapSpeedS));
                MapSpeedT = s.Serialize<short>(MapSpeedT, name: nameof(MapSpeedT));
                BackgroundSpeedS = s.Serialize<short>(BackgroundSpeedS, name: nameof(BackgroundSpeedS));
                BackgroundSpeedT = s.Serialize<short>(BackgroundSpeedT, name: nameof(BackgroundSpeedT));
            }

            FogColor = s.SerializeObject<KlonoaLV_IntColor>(FogColor, name: nameof(FogColor));
            FogStart = s.Serialize<int>(FogStart, name: nameof(FogStart));
            FogEnd = s.Serialize<int>(FogEnd, name: nameof(FogEnd));
            BGT = s.SerializeObject<PS2_RGBA8888Color>(BGT, name: nameof(BGT));
            BGU = s.SerializeObject<PS2_RGBA8888Color>(BGU, name: nameof(BGU));
        }
    }
}