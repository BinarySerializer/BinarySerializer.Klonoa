namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_CreateGameObject_4 : BaseCutsceneInstructionData
    {
        public byte SecondaryType { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SecondaryType = s.Serialize<byte>(SecondaryType, name: nameof(SecondaryType));
            s.SerializePadding(5, logIfNotNull: false);
        }
    }
}