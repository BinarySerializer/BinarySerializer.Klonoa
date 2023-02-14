using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class RGBAnimation : BinarySerializable
    {
        public int ModelObjectIndex { get; set; } // The game only uses this to make sure the object is on screen before updating the animation
        public int ColorOffset { get; set; }
        public TMD_Color OriginalColor { get; set; }
        public TMD_Color NewColor { get; set; } // Might be wrong, seems to be higher than 255? The game uses dpcs to calculate it.

        public override void SerializeImpl(SerializerObject s)
        {
            ModelObjectIndex = s.Serialize<int>(ModelObjectIndex, name: nameof(ModelObjectIndex));
            ColorOffset = s.Serialize<int>(ColorOffset, name: nameof(ColorOffset));
            OriginalColor = s.SerializeObject<TMD_Color>(OriginalColor, name: nameof(OriginalColor));
            NewColor = s.SerializeObject<TMD_Color>(NewColor, name: nameof(NewColor));
        }
    }
}