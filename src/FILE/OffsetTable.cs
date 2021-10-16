namespace BinarySerializer.Klonoa
{
    /// <summary>
    /// An offset table used for archives
    /// </summary>
    public class OffsetTable : BinarySerializable
    {
        public ArchiveFileType Pre_Type { get; set; }

        public int FilesCount { get; set; }
        public Pointer[] FilePointers { get; set; }

        // Klonoa Heroes PF
        public string KH_PF_Magic { get; set; }
        public int[] KH_PF_FileSizes { get; set; }

        // Klonoa Heroes TP
        public string KH_TP_Magic { get; set; }
        public Pointer KH_TP_FileOffsetsPointer { get; set; }
        public int KH_TP_OffsetTableLength { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Pointer offsetsAnchor = Offset;

            if (Pre_Type == ArchiveFileType.KH_PF)
            {
                KH_PF_Magic = s.SerializeString(KH_PF_Magic, 2, name: nameof(KH_PF_Magic));

                if (KH_PF_Magic != "Pf")
                    throw new BinarySerializableException(this, $"Invalid magic header '{KH_PF_Magic}'");

                FilesCount = s.Serialize<short>((short)FilesCount, name: nameof(FilesCount));
                KH_PF_FileSizes = s.SerializeArray<int>(KH_PF_FileSizes, FilesCount, name: nameof(KH_PF_FileSizes));
                offsetsAnchor = s.CurrentPointer + FilesCount * 4;
            }
            else if (Pre_Type == ArchiveFileType.KH_TP)
            {
                KH_TP_Magic = s.SerializeString(KH_TP_Magic, 4, name: nameof(KH_TP_Magic));

                if (KH_TP_Magic != "TP")
                    throw new BinarySerializableException(this, $"Invalid magic header '{KH_PF_Magic}'");

                FilesCount = s.Serialize<int>(FilesCount, name: nameof(FilesCount));
                KH_TP_FileOffsetsPointer = s.SerializePointer(KH_TP_FileOffsetsPointer, anchor: Offset, name: nameof(KH_TP_FileOffsetsPointer));
                KH_TP_OffsetTableLength = s.Serialize<int>(KH_TP_OffsetTableLength, name: nameof(KH_TP_OffsetTableLength));

                // For simplicity we don't use a GoTo or DoAt and instead check to make sure we're at the correct position
                if (KH_TP_FileOffsetsPointer != s.CurrentPointer)
                    throw new BinarySerializableException(this, $"Unexpected file offsets pointer");
            }
            else
            {
                FilesCount = s.Serialize<int>(FilesCount, name: nameof(FilesCount));
            }

            if (FilesCount == -1)
                FilesCount = 0;

            FilePointers ??= new Pointer[FilesCount];

            for (int i = 0; i < FilePointers.Length; i++)
            {
                FilePointers[i] = s.SerializePointer(FilePointers[i], anchor: offsetsAnchor, name: $"{nameof(FilePointers)}[{i}]");

                // If file offset is 0 we null it
                if ((Pre_Type == ArchiveFileType.Default || Pre_Type == ArchiveFileType.KH_TP) && FilePointers[i] == offsetsAnchor)
                    FilePointers[i] = null;
            }
        }
    }
}