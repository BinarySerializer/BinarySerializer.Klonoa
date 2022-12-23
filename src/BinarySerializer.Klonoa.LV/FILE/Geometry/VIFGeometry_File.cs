namespace BinarySerializer.Klonoa.LV
{
    public class VIFGeometry_File : BaseFile
    {
        public uint Type;
        public uint SectionCount;
        public VIFGeometry_Section[] Sections;

        public override void SerializeImpl(SerializerObject s)
        {
            Type = s.Serialize<uint>(Type, name: nameof(Type));
            SectionCount = s.Serialize<uint>(SectionCount, name: nameof(SectionCount));
            s.SerializePadding(8);
            Sections = s.SerializeObjectArray<VIFGeometry_Section>(Sections, SectionCount * 2, name: nameof(Sections), onPreSerialize: x => x.Pre_Offset = Offset);
        }
    }
}