using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    // Low-level commands to transfer texture data to GS VRAM
    // Very similar to this: https://openkh.dev/kh2/file/raw-texture.html
    // TODO: Implement this
    public class GSTexture_Packet : BinarySerializable
    {
        public GIFtag GIFTag_Packed { get; set; }

        // Image/palette data
        public GSReg_BITBLTBUF BITBLTBUF { get; set; }
        public GSReg_TRXPOS TRXPOS { get; set; }
        public GSReg_TRXREG TRXREG { get; set; }
        public GSReg_TRXDIR TRXDIR { get; set; }
        public GIFtag GIFTag_Image { get; set; }
        public byte[] RawData { get; set; }

        // End of packet
        public Chain_DMAtag DMATag { get; set; }
        public GSReg_TEXFLUSH TEXFLUSH { get; set; }
        public VIFcode VIFCode_NOP1 { get; set; }
        public VIFcode VIFCode_NOP2 { get; set; }


        public override void SerializeImpl(SerializerObject s)
        {
            GIFTag_Packed = s.SerializeObject<GIFtag>(GIFTag_Packed, name: nameof(GIFTag_Packed));
            if (GIFTag_Packed.NREG == 4) // Image/palette data
            {
                BITBLTBUF = s.SerializeObject<GSReg_BITBLTBUF>(BITBLTBUF, onPreSerialize: x => x.SerializeTag = true, name: nameof(BITBLTBUF));
                TRXPOS = s.SerializeObject<GSReg_TRXPOS>(TRXPOS, onPreSerialize: x => x.SerializeTag = true, name: nameof(TRXPOS));
                TRXREG = s.SerializeObject<GSReg_TRXREG>(TRXREG, onPreSerialize: x => x.SerializeTag = true, name: nameof(TRXREG));
                TRXDIR = s.SerializeObject<GSReg_TRXDIR>(TRXDIR, onPreSerialize: x => x.SerializeTag = true, name: nameof(TRXDIR));
                GIFTag_Image = s.SerializeObject<GIFtag>(GIFTag_Image, name: nameof(GIFTag_Image));
                RawData = s.SerializeArray<byte>(RawData, GIFTag_Image.NLOOP * 0x10);
            } else // End of packet
            {
                TEXFLUSH = s.SerializeObject<GSReg_TEXFLUSH>(TEXFLUSH, onPreSerialize: x => x.SerializeTag = true, name: nameof(TEXFLUSH));
                if (GIFTag_Packed.EOP != 1)
                    // For level sector textures, the first GIFTag is not marked as EOP
                    // There is however an extra GIFTag that is marked as EOP, so let's use that instead so the condition-check in GSTextures_File works properly
                    GIFTag_Packed = s.SerializeObject<GIFtag>(GIFTag_Packed, name: nameof(GIFTag_Packed));
                DMATag = s.SerializeObject<Chain_DMAtag>(DMATag, name: nameof(DMATag));
                VIFCode_NOP1 = s.SerializeObject<VIFcode>(VIFCode_NOP1, name: nameof(VIFCode_NOP1));
                VIFCode_NOP2 = s.SerializeObject<VIFcode>(VIFCode_NOP2, name: nameof(VIFCode_NOP2));
            }
        }
    }
}