namespace BinarySerializer.Klonoa.DTP
{
    public class UVScrollAnimation_File : BaseFile
    {
        public int[] UVOffsets { get; set; } // Groups of 4

        public override void SerializeImpl(SerializerObject s)
        {
            UVOffsets = s.SerializeArrayUntil(UVOffsets, x => x == -1, () => -1, name: nameof(UVOffsets));
        }
    }
}