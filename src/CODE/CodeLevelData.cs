namespace BinarySerializer.KlonoaDTP
{
    public class CodeLevelData : BinarySerializable
    {
        public Pointer Pointer_0 { get; set; }
        public Pointer Pointer_1 { get; set; }
        public Pointer Pointer_2 { get; set; }
        public Pointer Pointer_3 { get; set; }

        // Serialized from pointers
        public Data0Structs[] Data0 { get; set; } // One for each sector

        public override void SerializeImpl(SerializerObject s)
        {
            Pointer_0 = s.SerializePointer(Pointer_0, name: nameof(Pointer_0));
            Pointer_1 = s.SerializePointer(Pointer_1, name: nameof(Pointer_1));
            Pointer_2 = s.SerializePointer(Pointer_2, name: nameof(Pointer_2));
            Pointer_3 = s.SerializePointer(Pointer_3, name: nameof(Pointer_3));

            s.DoAt(Pointer_0, () => Data0 = s.SerializeObjectArrayUntil(Data0, x => x.Entries[0].Short_0E == -1, name: nameof(Data0)));
        }

        public class Data0Structs : BinarySerializable
        {
            public Data0Struct[] Entries { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Entries = s.SerializeObjectArrayUntil(Entries, x => x.Short_00 == -1, name: nameof(Entries));
            }
        }
        public class Data0Struct : BinarySerializable
        {
            public short Short_00 { get; set; }
            public short Short_02 { get; set; }
            public byte[] Bytes_04 { get; set; }
            public short Short_08 { get; set; }
            public short Short_0A { get; set; }
            public short Short_0C { get; set; }
            public short Short_0E { get; set; }
            public uint Uint_10 { get; set; }
            public Pointer Pointer_14 { get; set; }
            public short Short_18 { get; set; }
            public short Short_1A { get; set; } // Seems to be used in memory to indicate if it's been loaded

            // Serialized from pointers
            public ushort[] TMDIndices { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
                Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
                Bytes_04 = s.SerializeArray<byte>(Bytes_04, 4, name: nameof(Bytes_04));
                Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
                Short_0A = s.Serialize<short>(Short_0A, name: nameof(Short_0A));
                Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
                Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));
                Uint_10 = s.Serialize<uint>(Uint_10, name: nameof(Uint_10));
                Pointer_14 = s.SerializePointer(Pointer_14, name: nameof(Pointer_14));
                Short_18 = s.Serialize<short>(Short_18, name: nameof(Short_18));
                Short_1A = s.Serialize<short>(Short_1A, name: nameof(Short_1A));

                s.DoAt(Pointer_14, () => TMDIndices = s.SerializeArray<ushort>(TMDIndices, 8, name: nameof(TMDIndices)));
            }
        }
    }
}