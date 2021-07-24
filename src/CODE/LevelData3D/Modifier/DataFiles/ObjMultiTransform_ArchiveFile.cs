namespace BinarySerializer.KlonoaDTP
{
    public class ObjMultiTransform_ArchiveFile : ArchiveFile
    {
        public RawData_File File_0 { get; set; }
        public ObjRotations_File Rotations { get; set; }
        public ObjPositions_File Positions { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<RawData_File>(s, File_0, 0, name: nameof(File_0));
            Rotations = SerializeFile<ObjRotations_File>(s, Rotations, 1, name: nameof(Rotations));
            Positions = SerializeFile<ObjPositions_File>(s, Positions, 2, name: nameof(Positions));
        }
    }
}