using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySerializer.Klonoa.DTP
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ModifierFilesAttribute : Attribute
    {
        public ModifierFilesAttribute(
            GlobalModifierFileType file_0 = GlobalModifierFileType.None,
            GlobalModifierFileType file_1 = GlobalModifierFileType.None,
            GlobalModifierFileType file_2 = GlobalModifierFileType.None,
            GlobalModifierFileType file_3 = GlobalModifierFileType.None,
            GlobalModifierFileType file_4 = GlobalModifierFileType.None,
            GlobalModifierFileType file_5 = GlobalModifierFileType.None,
            GlobalModifierFileType file_6 = GlobalModifierFileType.None,
            GlobalModifierFileType file_7 = GlobalModifierFileType.None,
            GlobalModifierFileType file_8 = GlobalModifierFileType.None)
        {
            File_0 = file_0;
            File_1 = file_1;
            File_2 = file_2;
            File_3 = file_3;
            File_4 = file_4;
            File_5 = file_5;
            File_6 = file_6;
            File_7 = file_7;
            File_8 = file_8;
        }

        public GlobalModifierFileType File_0 { get; }
        public GlobalModifierFileType File_1 { get; }
        public GlobalModifierFileType File_2 { get; }
        public GlobalModifierFileType File_3 { get; }
        public GlobalModifierFileType File_4 { get; }
        public GlobalModifierFileType File_5 { get; }
        public GlobalModifierFileType File_6 { get; }
        public GlobalModifierFileType File_7 { get; }
        public GlobalModifierFileType File_8 { get; }

        public IEnumerable<GlobalModifierFileType> GetFiles() => GetAllFiles().TakeWhile(x => x != GlobalModifierFileType.None);

        public IEnumerable<GlobalModifierFileType> GetAllFiles()
        {
            yield return File_0;
            yield return File_1;
            yield return File_2;
            yield return File_3;
            yield return File_4;
            yield return File_5;
            yield return File_6;
            yield return File_7;
        }
    }
}