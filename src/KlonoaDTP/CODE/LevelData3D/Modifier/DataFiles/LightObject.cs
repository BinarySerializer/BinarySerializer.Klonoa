using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class LightObject : BaseFile
    {
        public ModifierObject Pre_ModifierObj { get; set; }

        public PS1_TMD TMD { get; set; }
        public ObjPositions_File LightPositions { get; set; } // Each light has two positions, source and destination

        public override void SerializeImpl(SerializerObject s)
        {
            if (Pre_ModifierObj.Short_00 == 0x11)
            {
                LightPositions = s.SerializeObject<ObjPositions_File>(LightPositions, x =>
                {
                    x.Pre_FileSize = Pre_FileSize;
                    x.Pre_IsCompressed = Pre_IsCompressed;
                }, name: nameof(LightPositions));
            }
            else
            {
                TMD = s.SerializeObject<PS1_TMD>(TMD, name: nameof(TMD));
                s.Goto(Offset + Pre_FileSize);
            }
        }
    }
}