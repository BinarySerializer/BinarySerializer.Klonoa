namespace BinarySerializer.KlonoaDTP
{
    public class ScenerySprites_File : BaseFile
    {
        public short EntriesCount { get; set; }
        public short Short_02 { get; set; }
        public Entry[] Entries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            EntriesCount = s.Serialize<short>(EntriesCount, name: nameof(EntriesCount));
            Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
            Entries = s.SerializeObjectArray<Entry>(Entries, EntriesCount, name: nameof(Entries));
        }

        public class Entry : BinarySerializable
        {
            public short Short_00 { get; set; }
            public short Short_02 { get; set; }
            public short Short_04 { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
                Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
                Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
            }
        }
    }
}