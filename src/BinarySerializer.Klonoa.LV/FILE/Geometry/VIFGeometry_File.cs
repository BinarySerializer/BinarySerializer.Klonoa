namespace BinarySerializer.Klonoa.LV
{
    public class VIFGeometry_File : BaseFile
    {
        /// <summary>
        /// Decides how to parse this file.
        /// If 0: SectionCount is multiplied by 2 when parsing sections.
        /// If 1: SectionCount is equal to one.
        /// </summary>
        public uint Type;
        public uint SectionCount;
        public VIFGeometry_Section[] Sections;

        public override void SerializeImpl(SerializerObject s)
        {
            Type = s.Serialize<uint>(Type, name: nameof(Type));
            if (Type == 0) {
                SectionCount = s.Serialize<uint>(SectionCount, name: nameof(SectionCount));
            } else {
                SectionCount = 1;
                s.SerializePadding(4, logIfNotNull: true);
            }
            s.SerializePadding(8);
            Sections = s.SerializeObjectArray<VIFGeometry_Section>(Sections, SectionCount * (Type == 0 ? 2 : 1), 
                onPreSerialize: x => x.Pre_Offset = Offset, name: nameof(Sections));
        }
    }
}