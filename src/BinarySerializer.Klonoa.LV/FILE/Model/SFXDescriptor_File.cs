namespace BinarySerializer.Klonoa.LV {
    public class SFXDescriptor_File : BaseFile {
        public string ModelName { get; set; }
        
        public override void SerializeImpl(SerializerObject s) 
        {
            ModelName = s.SerializeString(ModelName, 3, name: nameof(ModelName));
            // TODO
            s.Goto(Offset + Pre_FileSize);
        }
    }
}