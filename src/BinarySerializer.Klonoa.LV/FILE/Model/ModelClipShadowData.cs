namespace BinarySerializer.Klonoa.LV {
    public class ModelClipShadowData : BinarySerializable {
        public float ClipOffset { get; set; }
        public float ClipZone { get; set; }
        public int ShadowType { get; set; }
        public float ShadowSize { get; set; }
        public float ShadowOffset { get; set; }
        public float ShadowRange { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            ClipOffset = s.Serialize<float>(ClipOffset, name: nameof(ClipOffset));
            ClipZone = s.Serialize<float>(ClipZone, name: nameof(ClipZone));
            ShadowType = s.Serialize<int>(ShadowType, name: nameof(ShadowType));
            ShadowSize = s.Serialize<float>(ShadowSize, name: nameof(ShadowSize));
            ShadowOffset = s.Serialize<float>(ShadowOffset, name: nameof(ShadowOffset));
            ShadowRange = s.Serialize<float>(ShadowRange, name: nameof(ShadowRange));
        }
    }
}