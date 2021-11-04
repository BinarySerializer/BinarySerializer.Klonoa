using System.Collections.Generic;

namespace BinarySerializer.Klonoa.DTP
{
    public class VectorAnimationKeyFrames_File : BaseFile
    {
        public byte BonesCount { get; set; }
        public short FramesCount { get; set; }
        public short[] KeyFrameOffsets { get; set; }
        public VectorAnimationKeyFrame[][] KeyFrames { get; set; } // 3 entries per bone (x, y, z)

        public override void SerializeImpl(SerializerObject s)
        {
            BonesCount = s.Serialize<byte>(BonesCount, name: nameof(BonesCount));
            s.SerializePadding(1);
            FramesCount = s.Serialize<short>(FramesCount, name: nameof(FramesCount));
            KeyFrameOffsets = s.SerializeArray<short>(KeyFrameOffsets, BonesCount * 3, name: nameof(KeyFrameOffsets));

            Pointer anchor = s.CurrentPointer;

            KeyFrames ??= new VectorAnimationKeyFrame[KeyFrameOffsets.Length][];

            for (int i = 0; i < KeyFrames.Length; i++)
            {
                s.DoAt(anchor + KeyFrameOffsets[i], () =>
                {
                    s.SerializeBitValues(bitFunc =>
                    {
                        if (KeyFrames[i] == null)
                        {
                            long frames = 0;
                            var keyFrames = new List<VectorAnimationKeyFrame>();

                            while (frames < FramesCount)
                            {
                                var keyFrame = new VectorAnimationKeyFrame();
                                keyFrame.Serialize(bitFunc);

                                keyFrames.Add(keyFrame);
                                frames += 1 + keyFrame.RepeatCount;
                            }

                            KeyFrames[i] = keyFrames.ToArray();
                        }
                        else
                        {
                            foreach (VectorAnimationKeyFrame keyFrame in KeyFrames[i])
                                keyFrame.Serialize(bitFunc);
                        }
                    });
                });
            }

            s.Goto(Offset + Pre_FileSize);
        }

        public int[] GetValues(int index)
        {
            int[] values = new int[FramesCount];
            int frameIndex = 0;

            void setValue(int change)
            {
                int prevValue = frameIndex > 0 ? values[frameIndex - 1] : 0;
                values[frameIndex] = prevValue + change;
                frameIndex++;
            }

            foreach (VectorAnimationKeyFrame keyFrame in KeyFrames[index])
            {
                int valueChange = keyFrame.ActualValueChange;
                int changeBy1Count = keyFrame.ChangeBy1Count;

                if (keyFrame.ChangeBy1Count > 0)
                {
                    valueChange += keyFrame.Sign ? -1 : 1;
                    changeBy1Count--;
                }

                setValue(valueChange);

                if (keyFrame.RepeatCount == 0)
                    continue;

                int timer = 1;

                do
                {
                    valueChange = keyFrame.ActualValueChange;

                    if (changeBy1Count > 0)
                    {
                        valueChange += keyFrame.Sign ? -1 : 1;
                        changeBy1Count--;
                    }

                    setValue(valueChange);
                    timer++;
                } while (timer < keyFrame.RepeatCount);
            }

            return values;
        }
    }
}