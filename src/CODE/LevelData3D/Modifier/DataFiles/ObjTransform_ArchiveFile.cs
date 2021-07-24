namespace BinarySerializer.KlonoaDTP
{
    public class ObjTransform_ArchiveFile : ArchiveFile
    {
        public RawData_File File_0 { get; set; } // TODO: What is this?
        public ObjRotation Rotation { get; set; }
        public ObjPosition Position { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<RawData_File>(s, File_0, 0, name: nameof(File_0));
            Rotation = SerializeFile<ObjRotation>(s, Rotation, 1, name: nameof(Rotation));
            Position = SerializeFile<ObjPosition>(s, Position, 2, name: nameof(Position));
        }
    }
}