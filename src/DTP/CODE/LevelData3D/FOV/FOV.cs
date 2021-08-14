namespace BinarySerializer.Klonoa.DTP
{
    public class FOV : BinarySerializable
    {
        public int MovementPathPosition { get; set; } // How far along the path Klonoa is

        // Determines the current field of view
        public short Short_04 { get; set; }
        public short Short_06 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            MovementPathPosition = s.Serialize<int>(MovementPathPosition, name: nameof(MovementPathPosition));
            Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
            Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
        }
    }
}