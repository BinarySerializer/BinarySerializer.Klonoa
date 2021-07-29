namespace BinarySerializer.KlonoaDTP
{
    public class ObjTransform_ArchiveFile : ArchiveFile
    {
        public uint Pre_ObjsCount { get; set; }

        public RawData_File File_0 { get; set; } // TODO: What is this?
        public ObjRotations_File Rotations { get; set; }
        public ObjPositions_File Positions { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            File_0 = SerializeFile<RawData_File>(s, File_0, 0, name: nameof(File_0));
            Rotations = SerializeFile<ObjRotations_File>(s, Rotations, 1, onPreSerialize: x => x.Pre_OverrideCount = Pre_ObjsCount, name: nameof(Rotations));
            Positions = SerializeFile<ObjPositions_File>(s, Positions, 2, onPreSerialize: x => x.Pre_OverrideCount = Pre_ObjsCount, name: nameof(Positions));
        }
    }
}