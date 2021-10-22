namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneFont_File : BaseFile
    {
        public uint CharactersCount { get; set; }
        public uint Offset_0 { get; set; }
        public uint ImgDataOffset { get; set; }
        
        public byte[] Data_0 { get; set; }
        public byte[][] CharactersImgData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            CharactersCount = s.Serialize<uint>(CharactersCount, name: nameof(CharactersCount));
            Offset_0 = s.Serialize<uint>(Offset_0, name: nameof(Offset_0));
            ImgDataOffset = s.Serialize<uint>(ImgDataOffset, name: nameof(ImgDataOffset));

            s.DoAt(Offset + Offset_0, () =>
            {
                Data_0 ??= new byte[(ImgDataOffset - Offset_0) * 2];

                for (int i = 0; i < Data_0.Length; i += 2)
                {
                    s.SerializeBitValues<byte>(bitFunc =>
                    {
                        Data_0[i] = (byte)bitFunc(Data_0[i], 4, name: $"{nameof(Data_0)}[{i}]");
                        Data_0[i + 1] = (byte)bitFunc(Data_0[i + 1], 4, name: $"{nameof(Data_0)}[{i + 1}]");
                    });
                }
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