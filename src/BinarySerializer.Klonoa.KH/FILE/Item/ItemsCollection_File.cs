namespace BinarySerializer.Klonoa.KH
{
    public class ItemsCollection_File : BaseFile
    {
        public int Pre_AdditionalCount { get; set; } // For unused key items

        public ushort Count { get; set; }
        public ushort Ushort_02 { get; set; } // Always same as count
        public Item[] Items { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Count = s.Serialize<ushort>(Count, name: nameof(Count));
            Ushort_02 = s.Serialize<ushort>(Ushort_02, name: nameof(Ushort_02));
            Items = s.SerializeObjectArray<Item>(Items, Count + Pre_AdditionalCount, name: nameof(Items));
        }
    }
}