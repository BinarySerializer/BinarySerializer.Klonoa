namespace BinarySerializer.Klonoa.LV
{
    public class LevelGeometry_File : BaseFile
    {
        public uint SectionCount; // Needs to be multiplied by 2; half of the sections are empty
        public CommandSection[] Sections;

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializePadding(4);
            SectionCount = s.Serialize<uint>(SectionCount, name: nameof(SectionCount));
            s.SerializePadding(8);
            Sections = s.SerializeObjectArray<CommandSection>(Sections, SectionCount * 2, name: nameof(Sections), onPreSerialize: x => x.Pre_Offset = Offset);
        }
    }
}