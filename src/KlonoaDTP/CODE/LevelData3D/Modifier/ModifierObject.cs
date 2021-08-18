using System;
using System.Linq;
using System.Reflection;

namespace BinarySerializer.Klonoa.DTP
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
        
        // Custom
        public GlobalModifierType GlobalModifierType { get; set; }
        public ModifierRotationAttribute RotationAttribute { get; set; }

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

            var loader = Loader_DTP.GetLoader(s.Context);
            GlobalModifierType = loader.Config.GetGlobalModifierType(loader.BINBlock, (int)PrimaryType, SecondaryType);

            if (GlobalModifierType == GlobalModifierType.Unknown)
            {
                var count = DataFileIndices.Select((x, i) => new { x, i }).ToList().FindIndex(x => x.x == 0 && x.i > 0);
                s.LogWarning($"Unknown modifier at {Offset} with {count} data files");
                
                DataFiles ??= new ModifierObjectDynamicData_File[count];

                for (int i = 0; i < DataFiles.Length; i++)
                {
                    DataFiles[i] = Pre_AdditionalLevelFilePack.SerializeFile(s, DataFiles[i], DataFileIndices[i], onPreSerialize: x =>
                    {
                        x.Pre_FileType = GlobalModifierFileType.Unknown;
                    }, name: $"{nameof(DataFiles)}[{i}]");
                }

                return;
            }

            var modifierFiles = GetAttribute<ModifierFilesAttribute>(GlobalModifierType);

            if (modifierFiles == null)
                return;

            var files = modifierFiles.GetFiles().ToArray();

            DataFiles ??= new ModifierObjectDynamicData_File[files.Length];

            uint modelObjsCount = 0;

            for (int i = 0; i < DataFiles.Length; i++)
            {
                DataFiles[i] = Pre_AdditionalLevelFilePack.SerializeFile(s, DataFiles[i], DataFileIndices[i], onPreSerialize: x =>
                {
                    x.Pre_FileType = files[i];
                    x.Pre_ModelObjsCount = modelObjsCount;
                }, name: $"{nameof(DataFiles)}[{i}]");

                if (i == 0 && DataFiles[i].Pre_FileType == GlobalModifierFileType.TMD)
                    modelObjsCount = DataFiles[i].TMD.ObjectsCount;
            }

            RotationAttribute = GetAttribute<ModifierRotationAttribute>(GlobalModifierType);
        }

        private static T GetAttribute<T>(Enum value) 
            where T : Attribute
        {
            if (value == null)
                return null;

            // Get the member info for the value
            var memberInfo = value.GetType().GetMember(value.ToString());

            // Get the attribute
            var attributes = memberInfo.FirstOrDefault<MemberInfo>()?.GetCustomAttributes<T>(false);

            // Return the first attribute
            return attributes?.FirstOrDefault<T>();
        }
    }
}