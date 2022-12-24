namespace BinarySerializer.Klonoa.LV {
    public class Model_ArchiveFile : ArchiveFile {
        public ModelGeometry_File Geometry { get; set; }
        public ArchiveFile<GSTextures_File> Textures { get; set; }
        public ModelMorphTargets_ArchiveFile MorphTargets { get; set; }
        public ModelAnimationData_ArchiveFile AnimationData { get; set; }
        public ModelDescriptor_File Descriptor { get; set; } // Includes model name (3 characters long)

        protected override void SerializeFiles(SerializerObject s)
        {
            Geometry = SerializeFile(s, Geometry, 0, name: nameof(Geometry));
            Textures = SerializeFile(s, Textures, 1, name: nameof(Textures));
            MorphTargets = SerializeFile(s, MorphTargets, 2, onPreSerialize: x => x.Pre_Meshes = Geometry.Meshes, name: nameof(MorphTargets));
            AnimationData = SerializeFile(s, AnimationData, 3, name: nameof(AnimationData));
            Descriptor = SerializeFile(s, Descriptor, 4, name: nameof(Descriptor));
        }
    }
}