namespace BinarySerializer.Klonoa
{
    /// <summary>
    /// A base class for BIN files
    /// </summary>
    public abstract class BaseFile : BinarySerializable
    {
        /// <summary>
        /// Indicates if the warning logs for the file not being fully serialized should be disabled for this file
        /// </summary>
        public virtual bool DisableNotFullySerializedWarning => false;

        /// <summary>
        /// The file size, should be set before serializing
        /// </summary>
        public long Pre_FileSize { get; set; } = -1;

        /// <summary>
        /// Indicates if the file is encoded
        /// </summary>
        public bool Pre_IsCompressed => Pre_FileEncoder != null;
        
        /// <summary>
        /// The file encoder the file is encoded with. This gets set when deserializing the file and is used to correctly serialize the file.
        /// If the file is not encoded it is null.
        /// </summary>
        public IStreamEncoder Pre_FileEncoder { get; set; }
    }
}