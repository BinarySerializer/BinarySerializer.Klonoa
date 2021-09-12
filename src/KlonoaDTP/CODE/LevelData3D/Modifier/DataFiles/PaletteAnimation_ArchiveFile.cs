namespace BinarySerializer.Klonoa.DTP
{
    public class PaletteAnimation_ArchiveFile : ArchiveFile<ArchiveFile<PaletteAnimation_ArchiveFile.RawPalette>>
    {
        public class RawPalette : BinarySerializable
        {
            public byte[] Colors { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Colors = s.SerializeArray<byte>(Colors, 32, name: nameof(Colors));
            }
        }
    }
}