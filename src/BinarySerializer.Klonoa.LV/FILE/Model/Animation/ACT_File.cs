namespace BinarySerializer.Klonoa.LV
{
    public class ACT_File : BaseFile
    {
        #region Common Fields
        /// <summary>
        /// The number of joints for this animation.
        /// This should always equal the number of joints in BoneData.
        /// </summary>
        public ushort JointCount { get; set; }

        /// <summary>
        /// The number of frames in this animation.
        /// All animations run at 60 frames per second.
        /// </summary>
        public ushort FrameCount { get; set; }

        /// <summary>
        /// The number of joints being used for rotation animations.
        /// Only used in fixed animations.
        /// </summary>
        public ushort RotationCount { get; set; }

        /// <summary>
        /// The number of joints being used for translation animations.
        /// Only used in fixed animations.
        /// </summary>
        public ushort TranslationCount { get; set; }

        /// <summary>
        /// The name of this animation.
        /// </summary>
        public string Name {get; set; }

        /// <summary>
        /// Unknown value.
        /// </summary>
        public uint UInt_10 { get; set; }

        /// <summary>
        /// Pointer to morph animation data.
        /// Null pointer if there is no morph animation.
        /// </summary>
        public Pointer MorphAnimationPointer { get; set; } // Null pointer if there are no morph keyframes

        /// <summary>
        /// The name of the animation.
        /// </summary>
        public Pointer KeyframesPointer { get; set; } // Where the keyframe data starts

        /// <summary>
        /// The joints to animate in translation keyframes.
        /// Only used in fixed animations.
        /// Length: JointCount
        /// </summary>
        public bool[] TranslationJoints { get; set; }
        
        /// <summary>
        /// The base position of the animation.
        /// </summary>
        public KlonoaLV_FloatVector Position { get; set; }

        /// <summary>
        /// The scalar value for all translation vectors.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Where the animation data actually starts (either translation keyframes or morph keyframes).
        /// </summary>
        public Pointer DataPointer { get; set; }

        /// <summary>
        /// Unknown value, seems to always be less than the size of the file.
        /// </summary>
        public uint UInt_44 { get; set; }

        /// <summary>
        /// Whether this animation should loop (either 0 or 1).
        /// </summary>
        public uint LoopFlag { get; set; }

        /// <summary>
        /// Number of frames to transition into the animation (might not be used).
        /// </summary>
        public ushort TransitionIn { get; set; }

        /// <summary>
        /// Number of frames to transition out of the animation
        /// </summary>
        public ushort TransitionOut { get; set; }
        
        /// <summary>
        /// Unknown value, always 1.
        /// </summary>
        public ushort UShort_50 { get; set; }

        /// <summary>
        /// Unknown value, always 1.
        /// </summary>
        public ushort UShort_52 { get; set; }

        /// <summary>
        /// Unknown value, always ulong.MaxValue.
        /// </summary>
        public ulong ULong_54 { get; set; }
        #endregion

        #region Morph Data
        public ACTMorphData MorphData { get; set; }
        #endregion

        #region Interpolated Data
        /// <summary>
        /// Unknown value, seems to always be 0xF314.
        /// </summary>
        public ushort Unk_UShort { get; set; }
        
        /// <summary>
        /// The number of joints for this animation (again).
        /// </summary>
        public ushort JointCount_2 { get; set; }

        /// <summary>
        /// The number of frames in this animation (again).
        /// </summary>
        public ushort FrameCount_2 { get; set; }

        /// <summary>
        /// The base position of the animation (again).
        /// </summary>
        public KlonoaLV_FloatVector Position_2 { get; set; }

        /// <summary>
        /// The scalar value for all translation vectors (again).
        /// </summary>
        public float Scale_2 { get; set; }
        
        public ACTInterpolatedKeyframes[] TranslationKeyframes { get; set; }
        public ACTInterpolatedKeyframes[] RotationKeyframes { get; set; }
        #endregion

        #region Fixed Data
        public KlonoaLV_Vector16[][] TranslationVectors { get; set; }
        public KlonoaLV_Vector16[][] RotationVectors { get; set; }
        #endregion

        #region Getters
        public bool IsDummy => Pre_FileSize == 0;
        public bool IsInterpolated => RotationCount == TranslationCount;
        public bool HasMorphAnimation => MorphAnimationPointer.SerializedOffset != 0;
        #endregion

        public override void SerializeImpl(SerializerObject s)
        {
            if (IsDummy)
                return;

            JointCount = s.Serialize<ushort>(JointCount, name: nameof(JointCount));
            FrameCount = s.Serialize<ushort>(FrameCount, name: nameof(FrameCount));
            RotationCount = s.Serialize<ushort>(RotationCount, name: nameof(RotationCount));
            TranslationCount = s.Serialize<ushort>(TranslationCount, name: nameof(TranslationCount));
            Name = s.SerializeString(Name, 8, name: nameof(Name));
            UInt_10 = s.Serialize<uint>(UInt_10, name: nameof(UInt_10));
            MorphAnimationPointer = s.SerializePointer(MorphAnimationPointer, anchor: Offset, name: nameof(MorphAnimationPointer));
            KeyframesPointer = s.SerializePointer(KeyframesPointer, anchor: Offset, name: nameof(KeyframesPointer));
            s.SerializePadding(4, logIfNotNull: true);
            TranslationJoints ??= new bool[JointCount];
            s.DoBits<long>(b => {
                for (int i = 0; i < JointCount; i++) {
                    TranslationJoints[i] = b.SerializeBits<bool>(TranslationJoints[i], 1, name: $"{nameof(TranslationJoints)}[{i}]");
                }
                b.SerializePadding(64 - JointCount, logIfNotNull: true);
            });
            s.SerializePadding(8, logIfNotNull: true);
            Position = s.SerializeObject<KlonoaLV_FloatVector>(Position, name: nameof(Position));
            Scale = s.Serialize<float>(Scale, name: nameof(Scale));
            DataPointer = s.SerializePointer(DataPointer, anchor: Offset, name: nameof(DataPointer));
            UInt_44 = s.Serialize<uint>(UInt_44, name: nameof(UInt_44));
            LoopFlag = s.Serialize<uint>(LoopFlag, name: nameof(LoopFlag));
            TransitionIn = s.Serialize<ushort>(TransitionIn, name: nameof(TransitionIn));
            TransitionOut = s.Serialize<ushort>(TransitionOut, name: nameof(TransitionOut));
            UShort_50 = s.Serialize<ushort>(UShort_50, name: nameof(UShort_50));
            UShort_52 = s.Serialize<ushort>(UShort_52, name: nameof(UShort_52));
            ULong_54 = s.Serialize<ulong>(ULong_54, name: nameof(ULong_54));
            
            if (HasMorphAnimation) {
                s.DoAt(MorphAnimationPointer, () => {
                    MorphData = s.SerializeObject<ACTMorphData>(MorphData, name: nameof(MorphData));
                });
            } else {
                s.SerializePadding(4, logIfNotNull: true);
            }

            if (IsInterpolated) {
                s.DoAt(KeyframesPointer, () => {
                    Unk_UShort = s.Serialize<ushort>(Unk_UShort, name: nameof(Unk_UShort));
                    JointCount_2 = s.Serialize<ushort>(JointCount_2, name: nameof(JointCount_2));
                    FrameCount_2 = s.Serialize<ushort>(FrameCount_2, name: nameof(FrameCount_2));
                    s.SerializePadding(2, logIfNotNull: true);
                    Position_2 = s.SerializeObject<KlonoaLV_FloatVector>(Position_2, name: nameof(Position_2));
                    Scale_2 = s.Serialize<float>(Scale_2, name: nameof(Scale_2));

                    TranslationKeyframes = s.SerializeObjectArray<ACTInterpolatedKeyframes>(TranslationKeyframes, TranslationCount, name: nameof(TranslationKeyframes));
                    RotationKeyframes = s.SerializeObjectArray<ACTInterpolatedKeyframes>(RotationKeyframes, RotationCount, name: nameof(RotationKeyframes));
                });
            } else {
                RotationVectors ??= new KlonoaLV_Vector16[FrameCount][];
                for (int i = 0; i < FrameCount; i++) {
                    RotationVectors[i] = s.SerializeObjectArray<KlonoaLV_Vector16>(RotationVectors[i], RotationCount, name: $"{nameof(RotationVectors)}[{i}]");
                }

                TranslationVectors ??= new KlonoaLV_Vector16[FrameCount][];
                for (int i = 0; i < FrameCount; i++) {
                    TranslationVectors[i] = s.SerializeObjectArray<KlonoaLV_Vector16>(TranslationVectors[i], TranslationCount, name: $"{nameof(TranslationVectors)}[{i}]");
                }
            }

            s.Goto(Offset + Pre_FileSize);
        }
    }
}