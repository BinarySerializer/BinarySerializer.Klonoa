namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// Hard-coded files for video animations (water, fire, etc)
    /// </summary>
    public class VTPack_ArchiveFile : ArchiveFile
    {
        public int Pre_Level { get; set; }
        public int Pre_SectorCount { get; set; }

        public VTIPU_File[,] IPUs { get; set; } // [SectorNum,Index]
        public VTWave_File[] Waves { get; set; } // Only used in Sea of Tears and Dark Sea of Tears
        public VTSpray_File[] Sprays { get; set; } // Only used in Sea of Tears

        protected override void SerializeFiles(SerializerObject s)
        {
            Sprays = new VTSpray_File[Pre_SectorCount];
            Waves = new VTWave_File[Pre_SectorCount];
            IPUs = new VTIPU_File[Pre_SectorCount, 7]; // IPU files are often shared between sectors

            switch (Pre_Level) {
                case 0: // Sea of Tears
                    IPUs[0,6] = IPUs[2,6] = IPUs[4,6] = SerializeFile(s, IPUs[0,6], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[1,6] = SerializeFile(s, IPUs[1,6], 1, name: $"{nameof(IPUs)}[1]");
                    Sprays[0] = SerializeFile(s, Sprays[0], 2, name: $"{nameof(Sprays)}[0]");
                    Sprays[2] = SerializeFile(s, Sprays[2], 3, name: $"{nameof(Sprays)}[2]");
                    Waves[0] = SerializeFile(s, Waves[0], 4, name: $"{nameof(Waves)}[0]");
                    Waves[1] = SerializeFile(s, Waves[0], 5, name: $"{nameof(Waves)}[1]");
                    Waves[2] = SerializeFile(s, Waves[0], 6, name: $"{nameof(Waves)}[2]");
                    Waves[3] = SerializeFile(s, Waves[0], 7, name: $"{nameof(Waves)}[3]");
                    Waves[4] = SerializeFile(s, Waves[0], 8, name: $"{nameof(Waves)}[4]");
                    // What is file 9/10?
                    IPUs[5,0] = SerializeFile(s, IPUs[5,0], 11, name: $"{nameof(IPUs)}[2]");
                    break;
                // TODO: Parse more levels
                default:
                    break;
            }
        }
    }
}