namespace BinarySerializer.Klonoa.DTP;

public class EnemyMovementData : BinarySerializable
{
    public short Short_00 { get; set; }
    public short Short_02 { get; set; }
    public short Short_04 { get; set; }
    public short Short_06 { get; set; }
    public int Int_08 { get; set; }
    public short Short_0C { get; set; }
    public short MovementPathIndex { get; set; } // If -2 use the current one
    public int MovementPathDistance { get; set; }

    public override void SerializeImpl(SerializerObject s)
    {
        Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
        Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
        Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
        Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
        Int_08 = s.Serialize<int>(Int_08, name: nameof(Int_08));
        Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
        MovementPathIndex = s.Serialize<short>(MovementPathIndex, name: nameof(MovementPathIndex));
        MovementPathDistance = s.Serialize<int>(MovementPathDistance, name: nameof(MovementPathDistance));
    }
}