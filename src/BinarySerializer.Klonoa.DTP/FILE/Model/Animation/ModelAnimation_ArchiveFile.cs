﻿namespace BinarySerializer.Klonoa.DTP
{
    public class ModelAnimation_ArchiveFile : ArchiveFile
    {
        public bool Pre_UsesInfo { get; set; } // If the info is not used then the info is prepended to each file

        public RawData_File File_0 { get; set; }
        public ModelAnimationInfo_File Info { get; set; }
        
        public VectorAnimation_File Rotations { get; set; }
        public VectorAnimation_File Positions { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            if (Pre_UsesInfo)
                Info = SerializeFile<ModelAnimationInfo_File>(s, Info, 0, name: nameof(Info));
            else
                File_0 = SerializeFile<RawData_File>(s, File_0, 0, name: nameof(File_0));
            
            Rotations = SerializeFile<VectorAnimation_File>(s, Rotations, 1, onPreSerialize: x =>
            {
                x.Pre_ObjectsCount = Info?.ObjectsCount;
                x.Pre_FramesCount = Info?.FramesCount;
            }, name: nameof(Rotations));
            Positions = SerializeFile<VectorAnimation_File>(s, Positions, 2, onPreSerialize: x =>
            {
                x.Pre_ObjectsCount = Info?.ObjectsCount;
                x.Pre_FramesCount = Info?.FramesCount;
            }, name: nameof(Positions));
        }
    }
}