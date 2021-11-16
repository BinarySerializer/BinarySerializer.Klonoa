namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_MoveCamera : BaseCutsceneInstructionData
    {
        public bool Animate { get; set; } // If false it goes to directly

        public KlonoaVector16 Position { get; set; }
        public short RotX { get; set; }
        public short RotY { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Animate = s.Serialize<bool>(Animate, name: nameof(Animate));
            s.SerializePadding(1, logIfNotNull: false);
            ParamsBufferOffset = s.Serialize<uint>(ParamsBufferOffset, name: nameof(ParamsBufferOffset));
            DoAtParams(s, () =>
            {
                Position = s.SerializeObject<KlonoaVector16>(Position, name: nameof(Position));
                RotX = s.Serialize<short>(RotX, name: nameof(RotX));
                RotY = s.Serialize<short>(RotY, name: nameof(RotY));
            });
        }
    }
}