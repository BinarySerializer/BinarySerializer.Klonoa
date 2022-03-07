namespace BinarySerializer.Klonoa.DTP
{
    public class ClipTable_ArchiveFile : ArchiveFile<ClipTable_ArchiveFile.Block>
    {
        public class Block : BinarySerializable
        {
            public Struct[] Values { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Values = s.SerializeObjectArray<Struct>(Values, 200, name: nameof(Values));
            }

            // Related to LevelModelObjectMap
            public class Struct : BinarySerializable
            {
                public sbyte X { get; set; }
                public sbyte Y { get; set; }
                public sbyte Z { get; set; }

                public override void SerializeImpl(SerializerObject s)
                {
                    X = s.Serialize<sbyte>(X, name: nameof(X));
                    Y = s.Serialize<sbyte>(Y, name: nameof(Y));
                    Z = s.Serialize<sbyte>(Z, name: nameof(Z));
                    s.SerializePadding(1, logIfNotNull: true);
                }
            }
        }
    }
}