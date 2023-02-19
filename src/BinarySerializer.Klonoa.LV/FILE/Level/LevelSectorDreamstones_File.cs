namespace BinarySerializer.Klonoa.LV
{
    public class LevelSectorDreamstones_File : BaseFile
    {
        public uint DreamstoneCount { get; set; }
        public uint DreamstonesSize { get; set; }
        public uint FlagsSize { get; set; }
        public uint DisplayFlagsSize { get; set; }

        public DreamstoneData[] Dreamstones { get; set; }
        public int[] Flags { get; set; }
        public int[] DisplayFlags { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Header
            DreamstoneCount = s.Serialize<uint>(DreamstoneCount, name: nameof(DreamstoneCount));
            DreamstonesSize = s.Serialize<uint>(DreamstonesSize, name: nameof(DreamstonesSize));
            FlagsSize = s.Serialize<uint>(FlagsSize, name: nameof(FlagsSize));
            DisplayFlagsSize = s.Serialize<uint>(DisplayFlagsSize, name: nameof(DisplayFlagsSize));

            // Data
            Dreamstones = s.SerializeObjectArray<DreamstoneData>(Dreamstones, DreamstoneCount, name: nameof(Dreamstones));
            Flags = s.SerializeArray<int>(Flags, 9, name: nameof(Flags));
            DisplayFlags = s.SerializeArray<int>(DisplayFlags, DreamstoneCount, name: nameof(DisplayFlags)); // The game uses an array size of 33, but that would go outside of the file...
        }

        public class DreamstoneData : BinarySerializable
        {
            public KlonoaLV_FloatVector Position { get; set; }
            public KlonoaLV_FloatVector Rotation { get; set; } // Radians

            public override void SerializeImpl(SerializerObject s)
            {
                Position = s.SerializeObject<KlonoaLV_FloatVector>(Position, onPreSerialize: x => x.Pre_HasW = true, name: nameof(Position));
                Rotation = s.SerializeObject<KlonoaLV_FloatVector>(Rotation, onPreSerialize: x => x.Pre_HasW = true, name: nameof(Rotation));
            }
        }
    }
}