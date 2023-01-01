namespace BinarySerializer.Klonoa.LV
{
    public class ModelAnimation_MorphData : BinarySerializable
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
        public ModelAnimation_MorphFrames[][] MorphFrames { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ChannelCount = s.Serialize<byte>(ChannelCount, name: nameof(ChannelCount));
            MorphFrames ??= new ModelAnimation_MorphFrames[Pre_FrameCount][];
            for (int i = 0; i < Pre_FrameCount; i++) {
                MorphFrames[i] = s.SerializeObjectArray<ModelAnimation_MorphFrames>(MorphFrames[i], ChannelCount, name: $"{nameof(MorphFrames)}[{i}]");
            }
        }
    }
}