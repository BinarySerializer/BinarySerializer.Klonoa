namespace BinarySerializer.Klonoa.LV {
    public class ModelMorphTargets_ArchiveFile : ArchiveFile {
        public ModelMesh[] Pre_Meshes { get; set; }

        public ModelMorphTarget_File[] ModelMorphTargets { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            ModelMorphTargets ??= new ModelMorphTarget_File[OffsetTable.FilesCount];
            for (int i = 0; i < OffsetTable.FilesCount; i++)
                ModelMorphTargets[i] = SerializeFile(s, ModelMorphTargets[i], i, onPreSerialize: x => x.Pre_Meshes = Pre_Meshes, name: $"{nameof(ModelMorphTargets)}[{i}]");
        }
    }
}