namespace BinarySerializer.Klonoa
{
    /// <summary>
    /// A base class for BIN files
    /// </summary>
    public abstract class BaseFile : BinarySerializable
    {
        public virtual bool DisableNotFullySerializedWarning => false;

        /// <summary>
        /// The file size, should be set before serializing
        /// </summary>
        public long Pre_FileSize { get; set; } = -1;

        /// <summary>
        /// Indicates if the file is compressed using ULZ, should be set before serializing
        /// </summary>
        public bool Pre_IsCompressed { get; set; }
    }
}