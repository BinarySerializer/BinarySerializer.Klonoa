namespace BinarySerializer.Klonoa.DTP
{
    public class MovementPathCameras : BinarySerializable
    {
        public int Pre_SectorsCount { get; set; }

        public MovementPathCamera[] FixedEntries { get; set; } // What is this for?
        public MovementPathCamera[][] PathCameras { get; set; } // One for each sector and then one for each movement path

        public override void SerializeImpl(SerializerObject s)
        {
            FixedEntries = s.SerializeObjectArrayUntil<MovementPathCamera>(FixedEntries, x => x.Short_04 == -1, name: nameof(FixedEntries));

            PathCameras ??= new MovementPathCamera[Pre_SectorsCount][];

            for (int i = 0; i < PathCameras.Length; i++)
                PathCameras[i] = s.SerializeObjectArrayUntil(PathCameras[i], x => x.Short_04 == -1, name: $"{nameof(PathCameras)}[{i}]");
        }
    }
}