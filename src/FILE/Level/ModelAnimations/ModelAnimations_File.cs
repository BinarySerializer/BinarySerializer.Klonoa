namespace BinarySerializer.KlonoaDTP
{
    // TODO: Name this better, how is this structure used?
    public class ModelAnimations_File : BaseFile
    {
        public ushort Data1Count { get; set; }
        public ushort Data1ItemLength { get; set; }
        public ushort Ushort_04 { get; set; }

        public short Short_08 { get; set; }
        public short Short_0A { get; set; }
        public short Short_0C { get; set; }

        public ushort Offset1 { get; set; }
        public ushort Offset2 { get; set; }
        public ushort AnimationsOffset { get; set; }

        public Data1Entry[][] Data1 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Data1Count = s.Serialize<ushort>(Data1Count, name: nameof(Data1Count));
            Data1ItemLength = s.Serialize<ushort>(Data1ItemLength, name: nameof(Data1ItemLength));
            Ushort_04 = s.Serialize<ushort>(Ushort_04, name: nameof(Ushort_04));
            s.SerializePadding(2, logIfNotNull: true);
            Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
            Short_0A = s.Serialize<short>(Short_0A, name: nameof(Short_0A));
            Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
            s.SerializePadding(2, logIfNotNull: true);
            Offset1 = s.Serialize<ushort>(Offset1, name: nameof(Offset1));
            Offset2 = s.Serialize<ushort>(Offset2, name: nameof(Offset2));
            AnimationsOffset = s.Serialize<ushort>(AnimationsOffset, name: nameof(AnimationsOffset));

            s.DoAt(Offset + Offset1 * 2, () =>
            {
                var animPointer = Offset + AnimationsOffset * 2;
                var data2Pointer = Offset + Offset2 * 2;

                Data1 ??= new Data1Entry[Data1Count][];

                for (int i = 0; i < Data1.Length; i++)
                {
                    Data1[i] = s.SerializeObjectArray<Data1Entry>(Data1[i], Data1ItemLength, onPreSerialize: x =>
                    {
                        x.Pre_AnimPointer = animPointer;
                        x.Pre_Data2Pointer = data2Pointer;
                        x.Pre_Ushort_04 = Ushort_04;
                    }, name: $"{nameof(Data1)}[{i}]");
                }
            });

            // Got to the end of the file
            s.Goto(Offset + Pre_FileSize);
        }

        public class Data1Entry : BinarySerializable
        {
            public Pointer Pre_Data2Pointer { get; set; }
            public Pointer Pre_AnimPointer { get; set; }
            public ushort Pre_Ushort_04 { get; set; }

            public ushort Data2Offset { get; set; }

            public Data2Struct[] Data2 { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Data2Offset = s.Serialize<ushort>(Data2Offset, name: nameof(Data2Offset));

                s.DoAt(Pre_Data2Pointer + Data2Offset * 4, () =>
                {
                    var v = 0;

                    Data2 = s.SerializeObjectArrayUntil<Data2Struct>(Data2, x => (v += x.Ushort_02) >= Pre_Ushort_04, onPreSerialize: x => x.Pre_AnimPointer = Pre_AnimPointer, name: nameof(Data2));
                });
            }
        }

        public class ModelAnimation : BinarySerializable
        {
            public ushort[] FrameIndices { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                FrameIndices = s.SerializeArrayUntil<ushort>(FrameIndices, x => x == 0xFFFF, () => 0xFFFF, name: nameof(FrameIndices));
            }
        }
        public class Data2Struct : BinarySerializable
        {
            public Pointer Pre_AnimPointer { get; set; }

            public ushort AnimOffset { get; set; }
            public ushort Ushort_02 { get; set; }

            public ModelAnimation Animation { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                AnimOffset = s.Serialize<ushort>(AnimOffset, name: nameof(AnimOffset));
                Ushort_02 = s.Serialize<ushort>(Ushort_02, name: nameof(Ushort_02));

                if (AnimOffset != 0xFFFF)
                    s.DoAt(Pre_AnimPointer + AnimOffset * 2, () => Animation = s.SerializeObject<ModelAnimation>(Animation, name: nameof(Animation)));
            }
        }
    }
}