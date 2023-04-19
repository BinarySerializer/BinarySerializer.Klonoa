namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// Wave animations (ex. water waves in Sea of Tears)
    /// </summary>
    public class WDT_File : BaseFile
    {
        public int ParameterCount { get; set; }
        public int Int_0 { get; set; } // ?
        public int Int_1 { get; set; } // ?
        public int Int_2 { get; set; } // ?
        public WDTParameter[] Parameters { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ParameterCount = s.Serialize<int>(ParameterCount, name: nameof(ParameterCount));
            Int_0 = s.Serialize<int>(Int_0, name: nameof(Int_0));
            Int_1 = s.Serialize<int>(Int_1, name: nameof(Int_1));
            Int_2 = s.Serialize<int>(Int_2, name: nameof(Int_2));
            Parameters = new WDTParameter[ParameterCount];
            for (int i = 0; i < ParameterCount; i++ ) {
                Parameters[i] = s.SerializeObject<WDTParameter>(Parameters[i], name: $"{nameof(Parameters)}[{i}]");
            }
        }
    }
}