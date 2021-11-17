namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_MoveCamera : BaseCutsceneInstructionData
    {
        public byte Time { get; set; } // If 0 it goes to position directly

        public KlonoaVector16 Position { get; set; }
        public short RotX { get; set; }
        public short RotY { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Time = s.Serialize<byte>(Time, name: nameof(Time));
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