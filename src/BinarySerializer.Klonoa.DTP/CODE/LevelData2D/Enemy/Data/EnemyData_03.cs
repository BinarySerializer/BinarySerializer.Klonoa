namespace BinarySerializer.Klonoa.DTP;

public class EnemyData_03 : BaseEnemyData
{
    public byte[] Bytes_08 { get; set; }

    protected override void SerializeData(SerializerObject s)
    {
        Bytes_08 = s.SerializeArray<byte>(Bytes_08, 32, name: nameof(Bytes_08));
    }
}