using System.Linq;
using System.Text;

namespace BinarySerializer.KlonoaDTP
{
    public class ModifierObject : BinarySerializable
    {
        public ArchiveFile Pre_AdditionalLevelFilePack { get; set; }

        public short Short_00 { get; set; }
        public short Short_02 { get; set; }
        public int Int_04 { get; set; }
        public PrimaryObjectType PrimaryType { get; set; }
        public short SecondaryType { get; set; }
        public short Short_0C { get; set; }
        public short Short_0E { get; set; }
        public uint Uint_10 { get; set; }
        public Pointer DataFileIndicesPointer { get; set; }
        public short Short_18 { get; set; }
        public short Short_1A { get; set; } // Seems to be used in memory to indicate if it's been loaded

        // Serialized from pointers
        public ushort[] DataFileIndices { get; set; }

        // Serialized from data files
        public ModifierObjectDynamicData_File[] DataFiles { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
            Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
            Int_04 = s.Serialize<int>(Int_04, name: nameof(Int_04));
            PrimaryType = s.Serialize<PrimaryObjectType>(PrimaryType, name: nameof(PrimaryType));
            SecondaryType = s.Serialize<short>(SecondaryType, name: nameof(SecondaryType));
            Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
            Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));
            Uint_10 = s.Serialize<uint>(Uint_10, name: nameof(Uint_10));
            DataFileIndicesPointer = s.SerializePointer(DataFileIndicesPointer, name: nameof(DataFileIndicesPointer));
            Short_18 = s.Serialize<short>(Short_18, name: nameof(Short_18));
            Short_1A = s.Serialize<short>(Short_1A, name: nameof(Short_1A));

            s.DoAt(DataFileIndicesPointer, () => DataFileIndices = s.SerializeArray<ushort>(DataFileIndices, 8, name: nameof(DataFileIndices)));
        }

        public void SerializeDataFiles(SerializerObject s)
        {
            if (PrimaryType == PrimaryObjectType.Invalid ||
                PrimaryType == PrimaryObjectType.None ||
                SecondaryType == -1 ||
                SecondaryType == 0)
                return;

            if (DataFileIndicesPointer == null)
            {
                DataFiles = new ModifierObjectDynamicData_File[0];
                s.LogWarning($"Modifier of primary type {PrimaryType} and secondary type {SecondaryType} has no data");
                return;
            }

            if (_logToStringBuilder)
            {
                for (int i = 0; i < DataFileIndices.Length; i++)
                {
                    // Assume repeated file 0 are padding
                    if (i > 0 && DataFileIndices[i] == 0)
                        break;

                    // Read as raw data
                    var rawFileData = Pre_AdditionalLevelFilePack.SerializeFile<RawData_File>(s, default, DataFileIndices[i]);

                    DebugStringBuilder.AppendLine($"Type {SecondaryType:00} | File[{i}] {DataFileIndices[i]:00} | Length 0x{rawFileData.Pre_FileSize:X8} | Header {rawFileData.Data.ToHexString(align: 16, maxLines: 1)}");
                }

                return;
            }

            if (PrimaryType == PrimaryObjectType.Modifier_41)
            {
                // Start by getting the amount of referenced data. We assume file 0 is never the last file. Unused files are always padded with 0.
                var count = DataFileIndices.Select((x, i) => new { x, i }).ToList().FindIndex(x => x.x == 0 && x.i > 0);

                if (DataFileIndices.Skip(count).Any(x => x != 0))
                    s.LogWarning($"A data reference got skipped!");

                DataFiles ??= new ModifierObjectDynamicData_File[count];

                for (int i = 0; i < count; i++)
                    DataFiles[i] = Pre_AdditionalLevelFilePack.SerializeFile(s, DataFiles[i], DataFileIndices[i], onPreSerialize: x => x.Pre_FileIndex = i, name: $"{nameof(DataFiles)}[{i}]");
            }
            else
            {
                s.LogWarning($"Modifier has unsupported primary type {PrimaryType}");
            }
        }

        // Used for debugging obj types
        public static StringBuilder DebugStringBuilder = new StringBuilder();
        private bool _logToStringBuilder = false;
    }
}