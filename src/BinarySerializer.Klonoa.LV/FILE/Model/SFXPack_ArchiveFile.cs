namespace BinarySerializer.Klonoa.LV {
    public class SFXPack_ArchiveFile : ArchiveFile {
        public SFX_File SFX { get; set; }
        public ArchiveFile<GMS_File> GMSPack { get; set; }
        public SFZPack_ArchiveFile SFZPack { get; set; }
        public ACTPack_ArchiveFile AnimationPack { get; set; }
        public SFXOutlineSpecularData_File OutlineSpecularData { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            SFX = SerializeFile(s, SFX, 0, name: nameof(SFX));
            GMSPack = SerializeFile(s, GMSPack, 1, name: nameof(GMSPack));
            SFZPack = SerializeFile(s, SFZPack, 2, onPreSerialize: x => x.Pre_Parts = SFX.Parts, name: nameof(SFZPack));
            AnimationPack = SerializeFile(s, AnimationPack, 3, name: nameof(AnimationPack));
            if (OffsetTable.FilesCount > 4)
                OutlineSpecularData = SerializeFile(s, OutlineSpecularData, 4, name: nameof(OutlineSpecularData));
        }
    }
}