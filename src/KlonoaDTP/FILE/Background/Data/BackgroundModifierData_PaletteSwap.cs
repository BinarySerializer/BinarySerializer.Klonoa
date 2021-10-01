namespace BinarySerializer.Klonoa.DTP
{
    public class BackgroundModifierData_PaletteSwap : BinarySerializable
    {
        public int PaletteIndex1 { get; set; }
        public int PaletteIndex2 { get; set; }
        public int Int_0C { get; set; }
        public int Int_10 { get; set; }
        public int Int_14 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializePadding(4, logIfNotNull: true);
            PaletteIndex1 = s.Serialize<int>(PaletteIndex1, name: nameof(PaletteIndex1));
            PaletteIndex2 = s.Serialize<int>(PaletteIndex2, name: nameof(PaletteIndex2));
            Int_0C = s.Serialize<int>(Int_0C, name: nameof(Int_0C));
            Int_10 = s.Serialize<int>(Int_10, name: nameof(Int_10));
            Int_14 = s.Serialize<int>(Int_14, name: nameof(Int_14));
        }
    }
}