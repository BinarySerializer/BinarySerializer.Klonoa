namespace BinarySerializer.Klonoa.LV
{
    public class HeadPack_ArchiveFile : ArchiveFile
    {
        public bool Pre_HasMultipleLanguages { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // If no size has been specified we use the size of the current file
            if (Pre_FileSize == 0)
                Pre_FileSize = s.CurrentLength;

            // Serialize archive
            base.SerializeImpl(s);
        }

        public BINHeaders_ArchiveFile KLDATA_Multi { get; set; }
        public BINHeader_File KLDATA_Single { get; set; }
        public BINHeader_BGM_File BGMPACK { get; set; }
        public BINHeader_File PPTPACK { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            if (Pre_HasMultipleLanguages)
                KLDATA_Multi = SerializeFile<BINHeaders_ArchiveFile>(s, KLDATA_Multi, 0, name: nameof(KLDATA_Multi));
            else
                KLDATA_Single = SerializeFile<BINHeader_File>(s, KLDATA_Single, 0, name: nameof(KLDATA_Single));

            BGMPACK = SerializeFile<BINHeader_BGM_File>(s, BGMPACK, 1, name: nameof(BGMPACK));
            PPTPACK = SerializeFile<BINHeader_File>(s, PPTPACK, 2, name: nameof(PPTPACK));
        }
    }
}