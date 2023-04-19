namespace BinarySerializer.Klonoa.LV
{
    public class SDTParameter : BinarySerializable
    {
        public KlonoaLV_FloatVector Position { get; set; }
        public KlonoaLV_FloatVector Color { get; set; }
        public float Scale { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float SpraySize { get; set; }
        public float Time { get; set; }
        public int Distance { get; set; }
        public int PreviousHeight { get; set; }
        public int Frame { get; set; }
        public int StartFrame { get; set; }
        public int EndFrame { get; set; }
        public byte SWave { get; set; } // Seems like some sort of flag?
        public byte PtclCount { get; set; }
        public byte Ptcl { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Position = s.SerializeObject<KlonoaLV_FloatVector>(Position, onPreSerialize: x => x.Pre_HasW = true, name: nameof(Position));
            Color = s.SerializeObject<KlonoaLV_FloatVector>(Color, onPreSerialize: x => x.Pre_HasW = true, name: nameof(Color));
            Scale = s.Serialize<float>(Scale, name: nameof(Scale));
            Width = s.Serialize<float>(Width, name: nameof(Width));
            Height = s.Serialize<float>(Height, name: nameof(Height));
            SpraySize = s.Serialize<float>(SpraySize, name: nameof(SpraySize));
            Time = s.Serialize<float>(Time, name: nameof(Time));
            Distance = s.Serialize<int>(Distance, name: nameof(Distance));
            PreviousHeight = s.Serialize<int>(PreviousHeight, name: nameof(PreviousHeight));
            Frame = s.Serialize<int>(Frame, name: nameof(Frame));
            StartFrame = s.Serialize<int>(StartFrame, name: nameof(StartFrame));
            EndFrame = s.Serialize<int>(EndFrame, name: nameof(EndFrame));
            SWave = s.Serialize<byte>(SWave, name: nameof(SWave));
            PtclCount = s.Serialize<byte>(PtclCount, name: nameof(PtclCount));
            Ptcl = s.Serialize<byte>(Ptcl, name: nameof(Ptcl));
            s.SerializePadding(5);
        }
    }
}