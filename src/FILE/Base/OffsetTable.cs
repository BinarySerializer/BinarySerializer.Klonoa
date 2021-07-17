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

            FilePointers ??= new Pointer[FilesCount];

            for (int i = 0; i < FilePointers.Length; i++)
            {
                FilePointers[i] = s.SerializePointer(FilePointers[i], anchor: Offset, name: $"{nameof(FilePointers)}[{i}]");

                // If file offset is 0 we null it
                if (FilePointers[i] == Offset)
                    FilePointers[i] = null;
            }
        }
    }
}