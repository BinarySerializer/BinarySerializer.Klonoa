﻿namespace BinarySerializer.Klonoa.DTP;

public class EnemyData_04 : BaseEnemyData
{
    public short Short_08 { get; set; }
    public byte[] Bytes_0A { get; set; }

    // What are these for?
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }
    public int OffsetZ { get; set; }

    public byte[] Bytes_1C { get; set; }

    protected override void SerializeData(SerializerObject s)
    {
        Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
        Bytes_0A = s.SerializeArray<byte>(Bytes_0A, 6, name: nameof(Bytes_0A));
        OffsetX = s.Serialize<int>(OffsetX, name: nameof(OffsetX));
        OffsetY = s.Serialize<int>(OffsetY, name: nameof(OffsetY));
        OffsetZ = s.Serialize<int>(OffsetZ, name: nameof(OffsetZ));
        Bytes_1C = s.SerializeArray<byte>(Bytes_1C, 4, name: nameof(Bytes_1C));
    }
}