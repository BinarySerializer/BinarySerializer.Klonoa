namespace BinarySerializer.Klonoa.KH
{
    public class AnimationGroup : BinarySerializable
    {
        public ushort AnimationsCount { get; set; } // Always 8
        public uint BaseAnimationsOffset { get; set; } // Always 48
        public int[] AnimationOffsets { get; set; } // Can be -1

        public Animation[] Animations { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("AD", 2);
            AnimationsCount = s.Serialize<ushort>(AnimationsCount, name: nameof(AnimationsCount));
            BaseAnimationsOffset = s.Serialize<uint>(BaseAnimationsOffset, name: nameof(BaseAnimationsOffset));
            s.SerializePadding(8, logIfNotNull: true);
            AnimationOffsets = s.SerializeArray<int>(AnimationOffsets, AnimationsCount, name: nameof(AnimationOffsets));

            Animations ??= new Animation[AnimationsCount];

            for (int i = 0; i < Animations.Length; i++)
            {
                if (AnimationOffsets[i] == -1)
                    continue;

                s.DoAt(Offset + BaseAnimationsOffset + AnimationOffsets[i], () =>
                {
                    // TODO: Find better way of handling this which works for writing
                    // Verify it's an animation. If all offsets are 0 there might not be any animations to point to.
                    var isValid = s.DoAt(s.CurrentPointer, () => s.SerializeString(default, 2, name: "AnimCheck") == "AF");

                    if (isValid)
                        Animations[i] = s.SerializeObject<Animation>(Animations[i], name: $"{nameof(Animations)}[{i}]");
                });
            }
        }
    }
}