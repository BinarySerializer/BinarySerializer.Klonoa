namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneFont_File : BaseFile
    {
        public uint CharactersCount { get; set; }
        public uint WidthsOffset { get; set; }
        public uint ImgDataOffset { get; set; }
        
        public byte[] CharacterWidths { get; set; }
        public byte[][] CharactersImgData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            CharactersCount = s.Serialize<uint>(CharactersCount, name: nameof(CharactersCount));
            WidthsOffset = s.Serialize<uint>(WidthsOffset, name: nameof(WidthsOffset));
            ImgDataOffset = s.Serialize<uint>(ImgDataOffset, name: nameof(ImgDataOffset));

            s.DoAt(Offset + WidthsOffset, () =>
            {
                CharacterWidths ??= new byte[CharactersCount];

                s.SerializeBitValues(bitFunc =>
                {
                    for (int i = 0; i < CharacterWidths.Length; i++)
                        CharacterWidths[i] = (byte)bitFunc(CharacterWidths[i], 4, name: $"{nameof(CharacterWidths)}[{i}]");
                });

                // Any additional ones (for alignment) are set to 8 (default)
            });
            s.DoAt(Offset + ImgDataOffset, () =>
            {
                CharactersImgData ??= new byte[CharactersCount][];

                for (int i = 0; i < CharactersImgData.Length; i++)
                    CharactersImgData[i] = s.SerializeArray<byte>(CharactersImgData[i], 0x40, name: $"{nameof(CharactersImgData)}[{i}]");
            });

            s.Goto(Offset + Pre_FileSize);
        }
    }
}