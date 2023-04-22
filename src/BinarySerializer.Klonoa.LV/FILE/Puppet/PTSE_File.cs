namespace BinarySerializer.Klonoa.LV
{
    public class PTSE_File : BaseFile
    {
        public uint IDCount { get; set; }
        public uint[] IDs { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            IDCount = s.Serialize<uint>(IDCount, name: nameof(IDCount));
            IDs = s.SerializeArray<uint>(IDs, IDCount, name: nameof(IDs));
        }
    }
}