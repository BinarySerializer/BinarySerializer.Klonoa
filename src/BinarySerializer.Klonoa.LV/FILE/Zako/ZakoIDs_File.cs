namespace BinarySerializer.Klonoa.LV
{
    public class ZakoIDs_File : BaseFile
    {
        public int[] IDs { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            IDs = s.SerializeArray<int>(IDs, 16, name: nameof(IDs));
        }
    }
}