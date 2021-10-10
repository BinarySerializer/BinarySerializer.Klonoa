namespace BinarySerializer.Klonoa.LV {
    public class Model_ArchiveFile : ArchiveFile {
        public ModelGeometry_File ModelGeometry { get; set; }
        public ArchiveFile<GSTextures_File> ModelTextures { get; set; }
        public ArchiveFile<ModelMorphTarget_File> ModelMorphTargets { get; set; }
        public ModelAnimData_ArchiveFile ModelAnimData { get; set; }
        public ModelDescriptor_File ModelDescriptor { get; set; } // Includes model name (3 characters long)

        protected override void SerializeFiles(SerializerObject s)
        {
            ModelGeometry = SerializeFile(s, ModelGeometry, 0, name: nameof(ModelGeometry));
            ModelTextures = SerializeFile(s, ModelTextures, 1, name: nameof(ModelTextures));
            ModelMorphTargets = SerializeFile(s, ModelMorphTargets, 2, name: nameof(ModelMorphTargets));
            ModelAnimData = SerializeFile(s, ModelAnimData, 3, name: nameof(ModelAnimData));
            ModelDescriptor = SerializeFile(s, ModelDescriptor, 4, name: nameof(ModelDescriptor));
        }
    }
}