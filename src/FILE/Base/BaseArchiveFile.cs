﻿using System.Linq;

namespace BinarySerializer.KlonoaDTP
{
    /// <summary>
    /// A file which contains multiple files. The files can be compressed and of different types.
    /// </summary>
    public abstract class BaseArchiveFile : BaseFile
    {
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
            var end = OffsetTable.FilePointers.OrderBy(x => x.FileOffset).FirstOrDefault(x => x.FileOffset > start.FileOffset);

            if (end == null)
                end = Offset + Pre_FileSize;

            return end;
        }

        /// <summary>
        /// Serializes a file within in the archive
        /// </summary>
        /// <typeparam name="T">The type of file to serialize. If the type is a <see cref="BaseFile"/> the pre_ values will be set.</typeparam>
        /// <param name="s">The serializer object</param>
        /// <param name="obj">The object</param>
        /// <param name="index">The file index</param>
        /// <param name="name">The name</param>
        /// <returns>The serialized object</returns>
        public T SerializeFile<T>(SerializerObject s, T obj, int index, string name = null)
            where T : BinarySerializable, new()
        {
            s.DoAt(OffsetTable.FilePointers[index], () =>
            {
                var endPointer = GetFileEndPointer(index);

                // TODO: Allow writing back the same object again
                var file = s.SerializeObject<FileWrapper<T>>(default, x => x.Pre_EndPointer = endPointer, name: name);

                obj = file.FileData;
            });

            return obj;
        }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize the offset table
            OffsetTable = s.SerializeObject<OffsetTable>(OffsetTable, name: nameof(OffsetTable));

            // Serialize the files
            SerializeFiles(s);

            // Go to the end of the archive
            s.Goto(Offset + Pre_FileSize);
        }

        protected abstract void SerializeFiles(SerializerObject s);

        /// <summary>
        /// A file wrapper when serializing files from an archive to fix files not being cached when compressed
        /// </summary>
        /// <typeparam name="File">The file type</typeparam>
        protected class FileWrapper<File> : BinarySerializable
            where File : BinarySerializable, new()
        {
            public Pointer Pre_EndPointer { get; set; }

            public File FileData { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                var header = s.DoAt(s.CurrentPointer, () => s.Serialize<uint>(default, "HeaderCheck"));

                var isCompressed = header == ULZEncoder.Header;

                s.DoEncodedIf(new ULZEncoder(), isCompressed, () =>
                {
                    var fileSize = isCompressed ? s.CurrentLength : Pre_EndPointer - s.CurrentPointer;

                    FileData = s.SerializeObject<File>(FileData, x =>
                    {
                        if (x is BaseFile f)
                        {
                            f.Pre_FileSize = fileSize;
                            f.Pre_IsCompressed = isCompressed;
                        }
                    }, name: nameof(FileData));
                });

                s.Align();

                if (s.CurrentPointer != Pre_EndPointer)
                    s.LogWarning($"Archived file of type {typeof(File).Name} at {Offset} was not fully serialized. {s.CurrentPointer} != {Pre_EndPointer}");
            }
        }
    }

    /// <summary>
    /// A file which contains multiple files of the same type. All the contained files will automatically be serialized.
    /// </summary>
    /// <typeparam name="T">The type of file</typeparam>
    public class ArchiveFile<T> : BaseArchiveFile
        where T : BinarySerializable, new()
    {
        /// <summary>
        /// The files
        /// </summary>
        public T[] Files { get; set; }

        protected virtual void OnPreSerialize(T obj, long fileSize) { }

        protected override void SerializeFiles(SerializerObject s)
        {
            Files ??= new T[OffsetTable.FilesCount];

            for (int i = 0; i < Files.Length; i++)
                Files[i] = SerializeFile<T>(s, Files[i], i, name: $"{nameof(Files)}[{i}]");
        }
    }
}