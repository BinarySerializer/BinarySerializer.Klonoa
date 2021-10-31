namespace BinarySerializer.Klonoa.DTP
{
    public class BackgroundGameObjectData_SetLightState : BinarySerializable
    {
        public int Int_00 { get; set; }
        public int Int_04 { get; set; }
        public int Int_08 { get; set; }
        public int Int_0C { get; set; }
        public int Int_10 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Int_00 = s.Serialize<int>(Int_00, name: nameof(Int_00));
            Int_04 = s.Serialize<int>(Int_04, name: nameof(Int_04));
            Int_08 = s.Serialize<int>(Int_08, name: nameof(Int_08));
            Int_0C = s.Serialize<int>(Int_0C, name: nameof(Int_0C));
            Int_10 = s.Serialize<int>(Int_10, name: nameof(Int_10));
        }
    }
}