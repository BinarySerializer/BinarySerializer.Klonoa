using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    public class SectorConfig_File : BaseFile
    {
        // Only exists if file size is 0x30
        public short MapSpeedS { get; set; }
        public short MapSpeedT { get; set; }
        public short BgSpeedS { get; set; }
        public short BgSpeedT { get; set; }

        // Regular fields
        public KlonoaLV_IntColor FogColor { get; set; }
        public int FogNear { get; set; }
        public int FogFar { get; set; }
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
                BgSpeedS = s.Serialize<short>(BgSpeedS, name: nameof(BgSpeedS));
                BgSpeedT = s.Serialize<short>(BgSpeedT, name: nameof(BgSpeedT));
            }

            FogColor = s.SerializeObject<KlonoaLV_IntColor>(FogColor, name: nameof(FogColor));
            FogNear = s.Serialize<int>(FogNear, name: nameof(FogNear));
            FogFar = s.Serialize<int>(FogFar, name: nameof(FogFar));
            BGT = s.SerializeObject<PS2_RGBA8888Color>(BGT, name: nameof(BGT));
            BGU = s.SerializeObject<PS2_RGBA8888Color>(BGU, name: nameof(BGU));
        }
    }
}