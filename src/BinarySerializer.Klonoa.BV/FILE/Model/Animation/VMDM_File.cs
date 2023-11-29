namespace BinarySerializer.Klonoa.BV
{
    public class VMDM_File : BaseFile
    {
        #region Header
        public string ID { get; set; }
        public ushort Ushort_0C { get; set; }
        public ushort Ushort_0E { get; set; }
        public ushort AnimationCount { get; set; }
        public ushort StringCount { get; set; }
        public ushort KeyframeCount { get; set; }
        public ushort VectorCount { get; set; }
        #endregion

        #region Buffers
        public VMDM_Animation[] Animations { get; set; }
        public string[] Strings { get; set; }
        public VMDM_Keyframe[] Keyframes { get; set; }
        public VMDM_Vector[] Vectors { get; set; }
        #endregion

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("VMDM", 4);
            s.SerializePadding(4, logIfNotNull: true);
            ID = s.SerializeString(ID, 2, name: nameof(ID));
            s.SerializePadding(2, logIfNotNull: true);
            Ushort_0C = s.Serialize<ushort>(Ushort_0C, name: nameof(Ushort_0C));
            Ushort_0E = s.Serialize<ushort>(Ushort_0E, name: nameof(Ushort_0E));
            s.SerializePadding(24, logIfNotNull: true);
            AnimationCount = s.Serialize<ushort>(AnimationCount, name: nameof(AnimationCount));
            StringCount = s.Serialize<ushort>(StringCount, name: nameof(StringCount));
            KeyframeCount = s.Serialize<ushort>(KeyframeCount, name: nameof(KeyframeCount));
            VectorCount = s.Serialize<ushort>(VectorCount, name: nameof(VectorCount));
            s.SerializePadding(16, logIfNotNull: true);

            Animations = s.SerializeObjectArray<VMDM_Animation>(Animations, AnimationCount, name: nameof(Animations));
            Strings ??= new string[StringCount];
            for (int i = 0; i < StringCount; i++) {
                s.SerializePadding(4, logIfNotNull: true);
                Strings[i] = s.SerializeString(Strings[i], 12, name: $"{nameof(Strings)}[{i}]");
            }
            Keyframes = s.SerializeObjectArray<VMDM_Keyframe>(Keyframes, KeyframeCount, name: nameof(Keyframes));
            Vectors = s.SerializeObjectArray<VMDM_Vector>(Vectors, VectorCount, name: nameof(Vectors));
        }
    }
}