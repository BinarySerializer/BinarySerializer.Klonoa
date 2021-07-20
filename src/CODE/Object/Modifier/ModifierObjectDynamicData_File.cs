using BinarySerializer.PS1;
using System.Linq;

namespace BinarySerializer.KlonoaDTP
{
    public class ModifierObjectDynamicData_File : BaseFile
    {
        public int Pre_FileIndex { get; set; }
        public FileType Pre_FileType { get; set; }

        public PS1_TMD TMD { get; set; }
        public ObjTransform_ArchiveFile Transform { get; set; }
        public ObjPosition_File Position { get; set; }
        public PS1_TIM TIM { get; set; }
        public TIM_ArchiveFile TIMFiles { get; set; }
        public ModifierObjectUnkownData1_File Unknown_0 { get; set; }
        public ModifierObjectUnkownData0_File Unknown_1 { get; set; }
        public byte[] Raw { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            void onPreSerialize(BaseFile f)
            {
                f.Pre_FileSize = Pre_FileSize;
                f.Pre_IsCompressed = Pre_IsCompressed;
            }

            switch (GetFileType(s))
            {
                case FileType.TMD:
                    TMD = s.SerializeObject<PS1_TMD>(TMD, name: nameof(TMD));
                    s.Goto(Offset + Pre_FileSize);
                    break;

                case FileType.Transform:
                    Transform = s.SerializeObject<ObjTransform_ArchiveFile>(Transform, onPreSerialize: onPreSerialize, name: nameof(Transform));
                    break;

                case FileType.Position:
                    Position = s.SerializeObject<ObjPosition_File>(Position, onPreSerialize: onPreSerialize, name: nameof(Position));
                    break;

                case FileType.TIM:
                    TIM = s.SerializeObject<PS1_TIM>(TIM, name: nameof(TIM));
                    break;

                case FileType.TIMFiles:
                    TIMFiles = s.SerializeObject<TIM_ArchiveFile>(TIMFiles, onPreSerialize: onPreSerialize, name: nameof(TIMFiles));
                    break;

                case FileType.Unknown_0:
                    Unknown_0 = s.SerializeObject<ModifierObjectUnkownData1_File>(Unknown_0, onPreSerialize: onPreSerialize, name: nameof(Unknown_0));
                    break;

                case FileType.Unknown_1:
                    Unknown_1 = s.SerializeObject<ModifierObjectUnkownData0_File>(Unknown_1, onPreSerialize: onPreSerialize, name: nameof(Unknown_1));
                    break;

                case FileType.UnknownArchive:
                    s.SerializeObject<RawData_ArchiveFile>(default, onPreSerialize: onPreSerialize);
                    break;

                case FileType.UnknownArchiveArchive:
                    s.SerializeObject<ArchiveFile<RawData_ArchiveFile>>(default, onPreSerialize: onPreSerialize);
                    break;

                case FileType.Unknown:
                default:
                    Raw = s.SerializeArray<byte>(Raw, Pre_FileSize, name: nameof(Raw));
                    break;
            }
        }

