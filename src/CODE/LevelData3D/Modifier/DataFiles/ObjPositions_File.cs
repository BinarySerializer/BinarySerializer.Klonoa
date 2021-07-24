namespace BinarySerializer.KlonoaDTP
{
    public class ObjPositions_File : BaseFile
    {
        public ushort Ushort_00 { get; set; } // Objects count?
        public ushort PositionsCount { get; set; }
        public ObjPosition[] Positions { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Ushort_00 = s.Serialize<ushort>(Ushort_00, name: nameof(Ushort_00));

            if (Ushort_00 != 1)
                s.LogWarning($"Unknown value for {nameof(Ushort_00)} in {nameof(ObjPositions_File)}");

            PositionsCount = s.Serialize<ushort>(PositionsCount, name: nameof(PositionsCount));

            Positions = s.SerializeObjectArray<ObjPosition>(Positions, PositionsCount, name: nameof(Positions));
        }
    }
}