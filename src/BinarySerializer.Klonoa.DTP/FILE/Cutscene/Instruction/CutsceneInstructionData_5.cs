namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_5 : BaseCutsceneInstructionData
    {
        public sbyte Byte_00 { get; set; } // 3 and 4 are special cases

        public override void SerializeImpl(SerializerObject s)
        {
            Byte_00 = s.Serialize<sbyte>(Byte_00, name: nameof(Byte_00));
            s.SerializePadding(5, logIfNotNull: false);
        }
    }
}