using System;

namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// Hard-coded files for video textures/animations (water, fire, etc)
    /// </summary>
    public class VTPack_ArchiveFile : ArchiveFile
    {
        public int Pre_Level { get; set; }
        public int Pre_SectorCount { get; set; }

        // The game uses an array of 8 pointers to VTIPU files, which gets set when a sector is loaded
        // Some files get placed at the end of the array, though the reason for this is unknown
        // (might have something to do with wave animations?)
        public VTIPU_File[,] IPUs { get; set; } // [SectorNum, Index]
        public VTWave_File[] Waves { get; set; }
        public VTSpray_File[] Sprays { get; set; } // Only used in Sea of Tears
        public RawData_File DummyFile { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Sprays = new VTSpray_File[Pre_SectorCount]; // One spray max per sector
            Waves = new VTWave_File[Pre_SectorCount]; // One wave max per sector
            IPUs = new VTIPU_File[Pre_SectorCount, 8]; // IPU files are often shared between sectors

            switch (Pre_Level) {
                case 1: // Sea of Tears
                    IPUs[0, 6] = IPUs[2, 6] = IPUs[4, 6] = SerializeFile(s, IPUs[0, 6], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[1, 6] = SerializeFile(s, IPUs[1, 6], 1, name: $"{nameof(IPUs)}[1]");
                    Sprays[0] = SerializeFile(s, Sprays[0], 2, name: $"{nameof(Sprays)}[0]");
                    Sprays[2] = SerializeFile(s, Sprays[2], 3, name: $"{nameof(Sprays)}[2]");
                    Waves[0] = SerializeFile(s, Waves[0], 4, name: $"{nameof(Waves)}[0]");
                    Waves[1] = SerializeFile(s, Waves[0], 5, name: $"{nameof(Waves)}[1]");
                    Waves[2] = SerializeFile(s, Waves[0], 6, name: $"{nameof(Waves)}[2]");
                    Waves[3] = SerializeFile(s, Waves[0], 7, name: $"{nameof(Waves)}[3]");
                    Waves[4] = SerializeFile(s, Waves[0], 8, name: $"{nameof(Waves)}[4]");
                    DummyFile = SerializeFile(s, DummyFile, 9, name: nameof(DummyFile));
                    // 10: ?
                    IPUs[5, 0] = SerializeFile(s, IPUs[5, 0], 11, name: $"{nameof(IPUs)}[2]");
                    break;
                case 2:
                    // 0: Collision
                    IPUs[0, 0] = IPUs[2, 0] = IPUs[4, 0] = SerializeFile(s, IPUs[0, 0], 1, name: $"{nameof(IPUs)}[0]");
                    break;
                case 4:
                    // 0-3: Collision
                    IPUs[0, 0] = IPUs[11, 0] = IPUs[12, 0] = SerializeFile(s, IPUs[0, 0], 4, name: $"{nameof(IPUs)}[0]");
                    IPUs[9, 0] = SerializeFile(s, IPUs[9, 0], 5, name: $"{nameof(IPUs)}[1]");
                    IPUs[3, 7] = SerializeFile(s, IPUs[1, 7], 6, name: $"{nameof(IPUs)}[2]");
                    break;
                case 5:
                    IPUs[0, 0] = IPUs[1, 0] = IPUs[3, 0] = IPUs[4, 0] = IPUs[5, 0] = SerializeFile(s, IPUs[0, 0], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[0, 1] = IPUs[1, 1] = IPUs[4, 1] = IPUs[5, 1] = SerializeFile(s, IPUs[0, 1], 1, name: $"{nameof(IPUs)}[1]");
                    IPUs[0, 2] = IPUs[1, 2] = IPUs[4, 2] = IPUs[5, 2] = SerializeFile(s, IPUs[0, 2], 2, name: $"{nameof(IPUs)}[2]");
                    DummyFile = SerializeFile(s, DummyFile, 3, name: nameof(DummyFile));
                    // 4: Collision
                    break;
                case 6:
                    IPUs[0, 0] = SerializeFile(s, IPUs[0, 0], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[0, 1] = SerializeFile(s, IPUs[0, 1], 1, name: $"{nameof(IPUs)}[1]");
                    IPUs[3, 0] = IPUs[6, 0] = IPUs[7, 0] = SerializeFile(s, IPUs[3, 0], 2, name: $"{nameof(IPUs)}[2]");
                    break;
                case 7:
                    // 0: Collision
                    // 1: Collision
                    IPUs[0, 0] = IPUs[1, 0] = IPUs[2, 0] = SerializeFile(s, IPUs[0, 0], 2, name: $"{nameof(IPUs)}[0]");
                    IPUs[0, 1] = SerializeFile(s, IPUs[0, 1], 3, name: $"{nameof(IPUs)}[1]");
                    IPUs[1, 7] = IPUs[2, 7] = IPUs[3, 7] = SerializeFile(s, IPUs[1, 7], 4, name: $"{nameof(IPUs)}[2]");
                    break;
                case 8:
                    IPUs[0, 0] = IPUs[1, 0] = IPUs[2, 0] = SerializeFile(s, IPUs[0, 0], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[1, 7] = IPUs[2, 7] = IPUs[3, 7] = IPUs[4, 7] = IPUs[5, 7] = SerializeFile(s, IPUs[1, 7], 1, name: $"{nameof(IPUs)}[1]");
                    break;
                case 9:
                    IPUs[0, 0] = IPUs[3, 0] = IPUs[4, 0] = SerializeFile(s, IPUs[0, 0], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[3, 1] = IPUs[4, 1] = SerializeFile(s, IPUs[3, 1], 1, name: $"{nameof(IPUs)}[1]");
                    IPUs[5, 0] = IPUs[6, 0] = IPUs[7, 0] = SerializeFile(s, IPUs[5, 0], 2, name: $"{nameof(IPUs)}[2]");
                    IPUs[0, 1] = SerializeFile(s, IPUs[0, 1], 3, name: $"{nameof(IPUs)}[3]");
                    break;
                case 10:
                    IPUs[2, 0] = SerializeFile(s, IPUs[0, 0], 0, name: $"{nameof(IPUs)}[0]");
                    break;
                case 11:
                    IPUs[12, 0] = SerializeFile(s, IPUs[12, 0], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[12, 1] = SerializeFile(s, IPUs[12, 1], 1, name: $"{nameof(IPUs)}[1]");
                    IPUs[12, 2] = SerializeFile(s, IPUs[12, 2], 2, name: $"{nameof(IPUs)}[2]");
                    IPUs[13, 0] = SerializeFile(s, IPUs[13, 0], 3, name: $"{nameof(IPUs)}[3]");
                    IPUs[13, 1] = SerializeFile(s, IPUs[13, 1], 4, name: $"{nameof(IPUs)}[4]");
                    IPUs[13, 2] = SerializeFile(s, IPUs[13, 2], 5, name: $"{nameof(IPUs)}[5]");
                    IPUs[14, 0] = SerializeFile(s, IPUs[14, 0], 6, name: $"{nameof(IPUs)}[6]");
                    IPUs[14, 1] = SerializeFile(s, IPUs[14, 1], 7, name: $"{nameof(IPUs)}[7]");
                    break;
                case 12:
                    // 0: Collision
                    IPUs[0, 0] = IPUs[2, 0] = IPUs[4, 0] = SerializeFile(s, IPUs[0, 0], 1, name: $"{nameof(IPUs)}[0]");
                    IPUs[8, 0] = SerializeFile(s, IPUs[8, 0], 1, name: $"{nameof(IPUs)}[1]");
                    break;
                case 13:
                    IPUs[0, 6] = IPUs[1, 6] = IPUs[2, 6] = IPUs[3, 6] = IPUs[4, 6] = SerializeFile(s, IPUs[0, 6], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[2, 7] = SerializeFile(s, IPUs[2, 7], 1, name: $"{nameof(IPUs)}[1]");
                    Waves[0] = SerializeFile(s, Waves[0], 2, name: $"{nameof(Waves)}[0]");
                    Waves[1] = SerializeFile(s, Waves[1], 3, name: $"{nameof(Waves)}[1]");
                    Waves[2] = SerializeFile(s, Waves[2], 4, name: $"{nameof(Waves)}[2]");
                    Waves[3] = SerializeFile(s, Waves[3], 5, name: $"{nameof(Waves)}[3]");
                    Waves[4] = SerializeFile(s, Waves[4], 6, name: $"{nameof(Waves)}[4]");
                    Waves[5] = SerializeFile(s, Waves[5], 7, name: $"{nameof(Waves)}[5]");
                    IPUs[5, 6] = SerializeFile(s, IPUs[5, 6], 8, name: $"{nameof(IPUs)}[2]");
                    IPUs[6, 0] = SerializeFile(s, IPUs[6, 0], 9, name: $"{nameof(IPUs)}[3]");
                    break;
                case 14:
                    IPUs[0, 0] = IPUs[1, 0] = IPUs[2, 0] = IPUs[3, 0] = IPUs[4, 0] = SerializeFile(s, IPUs[0, 0], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[0, 1] = SerializeFile(s, IPUs[0, 1], 1, name: $"{nameof(IPUs)}[1]");
                    IPUs[1, 1] = IPUs[2, 1] = IPUs[3, 1] = IPUs[4, 1] = SerializeFile(s, IPUs[0, 0], 2, name: $"{nameof(IPUs)}[2]");
                    IPUs[0, 2] = SerializeFile(s, IPUs[0, 2], 3, name: $"{nameof(IPUs)}[3]");
                    break;
                case 15:
                    IPUs[0, 0] = IPUs[3, 0] = IPUs[4, 0] = SerializeFile(s, IPUs[0, 0], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[0, 1] = IPUs[3, 1] = IPUs[4, 1] = SerializeFile(s, IPUs[0, 1], 1, name: $"{nameof(IPUs)}[1]");
                    IPUs[8, 6] = SerializeFile(s, IPUs[8, 6], 2, name: $"{nameof(IPUs)}[2]");
                    Waves[8] = SerializeFile(s, Waves[8], 3, name: $"{nameof(Waves)}[8]");
                    IPUs[2, 6] = SerializeFile(s, IPUs[2, 6], 4, name: $"{nameof(IPUs)}[3]");
                    Waves[2] = SerializeFile(s, Waves[2], 5, name: $"{nameof(Waves)}[5]");
                    break;
                case 16:
                    IPUs[7, 0] = IPUs[9, 0] = SerializeFile(s, IPUs[7, 0], 0, name: $"{nameof(IPUs)}[0]");
                    break;
                case 17:
                    IPUs[0, 0] = IPUs[1, 0] = IPUs[2, 0] = SerializeFile(s, IPUs[0, 0], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[0, 1] = IPUs[1, 1] = IPUs[2, 1] = SerializeFile(s, IPUs[0, 1], 1, name: $"{nameof(IPUs)}[1]");
                    break;
                case 18:
                    IPUs[0, 0] = IPUs[1, 0] = IPUs[2, 0] = SerializeFile(s, IPUs[0, 0], 0, name: $"{nameof(IPUs)}[0]");
                    break;
                case 19:
                    IPUs[0, 0] = IPUs[3, 0] = IPUs[4, 0] = IPUs[5, 0] = IPUs[7, 0] = SerializeFile(s, IPUs[0, 0], 0, name: $"{nameof(IPUs)}[0]");
                    break;
                case 22:
                    IPUs[0, 0] = SerializeFile(s, IPUs[0, 0], 0, name: $"{nameof(IPUs)}[0]");
                    break;
                case 23:
                    IPUs[1, 0] = SerializeFile(s, IPUs[1, 0], 0, name: $"{nameof(IPUs)}[0]");
                    break;
                case 26:
                    IPUs[1, 0] = SerializeFile(s, IPUs[1, 0], 0, name: $"{nameof(IPUs)}[0]");
                    // 1: Collision
                    break;
                case 30:
                    IPUs[1, 0] = SerializeFile(s, IPUs[1, 0], 0, name: $"{nameof(IPUs)}[0]");
                    break;
                case 34:
                    IPUs[2, 0] = IPUs[3, 0] = IPUs[4, 0] = IPUs[6, 0] = SerializeFile(s, IPUs[2, 0], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[2, 1] = IPUs[3, 1] = IPUs[6, 1] = SerializeFile(s, IPUs[2, 1], 1, name: $"{nameof(IPUs)}[1]");
                    break;
                case 39:
                    IPUs[1, 0] = SerializeFile(s, IPUs[1, 0], 0, name: $"{nameof(IPUs)}[0]");
                    break;
                case 40:
                    IPUs[1, 0] = SerializeFile(s, IPUs[1, 0], 0, name: $"{nameof(IPUs)}[0]");
                    IPUs[1, 1] = SerializeFile(s, IPUs[1, 1], 1, name: $"{nameof(IPUs)}[1]");
                    IPUs[1, 2] = SerializeFile(s, IPUs[1, 2], 2, name: $"{nameof(IPUs)}[2]");
                    break;
                default:
                    break;
            }

            // Last file is a zero-length file most of the time
            if (ParsedFiles[ParsedFiles.Length - 1] == null) {
                DummyFile = SerializeFile(s, DummyFile, ParsedFiles.Length - 1, name: nameof(DummyFile));
            }
        }
    }
}