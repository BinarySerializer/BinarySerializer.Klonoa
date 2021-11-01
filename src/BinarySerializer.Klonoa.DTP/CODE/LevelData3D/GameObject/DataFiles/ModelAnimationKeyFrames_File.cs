using System.Collections.Generic;

namespace BinarySerializer.Klonoa.DTP
{
    public class ModelAnimationKeyFrames_File : BaseFile
    {
        public byte PartsCount { get; set; }
        public short FramesCount { get; set; }
        public short[] KeyFrameOffsets { get; set; }
        public ModelAnimationKeyFrame[][] KeyFrames { get; set; } // 3 per part

        public override void SerializeImpl(SerializerObject s)
        {
            PartsCount = s.Serialize<byte>(PartsCount, name: nameof(PartsCount));
            s.SerializePadding(1);
            FramesCount = s.Serialize<short>(FramesCount, name: nameof(FramesCount));
            KeyFrameOffsets = s.SerializeArray<short>(KeyFrameOffsets, PartsCount * 3, name: nameof(KeyFrameOffsets));

            Pointer anchor = s.CurrentPointer;

            KeyFrames ??= new ModelAnimationKeyFrame[KeyFrameOffsets.Length][];

            for (int i = 0; i < KeyFrames.Length; i++)
            {
                s.DoAt(anchor + KeyFrameOffsets[i], () =>
                {
                    s.SerializeBitValues(bitFunc =>
                    {
                        if (KeyFrames[i] == null)
                        {
                            long frames = 0;
                            var keyFrames = new List<ModelAnimationKeyFrame>();

                            while (frames < FramesCount)
                            {
                                var keyFrame = new ModelAnimationKeyFrame();
                                keyFrame.Serialize(bitFunc);

                                keyFrames.Add(keyFrame);
                                frames += 1 + keyFrame.RepeatCount;
                            }

                            KeyFrames[i] = keyFrames.ToArray();
                        }
                        else
                        {
                            foreach (ModelAnimationKeyFrame keyFrame in KeyFrames[i])
                                keyFrame.Serialize(bitFunc);
                        }
                    });
                });
            }

            s.Goto(Offset + Pre_FileSize);
        }
    }
}