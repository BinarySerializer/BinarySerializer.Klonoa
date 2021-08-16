using BinarySerializer.PS1;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace BinarySerializer.Klonoa.DTP
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
        public ModifierObjectDynamicData_File[] Pre_Files { get; set; }

        public FileType DeterminedType { get; set; }

        public PS1_TMD TMD { get; set; }
        public ObjTransform_ArchiveFile Transform { get; set; }
        public ArchiveFile<ObjTransform_ArchiveFile> Transforms { get; set; }
        public ObjPosition Position { get; set; }
        public ObjCollisionItems_File Collision { get; set; }
        public PS1_TIM TIM { get; set; }
        public TIM_ArchiveFile TextureAnimation { get; set; }
        public ScenerySprites_File ScenerySprites { get; set; }
        public UVScrollAnimation_File UVScrollAnimation { get; set; }
        public UnknownModelObjectsData_File UnknownModelObjectsData { get; set; }
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
                    s.Goto(Offset + Pre_FileSize);
                    break;

                case FileType.Transform_WithInfo:
                    Transform = s.SerializeObject<ObjTransform_ArchiveFile>(Transform, onPreSerialize: x =>
                    {
                        onPreSerialize(x);
                        x.Pre_UsesTransformInfo = true;
                    }, name: nameof(Transform));
                    break;

                case FileType.Transform_WithoutInfo:
                    Transform = s.SerializeObject<ObjTransform_ArchiveFile>(Transform, onPreSerialize: x =>
                    {
                        onPreSerialize(x);
                        x.Pre_UsesTransformInfo = false;
                    }, name: nameof(Transform));
                    break;

                case FileType.Transforms_WithInfo:
                    Transforms = s.SerializeObject<ArchiveFile<ObjTransform_ArchiveFile>>(Transforms, onPreSerialize: x =>
                    {
                        onPreSerialize(x);
                        x.Pre_OnPreSerializeAction = obj => obj.Pre_UsesTransformInfo = true;
                    }, name: nameof(Transforms));
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

                case FileType.UnknownModelObjectsData:
                    UnknownModelObjectsData = s.SerializeObject<UnknownModelObjectsData_File>(UnknownModelObjectsData, onPreSerialize: x =>
                    {
                        onPreSerialize(x);
                        x.Pre_ObjsCount = Pre_Files[0].TMD.ObjectsCount;
                    }, name: nameof(UnknownModelObjectsData));
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

                    if (Pre_Files[i].DeterminedType != structure[i])
                        matches = false;
                }

                if (!matches)
                    continue;

                // Add the type
                possibleTypes.Add(structure[Pre_FileIndex]);
            }

            var type = FileType.Unknown;

            //if (possibleTypes.Count == 1)
            //{
            //    type = possibleTypes.First();

            //    if (type == FileType.Unknown || type == FileType.UnknownArchive || type == FileType.UnknownArchiveArchive)
            //        s.LogWarning($"Could not determine modifier file data at {Offset}");
            //}
            //else if (possibleTypes.Count != 0)
            if (possibleTypes.Count != 0)
            {
                // Check each possible type
                foreach (var fileTypeMatchFunc in FileTypeMatchFuncs.Where(x => possibleTypes.Contains(x.Key)))
                {
                    if (fileTypeMatchFunc.Value(s, int_00, int_04, Pre_FileSize, Pre_Files))
                    {
                        type = fileTypeMatchFunc.Key;
                        break;
                    }
                }

                if (type == FileType.Unknown || type == FileType.UnknownArchive || type == FileType.UnknownArchiveArchive)
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
            Transform_WithInfo,
            Transform_WithoutInfo,
            Transforms_WithInfo,
            Position,
            Collision,
            TIM,
            TextureAnimation,
            ScenerySprites,
            UVScrollAnimation,
            UnknownModelObjectsData,
        }

        private static FileType[][] FileTypeStructures { get; } = new FileType[][]
        {
            new FileType[] { FileType.UVScrollAnimation },
            new FileType[] { FileType.TextureAnimation },
            new FileType[] { FileType.ScenerySprites },
            new FileType[] { FileType.UnknownArchive },
            new FileType[] { FileType.TMD },
            new FileType[] { FileType.TMD, FileType.Transform_WithInfo },
            new FileType[] { FileType.TMD, FileType.Transform_WithoutInfo },
            new FileType[] { FileType.TMD, FileType.Position },
            new FileType[] { FileType.TMD, FileType.UnknownModelObjectsData },
            new FileType[] { FileType.TMD, FileType.Transform_WithInfo, FileType.TIM },
            new FileType[] { FileType.TMD, FileType.TMD, FileType.Position },

            // TODO: Unknown has 56 unknown bytes (28 + 28). Appears in level 3-2.
            new FileType[] { FileType.TMD, FileType.Collision, FileType.Unknown, FileType.Transform_WithoutInfo },

            // TODO: First unknown has 84 unknown bytes (28 + 28 + 28). Appears in level 4-2.
            // TODO: Second unknown has 564 bytes
            new FileType[] { FileType.TMD, FileType.Collision, FileType.Unknown, FileType.Transform_WithoutInfo, FileType.Unknown },
            
            // Mine-cart (4-4)
            // TODO: First unknown has 56 unknown bytes (28 + 28)
            // TODO: Second unknown is same structure as second in previous
            new FileType[] { FileType.TMD, FileType.Collision, FileType.Unknown, FileType.Transforms_WithInfo, FileType.Unknown, FileType.Transform_WithInfo },

            //new FileType[] { FileType.TMD, FileType.UnknownArchiveArchive, FileType.Unknown, FileType.Unknown, FileType.Unknown, FileType.UnknownArchive, FileType.UnknownArchive, },
        };

        [SuppressMessage("ReSharper", "ConvertToLambdaExpression")]
        private static KeyValuePair<FileType, FileTypeMatchCheck>[] FileTypeMatchFuncs { get; } = new KeyValuePair<FileType, FileTypeMatchCheck>[]
        {
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.TMD, (s, int_00, int_04, fileSize, files) =>
            {
                // TMD (ID is 0x41, flags are always 0 in the file data)
                return int_00 == 0x41 && int_04 == 0x00;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.Transform_WithInfo, (s, int_00, int_04, fileSize, files) =>
            {
                // Transform_WithInfo (an archive with 3 files, first file is always at 0x10)
                if (int_00 == 0x03 && int_04 == 0x10)
                {
                    if (files[0].TMD.ObjectsCount == 1)
                        return fileSize == 0x28;
                    else
                        return true;
                }

                return false; 
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.TIM, (s, int_00, int_04, fileSize, files) =>
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
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.TextureAnimation, (s, int_00, int_04, fileSize, files) =>
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
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.ScenerySprites, (s, int_00, int_04, fileSize, files) =>
            {
                var size = (int_00 & 0xFFFF) * 6 + 4;

                if ((size % 4) != 0)
                    size += 4 - (size % 4);

                // ScenerySprites
                return size == fileSize;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.Position, (s, int_00, int_04, fileSize, files) =>
            {
                // Position
                return fileSize == 0x08;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.Collision, (s, int_00, int_04, fileSize, files) =>
            {
                return int_00 * 28 + 4 == fileSize;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.UVScrollAnimation, (s, int_00, int_04, fileSize, files) =>
            {
                var int_last = s.DoAt(s.CurrentPointer + fileSize - 4, () => s.Serialize<int>(default, name: "Check"));

                // UVScrollAnimation
                if (int_last == -1)
                {
                    var ints = s.DoAt(s.CurrentPointer, () => s.SerializeArray<int>(default, fileSize / 4, name: "Check"));

                    var isValid = ints.Select((x, i) => new { x, i }).Take(ints.Length - 1).Skip(1).All(x => x.x > ints[x.i - 1]);

                    if (isValid)
                        return true;
                }

                return false;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.Transform_WithoutInfo, (s, int_00, int_04, fileSize, files) =>
            {
                if (int_00 != 3)
                    return false;

                if (int_04 != 16)
                    return false;

                var offsets = s.DoAt(s.CurrentPointer + 4, () => s.SerializeArray<int>(default, 3, name: "Check"));

                var file1Length = offsets[2] - offsets[1];
                var file1_ushort_02 = s.DoAt(s.CurrentPointer + offsets[1] + 2, () => s.Serialize<ushort>(default, name: "Check"));

                var size = file1_ushort_02 * 6 + 4;

                if ((size % 4) != 0)
                    size += 4 - (size % 4);

                // Transform_WithoutInfo
                return size == file1Length;

            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.UnknownArchive, (s, int_00, int_04, fileSize, files) =>
            {
                // UnknownArchive
                return int_00 * 4 + 4 == int_04;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.UnknownArchiveArchive, (s, int_00, int_04, fileSize, files) =>
            {
                var isArchive = int_00 * 4 + 4 == int_04;

                if (!isArchive)
                    return false;

                // UnknownArchiveArchive
                return s.DoAt(s.CurrentPointer + int_04, () =>
                {
                    var count = s.Serialize<int>(default, "Check");
                    var offset0 = s.Serialize<int>(default, "Check");

                    return count * 4 + 4 == offset0;
                });
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.UnknownModelObjectsData, (s, int_00, int_04, fileSize, files) =>
            {
                // UnknownModelObjectsData
                return files[0].TMD != null && fileSize == files[0].TMD.ObjectsCount * 8 * 4;
            }),
            new KeyValuePair<FileType, FileTypeMatchCheck>(FileType.Transforms_WithInfo, (s, int_00, int_04, fileSize, files) =>
            {
                // Transforms_WithInfo
                return int_00 * 4 + 4 == int_04;
            }),
        };

        private delegate bool FileTypeMatchCheck(SerializerObject s, int int_00, int int_04, long fileSize, ModifierObjectDynamicData_File[] files);
    }
}