namespace BinarySerializer.Klonoa.DTP
{
    public class ObjTransform_ArchiveFile : ArchiveFile
    {
        public bool Pre_UsesTransformInfo { get; set; } // If the transform info is not used then the info is prepended to each file

        public RawData_File File_0 { get; set; }
        public ObjTransformInfo_File Info { get; set; }
        
        public ObjRotations_File Rotations { get; set; }
        public ObjPositions_File Positions { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            if (Pre_UsesTransformInfo)
                Info = SerializeFile<ObjTransformInfo_File>(s, Info, 0, name: nameof(Info));
            else
                File_0 = SerializeFile<RawData_File>(s, File_0, 0, name: nameof(File_0));
            
            Rotations = SerializeFile<ObjRotations_File>(s, Rotations, 1, onPreSerialize: x => x.Pre_Info = Info, name: nameof(Rotations));
            Positions = SerializeFile<ObjPositions_File>(s, Positions, 2, onPreSerialize: x => x.Pre_Info = Info, name: nameof(Positions));
        }
    }
}