namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// A 3D model animation, mainly used for bosses
    /// </summary>
    public class ModelAnimation : ArchiveFile
    {
        public RawData_File File_0 { get; set; }
        public ModelAnimationKeyFrames_File VertexKeyFrames { get; set; }
        public ObjPositions_File Normals { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<RawData_File>(s, File_0, 0, name: nameof(File_0));
            VertexKeyFrames = SerializeFile<ModelAnimationKeyFrames_File>(s, VertexKeyFrames, 1, name: nameof(VertexKeyFrames));
            Normals = SerializeFile<ObjPositions_File>(s, Normals, 2, name: nameof(Normals));
        }
    }
}