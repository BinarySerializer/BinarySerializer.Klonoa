namespace BinarySerializer.KlonoaDTP
{
    public class ObjRotations_File : BaseFile
    {
        public ushort Ushort_00 { get; set; } // Objects count?
        public ushort RotationsCount { get; set; }
        public ObjRotation[] Rotations { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ushort_00 = s.Serialize<ushort>(Ushort_00, name: nameof(Ushort_00));

            if (Ushort_00 != 1)
                s.LogWarning($"Unknown value for {nameof(Ushort_00)} in {nameof(ObjPositions_File)}");

            RotationsCount = s.Serialize<ushort>(RotationsCount, name: nameof(RotationsCount));

            Rotations = s.SerializeObjectArray<ObjRotation>(Rotations, RotationsCount, name: nameof(Rotations));
        }
    }
}