namespace BinarySerializer.Klonoa.LV
{
    /// <summary>
    /// A vector with 2 values.
    /// </summary>
    public class KlonoaLV_Vector2<T> : BinarySerializable where T : struct
    {
        public T X { get; set; }
        public T Y { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            X = s.Serialize<T>(X, name: nameof(X));
            Y = s.Serialize<T>(Y, name: nameof(Y));
        }
    }
}