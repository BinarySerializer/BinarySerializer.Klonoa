using System;

namespace BinarySerializer.Klonoa
{
    /// <summary>
    /// A file wrapper when serializing files from an archive to fix files not being cached when compressed
    /// </summary>
    /// <typeparam name="File">The file type</typeparam>
    internal class FileWrapper<File> : BinarySerializable
        where File : BinarySerializable, new()
    {
        public FileWrapper() { }
        public FileWrapper(File fileData) => FileData = fileData;

        public Pointer Pre_EndPointer { get; set; }
        public bool Pre_LogIfNotFullyParsed { get; set; }
        public Action<File> Pre_OnPreSerialize { get; set; }
        public IStreamEncoder Pre_FileEncoder { get; set; }
        public ArchiveFileType Pre_ParentArchiveType { get; set; }

        public File FileData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            KlonoaSettings settings = s.Context.GetKlonoaSettings(throwIfNotFound: false);

            IStreamEncoder encoder = null;

            // If the file is not null (we're serializing/writing the file after it has already been deserialized), then use the existing values
            if (FileData is BaseFile file)
                encoder = file.Pre_FileEncoder;

            // If a file encoder is specified we always use that
            if (Pre_FileEncoder != null)
                encoder = Pre_FileEncoder;

            // If the game is DTP we check for the ULZ header (unless the file has already been serialized).
            // The game does this for certain files, while hard-coding it for others. For simplicity we do
            // it everywhere as the ULZ header is never used unless it's compressed.
            if (FileData == null && encoder == null && (settings?.Version == KlonoaGameVersion.DTP_Prototype_19970717 ||
                                                        settings?.Version == KlonoaGameVersion.DTP))
            {
                uint header = s.DoAt(s.CurrentPointer, () => s.Serialize<uint>(default, "HeaderCheck"));
                encoder = header == ULZEncoder.Header ? new ULZEncoder() : null;
            }

            s.DoEncoded(encoder, () =>
            {
                long fileSize;

                if (encoder != null)
                    fileSize = s.CurrentLength;
                else
                    fileSize = Pre_EndPointer != null ? Pre_EndPointer - s.CurrentPointer : -1;

                s.Log("FileSize: {0}", fileSize);

                FileData = s.SerializeObject<File>(FileData, x =>
                {
                    if (x is BaseFile f)
                    {
                        f.Pre_FileSize = fileSize;
                        f.Pre_FileEncoder = encoder;
                    }

                    // Default to inheriting the file type from the parent archive
                    if (x is ArchiveFile a)
                        a.Pre_Type = Pre_ParentArchiveType;

                    Pre_OnPreSerialize?.Invoke(x);
                }, name: nameof(FileData));
            }, allowLocalPointers: true);

            // Only align if not at the end. If a size is defined for a compressed block it might not be aligned.
            if (s.CurrentPointer != Pre_EndPointer)
            {
                if (settings?.Version == KlonoaGameVersion.LV)
                    s.Align(alignBytes: 16);
                else
                    s.Align(alignBytes: 4);
            }

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