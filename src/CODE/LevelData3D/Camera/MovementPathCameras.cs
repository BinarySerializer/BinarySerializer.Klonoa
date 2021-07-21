namespace BinarySerializer.KlonoaDTP
{
    public class MovementPathCameras : BinarySerializable
    {
        public int Pre_SectorsCount { get; set; }

        public MovementPathCamera[] FixedEntries { get; set; } // What is this for?
        public SectorMovementPathCameras[][] SectorEntries { get; set; } // One for each sector

        public override void SerializeImpl(SerializerObject s)
        {
            FixedEntries = s.SerializeObjectArrayUntil<MovementPathCamera>(FixedEntries, x => x.Short_04 == -1, name: nameof(FixedEntries));

            SectorEntries ??= new SectorMovementPathCameras[Pre_SectorsCount][];

            for (int i = 0; i < SectorEntries.Length; i++)
                SectorEntries[i] = s.SerializeObjectArrayUntil(SectorEntries[i], x => x.PathCameras[0].Short_04 == -1, name: $"{nameof(SectorEntries)}[{i}]");
        }
    }
}