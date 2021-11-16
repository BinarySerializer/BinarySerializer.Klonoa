namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_12 : BaseCutsceneInstructionData
    {
        public byte Byte_00 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Byte_00 = s.Serialize<byte>(Byte_00, name: nameof(Byte_00));
            s.SerializePadding(5, logIfNotNull: false);
        }
    }
}