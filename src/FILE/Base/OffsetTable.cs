using System.Linq;

namespace BinarySerializer.KlonoaDTP
{
    /// <summary>
    /// An offset table used for archives
    /// </summary>
    public class OffsetTable : BinarySerializable
    {
        public int FilesCount { get; set; }
        public Pointer[] FilePointers { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            FilesCount = s.Serialize<int>(FilesCount, name: nameof(FilesCount));

            if (FilesCount == -1)
                FilesCount = 0;

            FilePointers = s.SerializePointerArray(FilePointers, FilesCount, anchor: Offset, name: nameof(FilePointers));

            if (FilePointers.Any(x => x == Offset))
                s.LogWarning($"Offset table contains nulled out offset");
        }
    }
}