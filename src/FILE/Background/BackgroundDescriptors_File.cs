namespace BinarySerializer.KlonoaDTP
{
    public class BackgroundDescriptors_File : BaseFile
    {
        public int BackgroundsCount { get; set; } // The number of backgrounds to show for this sector
        public BackgroundDescriptor[] Descriptors { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            BackgroundsCount = s.Serialize<int>(BackgroundsCount, name: nameof(BackgroundsCount));
            Descriptors = s.SerializeObjectArray<BackgroundDescriptor>(Descriptors, BackgroundsCount, name: nameof(Descriptors));
        }
    }
}