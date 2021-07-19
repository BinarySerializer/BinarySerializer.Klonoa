namespace BinarySerializer.KlonoaDTP
{
    public class Object3DTransform_ArchiveFile : ArchiveFile
    {
        public Object3DUnknown_File File_0 { get; set; } // TODO: What is this?
        public Object3DRotation_File Rotation { get; set; }
        public Object3DPosition_File Position { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<Object3DUnknown_File>(s, File_0, 0, name: nameof(File_0));
            Rotation = SerializeFile<Object3DRotation_File>(s, Rotation, 1, name: nameof(Rotation));
            Position = SerializeFile<Object3DPosition_File>(s, Position, 2, name: nameof(Position));
        }
    }
}