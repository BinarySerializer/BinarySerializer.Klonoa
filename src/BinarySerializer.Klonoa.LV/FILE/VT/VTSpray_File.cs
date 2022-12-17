namespace BinarySerializer.Klonoa.LV
{
    public class VTSpray_File : BaseFile
    {
        public int TotalPtcl { get; set; } // ?
        public int ParameterCount { get; set; }
        public VTSpray_Parameter[] Parameters { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            TotalPtcl = s.Serialize<int>(TotalPtcl, name: nameof(TotalPtcl));
            ParameterCount = s.Serialize<int>(ParameterCount, name: nameof(ParameterCount));
            Parameters = new VTSpray_Parameter[ParameterCount];
            for (int i = 0; i < ParameterCount; i++ ) {
                Parameters[i] = s.SerializeObject<VTSpray_Parameter>(Parameters[i], name: $"{nameof(Parameters)}[{i}]");
            }
        }
    }
}