using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySerializer.Klonoa
{
    /// <summary>
    /// A file which contains multiple files. The files can be compressed and of different types.
    /// </summary>
    public class ArchiveFile : BaseFile
    {
        public static bool AddToParsedArchiveFiles { get; set; } = false;
        public static Dictionary<ArchiveFile, bool[]> ParsedArchiveFiles { get; } = new Dictionary<ArchiveFile, bool[]>();

        /// <summary>
        /// The parsed files and their names, if any
        /// </summary>
        public (BinarySerializable, string)[] ParsedFiles { get; set; }

        /// <summary>
        /// The offset table for the contained files
        /// </summary>
        public OffsetTable OffsetTable { get; set; }

        /// <summary>
        /// Gets the end pointer of the file of the specified index
        /// </summary>
        /// <param name="index">The index of the file</param>
        /// <returns>The end pointer</returns>
        public Pointer GetFileEndPointer(int index)
        {
            var start = OffsetTable.FilePointers[index];
            var end = OffsetTable.FilePointers.Where(x => x != null).OrderBy(x => x.FileOffset).FirstOrDefault(x => x.FileOffset > start.FileOffset);

            if (end != null) 
                return end;
            
            // If we don't have a file size of the archive we can't determine the file size of the last file
            if (Pre_FileSize == -1)
                return null;

            return OffsetTable.Offset + Pre_FileSize;
        }

        /// <summary>
        /// Serializes a file within in the archive
        /// </summary>
        /// <typeparam name="T">The type of file to serialize. If the type is a <see cref="BaseFile"/> the pre_ values will be set.</typeparam>
        /// <param name="s">The serializer object</param>
        /// <param name="obj">The object</param>
        /// <param name="index">The file index</param>
        /// <param name="logIfNotFullyParsed">Indicates if a warning log should be created if not fully parsed</param>
        /// <param name="onPreSerialize">Optional action to call before serializing</param>
        /// <param name="name">The name</param>
        /// <returns>The serialized object</returns>
        public T SerializeFile<T>(SerializerObject s, T obj, int index, bool logIfNotFullyParsed = true, Action<T> onPreSerialize = null, string name = null)
            where T : BinarySerializable, new()
        {
            if (index >= OffsetTable.FilePointers.Length)
            {
                s.LogWarning($"File {index} of requested type {typeof(T).Name} does not exist in archive at {Offset}");
                return null;
            }

            s.DoAt(OffsetTable.FilePointers[index], () =>
            {
                var endPointer = GetFileEndPointer(index);

                // TODO: Allow writing back the same object again
                var file = s.SerializeObject<FileWrapper<T>>(default, x =>
                {
                    x.Pre_EndPointer = endPointer;
                    x.Pre_LogIfNotFullyParsed = logIfNotFullyParsed && !DisableNotFullySerializedWarning;
                    x.Pre_OnPreSerialize = onPreSerialize;
                }, name: name);

                obj = file.FileData;

                FlagAsParsed(index, obj, name);
            });

            return obj;
        }

        public void FlagAsParsed(int index, BinarySerializable file, string name)
        {
            ParsedFiles[index] = (file, name);

            if (!AddToParsedArchiveFiles) 
                return;
            
            if (!ParsedArchiveFiles.ContainsKey(this))
                ParsedArchiveFiles[this] = new bool[OffsetTable.FilesCount];

            ParsedArchiveFiles[this][index] = true;
        }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize the offset table
            OffsetTable = s.SerializeObject<OffsetTable>(OffsetTable, name: nameof(OffsetTable));

            ParsedFiles = new (BinarySerializable, string)[OffsetTable.FilesCount];

            if (AddToParsedArchiveFiles)
                ParsedArchiveFiles[this] = new bool[OffsetTable.FilesCount];

            // Serialize the files
            SerializeFiles(s);

            // Go to the end of the archive if a length is specified
            if (Pre_FileSize != -1)
                s.Goto(Offset + Pre_FileSize);
        }

        protected virtual void SerializeFiles(SerializerObject s) { }

        /// <summary>
        /// A file wrapper when serializing files from an archive to fix files not being cached when compressed
        /// </summary>
        /// <typeparam name="File">The file type</typeparam>
        protected class FileWrapper<File> : BinarySerializable
            where File : BinarySerializable, new()
        {
            public Pointer Pre_EndPointer { get; set; }
            public bool Pre_LogIfNotFullyParsed { get; set; }
            public Action<File> Pre_OnPreSerialize { get; set; }

            public File FileData { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                KlonoaSettings settings = s.Context.GetKlonoaSettings(throwIfNotFound: false);

                bool isCompressed = false;
                IStreamEncoder encoder = null;

                if (settings?.Version == KlonoaGameVersion.DTP_Prototype_19970717 ||
                    settings?.Version == KlonoaGameVersion.DTP)
                {
                    uint header = s.DoAt(s.CurrentPointer, () => s.Serialize<uint>(default, "HeaderCheck"));
                    isCompressed = header == ULZEncoder.Header;
                    encoder = new ULZEncoder();
                }

                s.DoEncodedIf(encoder, isCompressed, () =>
                {
                    long fileSize;
                    
                    if (isCompressed)
                        fileSize = s.CurrentLength;
                    else
                        fileSize = Pre_EndPointer != null ? Pre_EndPointer - s.CurrentPointer : -1;

                    s.Log($"FileSize: {fileSize}");

                    FileData = s.SerializeObject<File>(FileData, x =>
                    {
                        if (x is BaseFile f)
                        {
                            f.Pre_FileSize = fileSize;
                            f.Pre_IsCompressed = isCompressed;
                        }

                        Pre_OnPreSerialize?.Invoke(x);
                    }, name: nameof(FileData));
                }, allowLocalPointers: true);

                if (settings?.Version == KlonoaGameVersion.LV)
                    s.Align(alignBytes: 16);
                else
                    s.Align(alignBytes: 4);

                if (Pre_LogIfNotFullyParsed && 
                    s.CurrentPointer != Pre_EndPointer && 
                    Pre_EndPointer != null && 
                    !(FileData is BaseFile { DisableNotFullySerializedWarning: true }))
                {
                    s.LogWarning($"Archived file of type {typeof(File).Name} at {Offset} was not fully serialized. {s.CurrentPointer} != {Pre_EndPointer}");
                }
            }
        }
    }

    /// <summary>
    /// A file which contains multiple files of the same type. All the contained files will automatically be serialized.
    /// </summary>
    /// <typeparam name="T">The type of file</typeparam>
    public class ArchiveFile<T> : ArchiveFile
        where T : BinarySerializable, new()
    {
        public Action<T> Pre_OnPreSerializeAction { get; set; }

        /// <summary>
        /// The files
        /// </summary>
        public T[] Files { get; set; }

        protected virtual void OnPreSerialize(T obj)
        {
            Pre_OnPreSerializeAction?.Invoke(obj);
        }

        protected override void SerializeFiles(SerializerObject s)
        {
            Files ??= new T[OffsetTable.FilesCount];

            for (int i = 0; i < Files.Length; i++)
                Files[i] = SerializeFile<T>(s, Files[i], i, onPreSerialize: OnPreSerialize, name: $"{nameof(Files)}[{i}]");
        }
    }
}