namespace BinarySerializer.Klonoa.LV
{
    public class SDT_File : BaseFile
    {
        public int TotalPtcl { get; set; } // ?
        public int ParameterCount { get; set; }
        public SDTParameter[] Parameters { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            TotalPtcl = s.Serialize<int>(TotalPtcl, name: nameof(TotalPtcl));
            ParameterCount = s.Serialize<int>(ParameterCount, name: nameof(ParameterCount));
            Parameters = new SDTParameter[ParameterCount];
            for (int i = 0; i < ParameterCount; i++ ) {
                Parameters[i] = s.SerializeObject<SDTParameter>(Parameters[i], name: $"{nameof(Parameters)}[{i}]");
            }
        }
    }
}