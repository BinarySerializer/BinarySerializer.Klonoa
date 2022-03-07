namespace BinarySerializer.Klonoa.DTP
{
    public class STM : BinarySerializable
    {
        public STM_Entry[] Entries { get; set; } // Last one is a dummy entry

        public override void SerializeImpl(SerializerObject s)
        {
            Entries = s.SerializeObjectArrayUntil<STM_Entry>(Entries, x => x.IsDummy, onPreSerialize: (x, i) => x.Pre_Anchor = Offset, name: nameof(Entries));
        }
    }
}