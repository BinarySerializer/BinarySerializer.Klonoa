namespace BinarySerializer.Klonoa.LV
{
    public class ACTMorphData : BinarySerializable
    {
        public uint Pre_FrameCount { get; set; }

        /// <summary>
        /// The number of morph animation channels.
        /// (ex. animating face and hand morphs at the same time)
        /// </summary>
        public byte ChannelCount { get; set; }

        /// <summary>
        /// Morph data for each frame.
        /// Length: [FrameCount][ChannelCount]
        /// </summary>
        public ACTMorphFrames[][] MorphFrames { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ChannelCount = s.Serialize<byte>(ChannelCount, name: nameof(ChannelCount));
            MorphFrames ??= new ACTMorphFrames[Pre_FrameCount][];
            for (int i = 0; i < Pre_FrameCount; i++) {
                MorphFrames[i] = s.SerializeObjectArray<ACTMorphFrames>(MorphFrames[i], ChannelCount, name: $"{nameof(MorphFrames)}[{i}]");
            }
        }
    }
}