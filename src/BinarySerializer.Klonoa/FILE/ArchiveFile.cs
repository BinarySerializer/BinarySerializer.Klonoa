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
        #region Private Fields

        /// <summary>
        /// The calculated file end pointers
        /// </summary>
        private Pointer[] _fileEndPointers;

        #endregion

        #region Public Static Properties

        public static bool AddToParsedArchiveFiles { get; set; } = false;
        public static Dictionary<ArchiveFile, bool[]> ParsedArchiveFiles { get; } = new Dictionary<ArchiveFile, bool[]>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Set in the OnPreSerialize action to specify the archive type
        /// </summary>
        public ArchiveFileType Pre_Type { get; set; }

        /// <summary>
        /// The parsed files and their names, if any
        /// </summary>
        public ParsedFile[] ParsedFiles { get; set; }

        /// <summary>
        /// The offset table for the contained files
        /// </summary>
        public OffsetTable OffsetTable { get; set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Serializes the files in the archive
        /// </summary>
        /// <param name="s">The serializer object</param>
        protected virtual void SerializeFiles(SerializerObject s) { }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the end pointer of the file of the specified index
        /// </summary>
        /// <param name="index">The index of the file</param>
        /// <returns>The end pointer, or null if it could not be determined</returns>
        public Pointer GetFileEndPointer(int index) => _fileEndPointers[index];

        /// <summary>
        /// Serializes a file within the archive. If the file is not of type <see cref="BaseFile"/> then it will not retain the encoder information
        /// (unless the encoder is explicitly set here using <see cref="fileEncoder"/>) which might be needed for writing the file.
        /// </summary>
        /// <typeparam name="T">The type of file to serialize. If the type is a <see cref="BaseFile"/> the pre_ values will be set.</typeparam>
        /// <param name="s">The serializer object</param>
        /// <param name="obj">The object</param>
        /// <param name="index">The file index</param>
        /// <param name="logIfNotFullyParsed">Indicates if a warning log should be created if not fully parsed</param>
        /// <param name="onPreSerialize">Optional action to call before serializing</param>
        /// <param name="fileEncoder">An optional file encoder if the file is encoded</param>
        /// <param name="name">The name</param>
        /// <returns>The serialized object</returns>
        public T SerializeFile<T>(SerializerObject s, T obj, int index, bool logIfNotFullyParsed = true, Action<T> onPreSerialize = null, IStreamEncoder fileEncoder = null, string name = null)
            where T : BinarySerializable, new()
        {
            if (index >= OffsetTable.FilePointers.Length)
            {
                s.LogWarning($"File {index} of requested type {typeof(T).Name} does not exist in archive at {Offset}");
                return obj;
            }

            s.DoAt(OffsetTable.FilePointers[index], () =>
            {
                var endPointer = GetFileEndPointer(index);

                var file = s.SerializeObject<FileWrapper<T>>(new FileWrapper<T>(obj), x =>
                {
                    x.Pre_EndPointer = endPointer;
                    x.Pre_LogIfNotFullyParsed = logIfNotFullyParsed && !DisableNotFullySerializedWarning;
                    x.Pre_OnPreSerialize = onPreSerialize;
                    x.Pre_FileEncoder = fileEncoder;
                    x.Pre_ParentArchiveType = Pre_Type;
                }, name: name);

                obj = file.FileData;

                FlagAsParsed(index, obj, name);
            });

            return obj;
        }

        public void FlagAsParsed(int index, BinarySerializable file, string name)
        {
            ParsedFiles[index] = new ParsedFile(file, name);

            if (!AddToParsedArchiveFiles)
                return;

            if (!ParsedArchiveFiles.ContainsKey(this))
                ParsedArchiveFiles[this] = new bool[OffsetTable.FilesCount];

            ParsedArchiveFiles[this][index] = true;
        }

        /// <summary>
        /// Calculates the end pointers for each file in the archive. This updates what is returned by <see cref="GetFileEndPointer"/> and is used
        /// to determine the file sizes. Only call this if a file size has been changed or the offset table has been modified.
        /// </summary>
        public void CalculateFileEndPointers()
        {
            if (Pre_Type == ArchiveFileType.KH_PF)
            {
                _fileEndPointers = Enumerable.Range(0, OffsetTable.FilesCount).Select(i => OffsetTable.FilePointers[i] + OffsetTable.KH_PF_FileSizes[i]).ToArray();

                return;
            }

            KlonoaSettings settings = Context.GetKlonoaSettings(false);
            HashSet<KlonoaSettings.RelocatedFile> relocatedFiles = null;
            settings?.RelocatedFiles?.TryGetValue(Offset, out relocatedFiles);

            IEnumerable<Pointer> filePointers = OffsetTable.FilePointers.Where(x => x != null);

            if (relocatedFiles != null)
            {
                // Replace any relocated pointer with their original pointer if it still exists, otherwise remove it. A relocated pointer
                // can't be used to determine the size of the files.
                filePointers = filePointers.Select(pointer =>
                {
                    KlonoaSettings.RelocatedFile r = relocatedFiles.FirstOrDefault(x => x.NewPointer == pointer);
                    return r == null ? pointer : r.OriginalPointer;
                }).Where(x => x != null);
            }

            // KW archives have an end pointer we can use to get the size for the last file
            if (Pre_Type == ArchiveFileType.KH_KW)
                filePointers = filePointers.Append(OffsetTable.KH_KW_SharedDataPointer);

            // Although they usually are the files don't have to be located in the order they're indexed in. Some archives for example
            // have "empty" files reference a dummy file with index 0 or 1.
            Pointer[] sortedFilePointers = filePointers.OrderBy(x => x.FileOffset).ToArray();

            _fileEndPointers = new Pointer[OffsetTable.FilesCount];

            for (int i = 0; i < OffsetTable.FilesCount; i++)
            {
                Pointer start = OffsetTable.FilePointers[i];

                if (start == null)
                {
                    _fileEndPointers[i] = null;
                    continue;
                }

                // If the file has been relocated we get the specified file size
                KlonoaSettings.RelocatedFile r = relocatedFiles?.FirstOrDefault(x => x.NewPointer == start);

                if (r != null)
                {
                    uint size = r.FileSize;

                    // Align
                    if (size % 4 != 0)
                        size += 4 - size % 4;

                    _fileEndPointers[i] = start + size;
                    continue;
                }

                // Try getting the end pointer by getting the pointer of the next file
                Pointer end = sortedFilePointers.FirstOrDefault(x => x.FileOffset > start.FileOffset);

                // If we found an end pointer we use that (i.e. this is not the last file)
                if (end != null)
                    _fileEndPointers[i] = end;
                // If we have a file size of the archive we can use that to determine the size of the last file
                else if (Pre_FileSize != -1)
                    _fileEndPointers[i] = OffsetTable.Offset + Pre_FileSize;
                // If we don't have a file size of the archive we can't determine the file size of the last file
                else
                    _fileEndPointers[i] = null;
            }
        }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize the offset table
            OffsetTable = s.SerializeObject<OffsetTable>(OffsetTable, x => x.Pre_Type = Pre_Type, name: nameof(OffsetTable));

            ParsedFiles = new ParsedFile[OffsetTable.FilesCount];

            if (AddToParsedArchiveFiles)
                ParsedArchiveFiles[this] = new bool[OffsetTable.FilesCount];

            CalculateFileEndPointers();

            // Serialize the files
            SerializeFiles(s);

            // Go to the end of the archive if a length is specified
            if (Pre_FileSize != -1)
                s.Goto(Offset + Pre_FileSize);
        }

        #endregion

        #region Data Types

        public class ParsedFile
        {
            public ParsedFile(BinarySerializable obj, string name)
            {
                Obj = obj;
                Name = name;
            }

            public BinarySerializable Obj { get; }
            public string Name { get; }
        }

        #endregion
    }

    /// <summary>
    /// A file which contains multiple files of the same type. All the contained files will automatically be serialized.
    /// </summary>
    /// <typeparam name="T">The type of file</typeparam>
    public class ArchiveFile<T> : ArchiveFile
        where T : BinarySerializable, new()
    {
        public Action<T> Pre_OnPreSerializeAction { get; set; }
        public IStreamEncoder Pre_ArchivedFilesEncoder { get; set; }

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
                Files[i] = SerializeFile<T>(s, Files[i], i, onPreSerialize: OnPreSerialize, fileEncoder: Pre_ArchivedFilesEncoder, name: $"{nameof(Files)}[{i}]");
        }
    }
}