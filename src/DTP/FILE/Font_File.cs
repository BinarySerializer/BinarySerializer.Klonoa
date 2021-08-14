namespace BinarySerializer.Klonoa.DTP
{
    public class Font_File : BaseFile
    {
        // The font sheet is split up into rows where each row has 10 characters. Each character is 12x24 pixels.
        public byte[] FontData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FontData = s.SerializeArray<byte>(FontData, Pre_FileSize, name: nameof(FontData));
        }
    }
}