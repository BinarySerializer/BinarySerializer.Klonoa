namespace BinarySerializer.Klonoa.LV {
    public class SFXClipShadowData_File : BaseFile {
        public SFXClipShadowData[] Data { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            Data = s.SerializeObjectArray<SFXClipShadowData>(Data, 936, name: nameof(SFXClipShadowData));
        }
    }
}