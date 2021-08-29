using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class LightObject : BaseFile
    {
        public ModifierObject Pre_ModifierObj { get; set; }

        public PS1_TMD TMD { get; set; }
        public ScenerySprites_File LightVectors { get; set; } // Each light has two vectors, source and destination

        public override void SerializeImpl(SerializerObject s)
        {
            if (Pre_ModifierObj.Short_00 == 0x11)
            {
                LightVectors = s.SerializeObject<ScenerySprites_File>(LightVectors, x =>
                {
                    x.Pre_FileSize = Pre_FileSize;
                    x.Pre_IsCompressed = Pre_IsCompressed;
                }, name: nameof(LightVectors));
            }
            else
            {
                TMD = s.SerializeObject<PS1_TMD>(TMD, name: nameof(TMD));
                s.Goto(Offset + Pre_FileSize);
            }
        }
    }
}