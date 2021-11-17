namespace BinarySerializer.Klonoa.DTP;

public class EnemyData_09 : BaseEnemyData
{
    public short Short_08 { get; set; }
    public byte[] Bytes_0A { get; set; }

    protected override void SerializeData(SerializerObject s)
    {
        Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
        Bytes_0A = s.SerializeArray<byte>(Bytes_0A, 14, name: nameof(Bytes_0A));
    }
}