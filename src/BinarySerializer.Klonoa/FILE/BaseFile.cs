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

    /// <summary>
    /// A <see cref="BaseFile"/> with a generic object. This is useful for serializing file objects which do not inherit from <see cref="BaseFile"/>
    /// but still require the base properties of the class (such as storing the encoder used for the file).
    /// </summary>
    /// <typeparam name="FileObject">The object type</typeparam>
    public class BaseFile<FileObject> : BaseFile
        where FileObject : BinarySerializable, new()
    {
        public BaseFile() { }
        public BaseFile(FileObject obj) => Obj = obj;

        public FileObject Obj { get; set; }

        public static implicit operator FileObject(BaseFile<FileObject> file) => file.Obj;
        public static explicit operator BaseFile<FileObject>(FileObject obj) => new BaseFile<FileObject>(obj);

        public override void SerializeImpl(SerializerObject s)
        {
            Obj = s.SerializeObject<FileObject>(Obj, name: nameof(Obj));
        }
    }
}