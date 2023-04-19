namespace BinarySerializer.Klonoa.LV {
    public class SFZPack_ArchiveFile : ArchiveFile {
        public SFXPart[] Pre_Parts { get; set; }

        public SFZ_File[] MorphTargets { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            MorphTargets ??= new SFZ_File[OffsetTable.FilesCount];
            for (int i = 0; i < OffsetTable.FilesCount; i++)
                MorphTargets[i] = SerializeFile(s, MorphTargets[i], i, onPreSerialize: x => x.Pre_Parts = Pre_Parts, name: $"{nameof(MorphTargets)}[{i}]");
        }
    }
}