namespace BinarySerializer.KlonoaDTP
{
    public class MovementPathFOVs : BinarySerializable
    {
        public MovementPathFOV[] PathFOVs { get; set; } // One per movement path in the sector

        public override void SerializeImpl(SerializerObject s)
        {
            PathFOVs = s.SerializeObjectArrayUntil<MovementPathFOV>(PathFOVs, x => x.FOVs[0].Short_06 == -1, name: nameof(PathFOVs));
        }
    }
}