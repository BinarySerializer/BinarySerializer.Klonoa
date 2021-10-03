namespace BinarySerializer.Klonoa.DTP
{
    public class CameraAnimation : BinarySerializable
    {
        public ushort Frame { get; set; } // On which absolute frame this should be
        public ushort FrameOffset { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Frame = s.Serialize<ushort>(Frame, name: nameof(Frame));
            FrameOffset = s.Serialize<ushort>(FrameOffset, name: nameof(FrameOffset));
        }
    }
}