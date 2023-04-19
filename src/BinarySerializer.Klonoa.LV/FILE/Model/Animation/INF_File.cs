namespace BinarySerializer.Klonoa.LV {
    public class INF_File : BaseFile {
        public int JointCount { get; set; }
        public Pointer LocalJointsPointer { get; set; }
        public Pointer GlobalJointsPointer { get; set; }
        public short[] ParentJoints { get; set; } // The index of the parent joint for each joint. -1 means no parent joint (root joint).
        public KlonoaLV_FloatVector[] LocalJointPositions { get; set; }
        public KlonoaLV_FloatVector[] GlobalJointPositions { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            JointCount = s.Serialize<int>(JointCount, name: nameof(JointCount));
            LocalJointsPointer = s.SerializePointer(LocalJointsPointer, anchor: Offset, name: nameof(LocalJointsPointer));
            GlobalJointsPointer = s.SerializePointer(GlobalJointsPointer, anchor: Offset, name: nameof(GlobalJointsPointer));
            ParentJoints = s.SerializeArray<short>(ParentJoints, JointCount, name: nameof(ParentJoints));
            s.DoAt(LocalJointsPointer, () => LocalJointPositions = s.SerializeObjectArray<KlonoaLV_FloatVector>(LocalJointPositions, JointCount, 
                onPreSerialize: _ => s.Align(16), name: nameof(LocalJointPositions)));
            s.DoAt(GlobalJointsPointer, () => GlobalJointPositions = s.SerializeObjectArray<KlonoaLV_FloatVector>(GlobalJointPositions, JointCount, 
                onPreSerialize: _ => s.Align(16), name: nameof(GlobalJointPositions)));
            s.Goto(Offset + Pre_FileSize);
        }
    }
}