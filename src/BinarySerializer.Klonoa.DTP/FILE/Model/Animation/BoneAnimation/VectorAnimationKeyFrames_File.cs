using System;
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

            foreach (VectorAnimationKeyFrame keyFrame in KeyFrames[index])
            {
                void setValue(int change, bool isRelative)
                {
                    if (!isRelative)
                    {
                        values[frameIndex] = change;
                    }
                    else
                    {
                        int prevValue = frameIndex > 0 ? values[frameIndex - 1] : 0;
                        values[frameIndex] = prevValue + change;
                    }
                    frameIndex++;
                }

                int valueChange = keyFrame.ActualValueChange;
                int timer = 1;
                int repeat = keyFrame.RepeatCount + 1;

                switch (keyFrame.Type)
                {
                    case VectorAnimationKeyFrame.CommandType.Relative:
                        setValue(valueChange, true);
                        break;

                    case VectorAnimationKeyFrame.CommandType.Absolute:
                        setValue(valueChange, false);
                        break;

                    case VectorAnimationKeyFrame.CommandType.RelativeRepeat:
                        setValue(valueChange, true);

                        do
                        {
                            setValue(valueChange, true);
                            timer++;
                        } while (timer < repeat);
                        break;

                    case VectorAnimationKeyFrame.CommandType.RelativeRepeatWithChangeBy1:
                        setValue(valueChange + (keyFrame.Sign ? -1 : 1), true);

                        int changeBy1CountDown = keyFrame.ChangeBy1Count - 1;

                        do
                        {
                            int v = valueChange;

                            if (changeBy1CountDown > 0)
                            {
                                v += keyFrame.Sign ? -1 : 1;
                                changeBy1CountDown--;
                            }

                            setValue(v, true);
                            timer++;
                        } while (timer < repeat);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (frameIndex != values.Length)
                throw new Exception($"Frame index: {frameIndex}, frames count: {FramesCount}");

            return values;
        }
    }
}