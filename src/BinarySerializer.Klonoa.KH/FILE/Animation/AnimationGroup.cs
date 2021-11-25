namespace BinarySerializer.Klonoa.KH
{
    public class AnimationGroup : BinarySerializable
    {
        public ushort AnimationsCount { get; set; } // Always 8
        public uint BaseAnimationsOffset { get; set; } // Always 48
        public int[] AnimationOffsets { get; set; } // Can be -1

        public Animation[] Animations { get; set; } // Contains an animation for each rotation (0-7)

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("AD", 2);
            AnimationsCount = s.Serialize<ushort>(AnimationsCount, name: nameof(AnimationsCount));
            BaseAnimationsOffset = s.Serialize<uint>(BaseAnimationsOffset, name: nameof(BaseAnimationsOffset));
            s.SerializePadding(8, logIfNotNull: true);
            AnimationOffsets = s.SerializeArray<int>(AnimationOffsets, AnimationsCount, name: nameof(AnimationOffsets));

            bool hasParsedAnims = Animations != null;

            Animations ??= new Animation[AnimationsCount];

            for (int i = 0; i < Animations.Length; i++)
            {
                if (AnimationOffsets[i] == -1)
                    continue;

                s.DoAt(Offset + BaseAnimationsOffset + AnimationOffsets[i], () =>
                {
                    // TODO: Potentially find a better solution to this to make both reading and writing work better

                    // Verify it's an animation. If all offsets are 0 there might not be any animations to point to.
                    // If the animation is not null (such as if we're writing a read object) then we assume it's valid
                    bool isValid = Animations[i] != null;

                    // If it's null we check the following bytes, unless we've already parsed the animations in which case
                    // we assume we're writing the data
                    if (!isValid && !hasParsedAnims)
                        isValid = s.DoAt(s.CurrentPointer, () => s.SerializeString(default, 2, name: "AnimCheck") == "AF");

                    if (isValid)
                        Animations[i] = s.SerializeObject<Animation>(Animations[i], name: $"{nameof(Animations)}[{i}]");
                });
            }
        }
    }
}