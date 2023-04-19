namespace BinarySerializer.Klonoa.LV
{
    public class SFX_File : BaseFile
    {
        public string Magic { get; set; } // FX
        public byte[] Magic2 { get; set; } // 80 00 40 00
        public ushort PartCount { get; set; }
        public float VertexScale { get; set; } // Multiplies vertices by this value
        public SFXPart[] Parts { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            Magic = s.SerializeString(Magic, 2, name: nameof(Magic));
            PartCount = s.Serialize<ushort>(PartCount, name: nameof(PartCount));
            Magic2 = s.SerializeArray<byte>(Magic2, 4, name: nameof(Magic2));
            VertexScale = s.Serialize<float>(VertexScale, name: nameof(VertexScale));
            s.SerializePadding(4);
            Parts = s.SerializeObjectArray<SFXPart>(Parts, PartCount, onPreSerialize: x => x.Pre_GeometryPointer = Offset, name: nameof(Parts));
            s.Goto(Offset + Pre_FileSize);
        }
    }
}