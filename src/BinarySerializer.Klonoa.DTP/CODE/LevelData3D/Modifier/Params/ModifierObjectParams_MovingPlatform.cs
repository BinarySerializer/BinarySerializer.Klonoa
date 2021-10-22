namespace BinarySerializer.Klonoa.DTP
{
    public class ModifierObjectParams_MovingPlatform : BinarySerializable
    {
        public uint Flags { get; set; }
        public Pointer MovementPathIndicesPointer_0 { get; set; }
        public Pointer MovementPathIndicesPointer_1 { get; set; }
        public FixedPointInt32 AnimSpeed { get; set; }

        public int[] MovementPathIndices_0 { get; set; }
        public int[] MovementPathIndices_1 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Flags = s.Serialize<uint>(Flags, name: nameof(Flags));
            MovementPathIndicesPointer_0 = s.SerializePointer(MovementPathIndicesPointer_0, name: nameof(MovementPathIndicesPointer_0));
            MovementPathIndicesPointer_1 = s.SerializePointer(MovementPathIndicesPointer_1, name: nameof(MovementPathIndicesPointer_1));
            AnimSpeed = s.SerializeObject<FixedPointInt32>(AnimSpeed, x => x.Pre_PointPosition = 0x0C, name: nameof(AnimSpeed));

            s.DoAt(MovementPathIndicesPointer_0, () => 
                MovementPathIndices_0 = s.SerializeArrayUntil<int>(MovementPathIndices_0, x => x == -1, () => -1, name: nameof(MovementPathIndices_0)));
            s.DoAt(MovementPathIndicesPointer_1, () => 
                MovementPathIndices_1 = s.SerializeArrayUntil<int>(MovementPathIndices_1, x => x == -1, () => -1, name: nameof(MovementPathIndices_1)));
        }
    }
}