namespace BinarySerializer.KlonoaDTP
{
    public class ObjTransform_ArchiveFile : ArchiveFile
    {
        public ObjUnknown_File File_0 { get; set; } // TODO: What is this?
        public ObjRotation_File Rotation { get; set; }
        public ObjPosition_File Position { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<ObjUnknown_File>(s, File_0, 0, name: nameof(File_0));
            Rotation = SerializeFile<ObjRotation_File>(s, Rotation, 1, name: nameof(Rotation));
            Position = SerializeFile<ObjPosition_File>(s, Position, 2, name: nameof(Position));
        }
    }
}