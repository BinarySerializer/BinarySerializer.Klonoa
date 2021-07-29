namespace BinarySerializer.KlonoaDTP
{
    public class ObjRotations_File : BaseFile
    {
        public uint? Pre_OverrideCount { get; set; }

        public ushort Ushort_00 { get; set; } // Objects count?
        public ushort RotationsCount { get; set; }
        public ObjRotation[] Rotations { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            uint count;

            if (Pre_OverrideCount == null)
            {
                Ushort_00 = s.Serialize<ushort>(Ushort_00, name: nameof(Ushort_00));

                if (Ushort_00 != 1)
                    s.LogWarning($"Unknown value for {nameof(Ushort_00)} in {nameof(ObjPositions_File)}");

                RotationsCount = s.Serialize<ushort>(RotationsCount, name: nameof(RotationsCount));
                count = RotationsCount;
            }
            else
            {
                count = Pre_OverrideCount.Value;
            }

            Rotations = s.SerializeObjectArray<ObjRotation>(Rotations, count, name: nameof(Rotations));
        }
    }
}