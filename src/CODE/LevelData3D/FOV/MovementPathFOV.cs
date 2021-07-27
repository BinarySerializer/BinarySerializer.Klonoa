namespace BinarySerializer.KlonoaDTP
{
    public class MovementPathFOV : BinarySerializable
    {
        public FOV[] FOVs { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FOVs = s.SerializeObjectArrayUntil<FOV>(FOVs, x => x.MovementPathPosition == -1 || x.MovementPathPosition == -2, name: nameof(FOVs));
        }
    }
}