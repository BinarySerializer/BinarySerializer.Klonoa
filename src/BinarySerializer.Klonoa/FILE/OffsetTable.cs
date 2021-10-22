using System.Linq;

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
        public int[] KH_PF_FileSizes { get; set; }

        // Klonoa Heroes TP
        public Pointer KH_TP_FileOffsetsPointer { get; set; }
        public int KH_TP_OffsetTableLength { get; set; }

        // Klonoa Heroes KW
        public Pointer KH_KW_FileOffsetsPointer { get; set; }
        public int KH_KW_OffsetTableLength { get; set; }
        public Pointer KH_KW_EndPointer { get; set; }
        public KH_KW_Entry[] KH_KW_Entries { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Pointer offsetsAnchor = Offset;

            if (Pre_Type == ArchiveFileType.KH_PF)
            {
                s.SerializeMagicString("Pf", 2);
                FilesCount = s.Serialize<short>((short)FilesCount, name: nameof(FilesCount));
                KH_PF_FileSizes = s.SerializeArray<int>(KH_PF_FileSizes, FilesCount, name: nameof(KH_PF_FileSizes));
                offsetsAnchor = s.CurrentPointer + FilesCount * 4;
            }
            else if (Pre_Type == ArchiveFileType.KH_TP)
            {
                s.SerializeMagicString("TP", 4);
                FilesCount = s.Serialize<int>(FilesCount, name: nameof(FilesCount));
                KH_TP_FileOffsetsPointer = s.SerializePointer(KH_TP_FileOffsetsPointer, anchor: Offset, name: nameof(KH_TP_FileOffsetsPointer));
                KH_TP_OffsetTableLength = s.Serialize<int>(KH_TP_OffsetTableLength, name: nameof(KH_TP_OffsetTableLength));

                // For simplicity we don't use a GoTo or DoAt and instead check to make sure we're at the correct position
                if (KH_TP_FileOffsetsPointer != s.CurrentPointer)
                    throw new BinarySerializableException(this, $"Unexpected file offsets pointer");
            }
            else if (Pre_Type == ArchiveFileType.KH_KW)
            {
                s.SerializeMagicString("KW", 4);
                FilesCount = s.Serialize<int>(FilesCount, name: nameof(FilesCount));
                KH_KW_FileOffsetsPointer = s.SerializePointer(KH_KW_FileOffsetsPointer, anchor: Offset, name: nameof(KH_KW_FileOffsetsPointer));
                KH_KW_OffsetTableLength = s.Serialize<int>(KH_KW_OffsetTableLength, name: nameof(KH_KW_OffsetTableLength));
                KH_KW_EndPointer = s.SerializePointer(KH_KW_EndPointer, anchor: Offset, name: nameof(KH_KW_EndPointer));
                s.SerializePadding(44, logIfNotNull: true);

                // For simplicity we don't use a GoTo or DoAt and instead check to make sure we're at the correct position
                if (KH_KW_FileOffsetsPointer != s.CurrentPointer)
                    throw new BinarySerializableException(this, $"Unexpected file offsets pointer");

                KH_KW_Entries = s.SerializeObjectArray<KH_KW_Entry>(KH_KW_Entries, FilesCount, name: nameof(KH_KW_Entries));
                FilePointers = KH_KW_Entries.Select(x => new Pointer(x.FileOffset, Offset.File, Offset)).ToArray();
                return;
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

        public class KH_KW_Entry : BinarySerializable
        {
            public byte MapID1 { get; set; }
            public byte MapID2 { get; set; }
            public byte MapID3 { get; set; }
            public byte Byte_03 { get; set; } // Always 1
            public uint FileOffset { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                MapID1 = s.Serialize<byte>(MapID1, name: nameof(MapID1));
                MapID2 = s.Serialize<byte>(MapID2, name: nameof(MapID2));
                MapID3 = s.Serialize<byte>(MapID3, name: nameof(MapID3));
                Byte_03 = s.Serialize<byte>(Byte_03, name: nameof(Byte_03));
                FileOffset = s.Serialize<uint>(FileOffset, name: nameof(FileOffset));
                s.SerializePadding(8, logIfNotNull: true);
            }
        }
    }
}