namespace BinarySerializer.Klonoa.LV
{
    public class ModelAnimation_MorphFrames : BinarySerializable
    {
        /// <summary>
        /// Weight: (255 - StartMorph) / 255
        /// </summary>
        public byte StartMorph { get; set; }
        
        /// <summary>
        /// Weight: EndMorph / 255
        /// </summary>
        public byte EndMorph { get; set; }

        /// <summary>
        /// The weight of the start/end morph at this frame.
        /// </summary>
        public byte Weight { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            StartMorph = s.Serialize<byte>(StartMorph, name: nameof(StartMorph));
            EndMorph = s.Serialize<byte>(EndMorph, name: nameof(EndMorph));
            Weight = s.Serialize<byte>(Weight, name: nameof(Weight));
        }
    }
}