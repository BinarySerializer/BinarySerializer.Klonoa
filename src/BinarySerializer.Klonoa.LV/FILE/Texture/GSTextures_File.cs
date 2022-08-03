using BinarySerializer.PS2;

namespace BinarySerializer.Klonoa.LV
{
    // File that contains one or more textures
    // TODO: Implement this
    public class GSTextures_File : BaseFile
    {
        public GSTexture_Packet[] Packets { get; set; }
        public Chain_DMAtag DMATag { get; set; }
        public VIFcode VIFCode_NOP { get; set; }
        public VIFcode VIFCode_DIRECTHL { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            DMATag = s.SerializeObject<Chain_DMAtag>(DMATag, name: nameof(DMATag));
            if (DMATag.QWC != 0)
            {
                if (DMATag.ADDR == 0x10 && DMATag.QWC == 0x01)
                {
                    // Menu sprites have an extra DMAtag for whatever reason
                    s.Align(16);
                    DMATag = s.SerializeObject<Chain_DMAtag>(DMATag, name: nameof(DMATag));
                }
                VIFCode_NOP = s.SerializeObject<VIFcode>(VIFCode_NOP, name: nameof(VIFCode_NOP));
                VIFCode_DIRECTHL = s.SerializeObject<VIFcode>(VIFCode_DIRECTHL, name: nameof(VIFCode_DIRECTHL));
                Packets = s.SerializeObjectArrayUntil<GSTexture_Packet>(Packets, x => x.GIFTag_Packed.EOP, name: nameof(Packets));
            }
            
        }
    }
}