using BinarySerializer.PS1;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BinarySerializer.KlonoaDTP
{
    // The game defines 24 secondary types by using the type as an index in a function table. This table is located in the code files
    // for each level block and thus will differ. For Vision 1-1 NTSC the function pointer table is at 0x80110808. A lot of the pointers
    // are nulled out, so you would believe the actual indices themselves will be globally the same, with each level only implementing
    // functions for the used ones, but oddly enough the indices differ between levels.
    // Thus we have two options here, either we match the types with their global types for each level (a lot of work) or we dynamically
    // determine the referenced data for the object. The latter if the option I went with as it will allow the same code to be reused for
    // all versions of the game, such as demos and prototypes where the types might yet again differ.

    public class ModifierObjectDynamicData_File : BaseFile
    {
        public int Pre_FileIndex { get; set; }
        public FileType[] Pre_Files { get; set; }

        public FileType DeterminedType { get; set; }

        public PS1_TMD TMD { get; set; }
        public ObjTransform_ArchiveFile Transform { get; set; }
        public ObjMultiTransform_ArchiveFile MultiTransform { get; set; }
        public ObjPosition Position { get; set; }
        public ObjCollisionItems_File Collision { get; set; }
        public PS1_TIM TIM { get; set; }
        public TIM_ArchiveFile TextureAnimation { get; set; }
        public ScenerySprites_File ScenerySprites { get; set; }
        public UVScrollAnimation_File UVScrollAnimation { get; set; }
        public byte[] Raw { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            void onPreSerialize(BaseFile f)
            {
                f.Pre_FileSize = Pre_FileSize;
                f.Pre_IsCompressed = Pre_IsCompressed;
            }

            switch (DeterminedType = GetFileType(s))
            {
                case FileType.TMD:
                    TMD = s.SerializeObject<PS1_TMD>(TMD, name: nameof(TMD));

                    if (TMD.ObjectsCount > 1)
                        s.LogWarning($"TMD in modifier data has multiple objects. Multiple positions needs to be parsed!");

                    s.Goto(Offset + Pre_FileSize);
                    break;

                case FileType.Transform:
                    Transform = s.SerializeObject<ObjTransform_ArchiveFile>(Transform, onPreSerialize: onPreSerialize, name: nameof(Transform));
                    break;

                case FileType.MultiTransform:
                    MultiTransform = s.SerializeObject<ObjMultiTransform_ArchiveFile>(MultiTransform, onPreSerialize: onPreSerialize, name: nameof(MultiTransform));
                    break;

                case FileType.Position:
                    Position = s.SerializeObject<ObjPosition>(Position, name: nameof(Position));
                    break;

                case FileType.Collision:
                    Collision = s.SerializeObject<ObjCollisionItems_File>(Collision, onPreSerialize: onPreSerialize, name: nameof(Collision));
                    break;

                case FileType.TIM:
                    TIM = s.SerializeObject<PS1_TIM>(TIM, name: nameof(TIM));
                    break;

                case FileType.TextureAnimation:
                    TextureAnimation = s.SerializeObject<TIM_ArchiveFile>(TextureAnimation, onPreSerialize: onPreSerialize, name: nameof(TextureAnimation));
                    break;

                case FileType.ScenerySprites:
                    ScenerySprites = s.SerializeObject<ScenerySprites_File>(ScenerySprites, onPreSerialize: onPreSerialize, name: nameof(ScenerySprites));
                    break;

                case FileType.UVScrollAnimation:
                    UVScrollAnimation = s.SerializeObject<UVScrollAnimation_File>(UVScrollAnimation, onPreSerialize: onPreSerialize, name: nameof(UVScrollAnimation));
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
            // Start by reading the first two values. We will need these for most of the match checks.
            var int_00 = s.DoAt(s.CurrentPointer, () => s.Serialize<int>(default, name: "Check"));
            var int_04 = s.DoAt(s.CurrentPointer + 4, () => s.Serialize<int>(default, name: "Check"));

            var possibleTypes = new HashSet<FileType>();

            // Enumerate every possible file type based on the matching structures
            foreach (var structure in FileTypeStructures)
            {
                // Make sure the files count matches
                if (structure.Length != Pre_Files.Length)
                    continue;

                // Make sure previously parsed files match
                var matches = true;
                for (int i = 0; i < Pre_FileIndex; i++)
                {
                    if (!matches)
                        break;

                    if (Pre_Files[i] != structure[i])
                        matches = false;
                }

                if (!matches)
                    continue;

                // Add the type
                possibleTypes.Add(structure[Pre_FileIndex]);
            }

            var type = FileType.Unknown;

            if (possibleTypes.Count == 1)
            {
                type = possibleTypes.First();
            }
            else if (possibleTypes.Count != 0)
            {
                // Check each possible type
                foreach (var fileTypeMatchFunc in FileTypeMatchFuncs.Where(x => possibleTypes.Contains(x.Key)))
                {
                    if (fileTypeMatchFunc.Value(s, int_00, int_04, Pre_FileSize))
                    {
                        type = fileTypeMatchFunc.Key;
                        break;
                    }
                }

                if (type == FileType.Unknown)
                    s.LogWarning($"Could not determine modifier file data at {Offset}");
            }
            else
            {
                s.LogWarning($"No matching file structure for modifier file data at {Offset}");
            }

            return type;
        }

        public enum FileType
        {
            Unknown,
            UnknownArchive,
            UnknownArchiveArchive,
            TMD,
            Transform,
            MultiTransform,
            Position,
            Collision,
            TIM,
            TextureAnimation,
            ScenerySprites,
            UVScrollAnimation,
        }

        private static FileType[][] FileTypeStructures { get; } = new FileType[][]
        {
            new FileType[] { FileType.UVScrollAnimation },
            new FileType[] { FileType.TextureAnimation },
            new FileType[] { FileType.ScenerySprites },
            new FileType[] { FileType.TMD },
            new FileType[] { FileType.TMD, FileType.Transform },
            new FileType[] { FileType.TMD, FileType.Position },
            new FileType[] { FileType.TMD, FileType.Unknown },
            new FileType[] { FileType.TMD, FileType.Transform, FileType.TIM },
            new FileType[] { FileType.TMD, FileType.TMD, FileType.Position },
            new FileType[] { FileType.TMD, FileType.Collision, FileType.Unknown, FileType.MultiTransform },
            new FileType[] { FileType.TMD, FileType.UnknownArchiveArchive, FileType.Unknown, FileType.Unknown, FileType.Unknown, FileType.UnknownArchive, FileType.UnknownArchive, },
        };

        [SuppressMessage("ReSharper", "ConvertToLambdaExpression")]
        private static KeyValuePair<FileType, FileTypeMatchCheck>[] FileTypeMatchFuncs { get; } = new KeyValuePair<FileType, FileTypeMatchCheck>[]
        {
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.TMD, (s, int_00, int_04, fileSize) =>
            {
                // TMD (ID is 0x41, flags are always 0 in the file data)
                return int_00 == 0x41 && int_04 == 0x00;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.Transform, (s, int_00, int_04, fileSize) =>
            {
                // Transform (an archive with 3 files, first file is always at 0x10 and the file size is always 0x28)
                return int_00 == 0x03 && int_04 == 0x10 && fileSize == 0x28;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.TIM, (s, int_00, int_04, fileSize) =>
            {
                // TIM (ID is 0x10)
                if (int_00 == 0x10)
                {
                    // Verify that length - 12 = width * height * 2
                    var length = s.DoAt(s.CurrentPointer + 8, () => s.Serialize<int>(default, name: "Check"));
                    var width = s.DoAt(s.CurrentPointer + 16, () => s.Serialize<ushort>(default, name: "Check"));
                    var height = s.DoAt(s.CurrentPointer + 18, () => s.Serialize<ushort>(default, name: "Check"));

                    if (length - 12 == width * height * 2)
                        return true;
                }

                return false;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.TextureAnimation, (s, int_00, int_04, fileSize) =>
            {
                // TextureAnimation (archive with compressed TIM files)
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
                            return true;
                    }
                }

                return false;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.ScenerySprites, (s, int_00, int_04, fileSize) =>
            {
                // ScenerySprites
                return (int_00 & 0xFFFF) * 6 + 4 == fileSize;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.Position, (s, int_00, int_04, fileSize) =>
            {
                // Position
                return fileSize == 0x08;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.Collision, (s, int_00, int_04, fileSize) =>
            {
                return int_00 * 28 + 4 == fileSize;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.UVScrollAnimation, (s, int_00, int_04, fileSize) =>
            {
                var int_last = s.DoAt(s.CurrentPointer + fileSize - 4, () => s.Serialize<int>(default, name: "Check"));

                // UVScrollAnimation
                if (int_last == -1)
                {
                    var ints = s.DoAt(s.CurrentPointer, () => s.SerializeArray<int>(default, fileSize / 4, "Check"));

                    var isValid = ints.Select((x, i) => new { x, i }).Take(ints.Length - 1).Skip(1).All(x => x.x > ints[x.i - 1]);

                    if (isValid)
                        return true;
                }

                return false;
            }),
        };

        private delegate bool FileTypeMatchCheck(SerializerObject s, int int_00, int int_04, long fileSize);
    }
}