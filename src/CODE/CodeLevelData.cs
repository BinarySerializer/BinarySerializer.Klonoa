namespace BinarySerializer.KlonoaDTP
{
    public class CodeLevelData : BinarySerializable
    {
        public Pointer Pointer_0 { get; set; }
        public Pointer Pointer_1 { get; set; }
        public Pointer Pointer_2 { get; set; }
        public Pointer Pointer_3 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Pointer_0 = s.SerializePointer(Pointer_0, name: nameof(Pointer_0));
            Pointer_1 = s.SerializePointer(Pointer_1, name: nameof(Pointer_1));
            Pointer_2 = s.SerializePointer(Pointer_2, name: nameof(Pointer_2));
            Pointer_3 = s.SerializePointer(Pointer_3, name: nameof(Pointer_3));
        }
    }
}