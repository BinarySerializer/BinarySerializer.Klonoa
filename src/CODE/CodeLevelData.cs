namespace BinarySerializer.KlonoaDTP
{
    public class CodeLevelData : BinarySerializable
    {
        public int Pre_SectorsCount { get; set; }

        public Pointer Pointer_0 { get; set; }
        public Pointer Pointer_1 { get; set; }
        public Pointer Pointer_2 { get; set; }
        public Pointer Pointer_3 { get; set; }

        // Serialized from pointers
        public Objects3D[] Data0 { get; set; } // One for each sector
        public Data1Structs Data1 { get; set; }
        public Data2Structs[] Data2 { get; set; } // One for each sector
        public Data3Structs[] Data3 { get; set; } // One for each sector

        public override void SerializeImpl(SerializerObject s)
        {
            Pointer_0 = s.SerializePointer(Pointer_0, name: nameof(Pointer_0));
            Pointer_1 = s.SerializePointer(Pointer_1, name: nameof(Pointer_1));
            Pointer_2 = s.SerializePointer(Pointer_2, name: nameof(Pointer_2));
            Pointer_3 = s.SerializePointer(Pointer_3, name: nameof(Pointer_3));

            s.DoAt(Pointer_0, () => Data0 = s.SerializeObjectArrayUntil<Objects3D>(Data0, x => x.Entries[0].Short_0E == -1, name: nameof(Data0)));
            s.DoAt(Pointer_1, () => Data1 = s.SerializeObject<Data1Structs>(Data1, x => x.Pre_SectorsCount = Pre_SectorsCount, name: nameof(Data1)));
            s.DoAt(Pointer_2, () => Data2 = s.SerializeObjectArrayUntil<Data2Structs>(Data2, x => x.Entries[0].Values[1] == -1, name: nameof(Data2)));
            s.DoAt(Pointer_3, () => Data3 = s.SerializeObjectArrayUntil<Data3Structs>(Data3, x => x.Entries[0].Structs[0].Int_00 == -2, name: nameof(Data3)));
        }

        // TODO: Name and move to separate files
        public class Objects3D : BinarySerializable
        {
            public Object3D[] Entries { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Entries = s.SerializeObjectArrayUntil<Object3D>(Entries, x => x.Short_00 == -1, name: nameof(Entries));
            }
        }
        public class Object3D : BinarySerializable
        {
            public short Short_00 { get; set; }
            public short Short_02 { get; set; }
            public int Int_04 { get; set; }
            public short Short_08 { get; set; } // If 40 then do something special
            public short Type { get; set; }
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
                Int_04 = s.Serialize<int>(Int_04, name: nameof(Int_04));
                Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
                Type = s.Serialize<short>(Type, name: nameof(Type));
                Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
                Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));
                Uint_10 = s.Serialize<uint>(Uint_10, name: nameof(Uint_10));
                Pointer_14 = s.SerializePointer(Pointer_14, name: nameof(Pointer_14));
                Short_18 = s.Serialize<short>(Short_18, name: nameof(Short_18));
                Short_1A = s.Serialize<short>(Short_1A, name: nameof(Short_1A));

                s.DoAt(Pointer_14, () => TMDIndices = s.SerializeArray<ushort>(TMDIndices, 8, name: nameof(TMDIndices)));
            }
        }

        public class Data1Structs : BinarySerializable
        {
            public int Pre_SectorsCount { get; set; }

            public Data1Struct[] FixedEntries { get; set; }
            public Data1StructGroup[][] SectorEntries { get; set; } // One for each sector

            public override void SerializeImpl(SerializerObject s)
            {
                FixedEntries = s.SerializeObjectArrayUntil<Data1Struct>(FixedEntries, x => x.Short_04 == -1, name: nameof(FixedEntries));

                SectorEntries ??= new Data1StructGroup[Pre_SectorsCount][];

                for (int i = 0; i < SectorEntries.Length; i++)
                    SectorEntries[i] = s.SerializeObjectArrayUntil(SectorEntries[i], x => x.Structs[0].Short_04 == -1, name: $"{nameof(SectorEntries)}[{i}]");
            }
        }
        public class Data1StructGroup : BinarySerializable
        {
            public Data1Struct[] Structs { get; set; } // One per movement path in the sector

            public override void SerializeImpl(SerializerObject s)
            {
                Structs = s.SerializeObjectArrayUntil<Data1Struct>(Structs, x => x.Int_00 == -1, name: nameof(Structs));
            }
        }
        public class Data1Struct : BinarySerializable
        {
            public int Int_00 { get; set; }
            public short Short_04 { get; set; }
            public short Short_06 { get; set; }
            public short Short_08 { get; set; }
            public short Short_0A { get; set; }
            public short Short_0C { get; set; }
            public short Short_0E { get; set; }
            public short Short_10 { get; set; }
            public short Short_12 { get; set; } // Is 1 when the pointer is valid, otherwise 0 - a bool?
            public Pointer Pointer_14 { get; set; } // 7 shorts

            // Serialized from pointers
            public UnknownStruct[] Structs { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Int_00 = s.Serialize<int>(Int_00, name: nameof(Int_00));
                Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
                Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
                Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
                Short_0A = s.Serialize<short>(Short_0A, name: nameof(Short_0A));
                Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
                Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));
                Short_10 = s.Serialize<short>(Short_10, name: nameof(Short_10));
                Short_12 = s.Serialize<short>(Short_12, name: nameof(Short_12));
                Pointer_14 = s.SerializePointer(Pointer_14, name: nameof(Pointer_14));

                s.DoAt(Pointer_14, () => Structs = s.SerializeObjectArray<UnknownStruct>(Structs, 2, name: nameof(Structs)));
            }

            // Vector?
            public class UnknownStruct : BinarySerializable
            {
                public short Short_00 { get; set; }
                public short Short_02 { get; set; }
                public short Short_04 { get; set; }

                public override void SerializeImpl(SerializerObject s)
                {
                    Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
                    Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
                    Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
                    s.SerializePadding(2, logIfNotNull: true);
                }
            }
        }

        public class Data2Structs : BinarySerializable
        {
            public Data2Struct[] Entries { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Entries = s.SerializeObjectArrayUntil<Data2Struct>(Entries, x => x.Values[0] == -1, name: nameof(Entries));
            }
        }
        public class Data2Struct : BinarySerializable
        {
            public short[] Values { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Values = s.SerializeArray<short>(Values, 9, name: nameof(Values));
                s.SerializePadding(2, logIfNotNull: true);
            }
        }
        public class Data3Structs : BinarySerializable
        {
            public Data3StructGroup[] Entries { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Entries = s.SerializeObjectArrayUntil<Data3StructGroup>(Entries, x => x.Structs[0].Short_06 == -1, name: nameof(Entries));
            }
        }
        public class Data3StructGroup : BinarySerializable
        {
            public Data3Struct[] Structs { get; set; } // One per movement path in the sector

            public override void SerializeImpl(SerializerObject s)
            {
                Structs = s.SerializeObjectArrayUntil<Data3Struct>(Structs, x => x.Int_00 == -1 || x.Int_00 == -2, name: nameof(Structs));
            }
        }
        public class Data3Struct : BinarySerializable
        {
            public int Int_00 { get; set; }
            public short Short_04 { get; set; }
            public short Short_06 { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Int_00 = s.Serialize<int>(Int_00, name: nameof(Int_00));
                Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
                Short_06 = s.Serialize<short>(Short_06, name: nameof(Short_06));
            }
        }
    }
}