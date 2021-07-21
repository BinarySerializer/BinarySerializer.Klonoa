namespace BinarySerializer.KlonoaDTP
{
    public class SectorMovementPathCameras : BinarySerializable
    {
        public MovementPathCamera[] PathCameras { get; set; } // One per movement path in the sector

        public override void SerializeImpl(SerializerObject s)
        {
            PathCameras = s.SerializeObjectArrayUntil<MovementPathCamera>(PathCameras, x => x.Int_00 == -1, name: nameof(PathCameras));
        }
    }
}