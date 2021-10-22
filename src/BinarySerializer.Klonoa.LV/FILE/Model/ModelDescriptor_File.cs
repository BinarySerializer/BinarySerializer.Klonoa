namespace BinarySerializer.Klonoa.LV {
    public class ModelDescriptor_File : BaseFile {
        public string ModelName { get; set; }
        
        public override void SerializeImpl(SerializerObject s) 
        {
            ModelName = s.SerializeString(ModelName, 3, name: nameof(ModelName));
        }
    }
}