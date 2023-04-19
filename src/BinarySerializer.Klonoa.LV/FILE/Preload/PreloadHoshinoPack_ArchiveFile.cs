namespace BinarySerializer.Klonoa.LV
{
    public class PreloadHoshinoPack_ArchiveFile : ArchiveFile
    {
        public RawData_File Soundbank_HD { get; set; } // Header file, loaded into IOP RAM
        public RawData_File Soundbank_BD { get; set; } // Body file, loaded into SPU2 RAM
        public RawData_File SoundEffectsIndices { get; set; } // Indices of sound effects that are specific to that level

        protected override void SerializeFiles(SerializerObject s)
        {
            Soundbank_HD = SerializeFile(s, Soundbank_HD, 0, name: nameof(Soundbank_HD));
            Soundbank_BD = SerializeFile(s, Soundbank_BD, 1, name: nameof(Soundbank_BD));
            SoundEffectsIndices = SerializeFile(s, SoundEffectsIndices, 2, name: nameof(SoundEffectsIndices));
        }
    }
}