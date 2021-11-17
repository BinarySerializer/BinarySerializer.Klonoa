namespace BinarySerializer.Klonoa.DTP
{
    public class EnemyData_17 : BaseEnemyData
    {
        public byte[] Bytes_08 { get; set; }

        protected override void SerializeData(SerializerObject s)
        {
            Bytes_08 = s.SerializeArray<byte>(Bytes_08, 0x24 - 8, name: nameof(Bytes_08));
        }
    }
}