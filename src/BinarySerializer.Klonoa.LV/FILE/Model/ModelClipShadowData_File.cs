namespace BinarySerializer.Klonoa.LV {
    public class ModelClipShadowData_File : BaseFile {
        public ModelClipShadowData[] Data { get; set; }

        public override void SerializeImpl(SerializerObject s) 
        {
            Data = s.SerializeObjectArray<ModelClipShadowData>(Data, 936, name: nameof(ModelClipShadowData));
        }
    }
}