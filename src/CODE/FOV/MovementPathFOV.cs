namespace BinarySerializer.KlonoaDTP
{
    public class MovementPathFOV : BinarySerializable
    {
        public FOV[] FOVs { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FOVs = s.SerializeObjectArrayUntil<FOV>(FOVs, x => x.PathOffset == -1 || x.PathOffset == -2, name: nameof(FOVs));
        }
    }
}