        protected FileType GetFileType(SerializerObject s)
        {
            if (Pre_FileType != FileType.Unknown)
                return Pre_FileType;

            // The game defines 24 secondary types by using the type as an index in a function table. This table is located in the code files
            // for each level block and thus will differ. For Vision 1-1 NTSC the function pointer table is at 0x80110808. A lot of the pointers
            // are nulled out, so you would believe the actual indices themselves will be globally the same, with each level only implementing
            // functions for the used ones, but oddly enough the indices differ between levels.
            // Thus we have two options here, either we match the types with their global types for each level (a lot of work) or we dynamically
            // determine the referenced data for the object. The latter if the option I went with as it will allow the same code to be reused for
            // all versions of the game, such as demos and prototypes where the types might yet again differ.

            var int_00 = s.DoAt(s.CurrentPointer, () => s.Serialize<int>(default, name: "Check"));
            var int_04 = s.DoAt(s.CurrentPointer + 4, () => s.Serialize<int>(default, name: "Check"));

            // TMD (ID is 0x41, flags are always 0 in the file data)
            if (int_00 == 0x41 && int_04 == 0x00)
                return FileType.TMD;

            // Transform (an archive with 3 files, first file is always at 0x10 and the file size is always 0x28)
            if (int_00 == 0x03 && int_04 == 0x10 && Pre_FileSize == 0x28)
                return FileType.Transform;

            // TIM (ID is 0x10)
            if (int_00 == 0x10)
            {
                // Verify that length - 12 = width * height * 2
                var length = s.DoAt(s.CurrentPointer + 8, () => s.Serialize<int>(default, name: "Check"));
                var width = s.DoAt(s.CurrentPointer + 16, () => s.Serialize<ushort>(default, name: "Check"));
                var height = s.DoAt(s.CurrentPointer + 18, () => s.Serialize<ushort>(default, name: "Check"));

                if (length - 12 == width * height * 2)
                    return FileType.TIM;
            }

            // TODO: This fails
            // TIMFiles (archive with compressed TIM files)
            if (int_00 * 4 + 4 == int_04)
            {
                var file_0 = s.DoAt(s.CurrentPointer + int_04, () => s.Serialize<uint>(default, name: "Check"));

                if (file_0 == 0x10)
                {
                    // Verify that length - 12 = width * height * 2
                    var length = s.DoAt(s.CurrentPointer + int_04 + 8, () => s.Serialize<int>(default, name: "Check"));
                    var width = s.DoAt(s.CurrentPointer + int_04 + 16, () => s.Serialize<ushort>(default, name: "Check"));
                    var height = s.DoAt(s.CurrentPointer + int_04 + 18, () => s.Serialize<ushort>(default, name: "Check"));

                    if (length - 12 == width * height * 2)
                        return FileType.TIMFiles;
                }
            }

            // Unknown_0
            if ((int_00 & 0xFFFF) * 6 + 4 == Pre_FileSize)
                return FileType.Unknown_0;

            // Position
            if ((Pre_FileIndex == 1 || Pre_FileIndex == 2) && Pre_FileSize == 0x08)
                return FileType.Position;

            var int_last = s.DoAt(s.CurrentPointer + Pre_FileSize - 4, () => s.Serialize<int>(default, name: "Check"));

            // Unknown_1
            if (Pre_FileIndex == 0 && int_last == -1)
            {
                var ints = s.DoAt(s.CurrentPointer, () => s.SerializeArray<int>(default, Pre_FileSize / 4, "Check"));

                var isValid = ints.Select((x, i) => new { x, i }).Take(ints.Length - 1).Skip(1).All(x => x.x > ints[x.i - 1]);

                if (isValid)
                    return FileType.Unknown_1;
            }

            s.LogWarning($"Could not determine modifier file data at {Offset}");

            return FileType.Unknown;
        }

        // TODO: Name and move to files
        public class ModifierObjectUnkownData0_File : BaseFile
        {
            public int[] Offsets { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Offsets = s.SerializeArrayUntil(Offsets, x => x == -1, () => -1, name: nameof(Offsets));
            }
        }
        public class ModifierObjectUnkownData1_File : BaseFile
        {
            public short EntriesCount { get; set; }
            public short Short_02 { get; set; }
            public Entry[] Entries { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                EntriesCount = s.Serialize<short>(EntriesCount, name: nameof(EntriesCount));
                Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
                Entries = s.SerializeObjectArray<Entry>(Entries, EntriesCount, name: nameof(Entries));
            }

            public class Entry : BinarySerializable
            {
                public short Short_00 { get; set; }
                public short Short_02 { get; set; }
                public short Short_04 { get; set; }

                public override void SerializeImpl(SerializerObject s)
                {
                    Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
                    Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
                    Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
                }
            }
        }

        public enum FileType
        {
            Unknown,
            UnknownArchive,
            UnknownArchiveArchive,
            TMD,
            Transform,
            Position,
            TIM,
            TIMFiles,
            Unknown_0,
            Unknown_1,
        }
    }
}