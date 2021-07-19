using BinarySerializer.PS1;

namespace BinarySerializer.KlonoaDTP
{
    public class Object3D : BinarySerializable
    {
        public ArchiveFile Pre_ObjectModelsDataPack { get; set; }

        public short Short_00 { get; set; }
        public short Short_02 { get; set; }
        public int Int_04 { get; set; }
        public short Short_08 { get; set; } // If 40 then save the object in another array in memory - why? Seems to be for objects which move.
        public Object3DType Type { get; set; }
        public short Short_0C { get; set; }
        public short Short_0E { get; set; }
        public uint Uint_10 { get; set; }
        public Pointer Pointer_14 { get; set; }
        public short Short_18 { get; set; }
        public short Short_1A { get; set; } // Seems to be used in memory to indicate if it's been loaded

        // Serialized from pointers
        public ushort[] DataFileIndices { get; set; }

        // Serialized from data files
        public PS1_TMD Data_TMD { get; set; }
        public Object3DPosition_File Data_Position { get; set; } // TODO: Objects which move have multiple positions
        public Object3DTransform_ArchiveFile Data_Transform { get; set; }
        public PS1_TIM Data_TIM { get; set; }
        public TIM_ArchiveFile Data_TextureAnimFrames { get; set; }
        
        public Object3DType3Data_File Data_Type3 { get; set; }
        public Object3DType7Data_File Data_Type7 { get; set; }
        public RawData_File Data_Type21 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
            Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
            Int_04 = s.Serialize<int>(Int_04, name: nameof(Int_04));
            Short_08 = s.Serialize<short>(Short_08, name: nameof(Short_08));
            Type = s.Serialize<Object3DType>(Type, name: nameof(Type));
            Short_0C = s.Serialize<short>(Short_0C, name: nameof(Short_0C));
            Short_0E = s.Serialize<short>(Short_0E, name: nameof(Short_0E));
            Uint_10 = s.Serialize<uint>(Uint_10, name: nameof(Uint_10));
            Pointer_14 = s.SerializePointer(Pointer_14, name: nameof(Pointer_14));
            Short_18 = s.Serialize<short>(Short_18, name: nameof(Short_18));
            Short_1A = s.Serialize<short>(Short_1A, name: nameof(Short_1A));

            s.DoAt(Pointer_14, () => DataFileIndices = s.SerializeArray<ushort>(DataFileIndices, 8, name: nameof(DataFileIndices)));

            if (Type ==  Object3DType.Invalid || Type == Object3DType.None)
                return;

            if (Pointer_14 == null)
            {
                s.LogWarning($"Object3D of type {Type} has no data");
                return;
            }

            var dataPack = Pre_ObjectModelsDataPack;

            // In Vision 1-1 NTSC the function pointer table is at 0x80110808, with a length of 24
            switch (Type)
            {
                case Object3DType.Type_1:
                    Data_TMD = dataPack.SerializeFile<PS1_TMD>(s, Data_TMD, DataFileIndices[0], logIfNotFullyParsed: false, name: nameof(Data_TMD));
                    // TODO: File 1 seems to usually be another TMD, but not when Short_08 is 40 - why? What is it then?
                    Data_Position = dataPack.SerializeFile<Object3DPosition_File>(s, Data_Position, DataFileIndices[2], name: nameof(Data_Position));
                    
                    if (DataFileIndices[3] != 0)
                        s.LogWarning($"Object3D of type {Type} has additional referenced files");

                    break;

                case Object3DType.Type_3:
                    Data_Type3 = dataPack.SerializeFile<Object3DType3Data_File>(s, Data_Type3, DataFileIndices[0], name: nameof(Data_Type3));

                    if (DataFileIndices[1] != 0)
                        s.LogWarning($"Object3D of type {Type} has additional referenced files");

                    break;

                case Object3DType.Type_5:
                case Object3DType.Type_8:
                    Data_TMD = dataPack.SerializeFile<PS1_TMD>(s, Data_TMD, DataFileIndices[0], logIfNotFullyParsed: false, name: nameof(Data_TMD));
                    Data_Transform = dataPack.SerializeFile<Object3DTransform_ArchiveFile>(s, Data_Transform, DataFileIndices[1], name: nameof(Data_Transform));

                    if (DataFileIndices[2] != 0)
                        s.LogWarning($"Object3D of type {Type} has additional referenced files");

                    break;

                case Object3DType.Type_6:
                    Data_TMD = dataPack.SerializeFile<PS1_TMD>(s, Data_TMD, DataFileIndices[0], logIfNotFullyParsed: false, name: nameof(Data_TMD));
                    Data_Transform = dataPack.SerializeFile<Object3DTransform_ArchiveFile>(s, Data_Transform, DataFileIndices[1], name: nameof(Data_Transform));
                    Data_TIM = dataPack.SerializeFile<PS1_TIM>(s, Data_TIM, DataFileIndices[2], name: nameof(Data_TIM));

                    if (DataFileIndices[3] != 0)
                        s.LogWarning($"Object3D of type {Type} has additional referenced files");

                    break;

                case Object3DType.Type_7:
                case Object3DType.Type_9:
                    Data_Type7 = dataPack.SerializeFile<Object3DType7Data_File>(s, Data_Type7, DataFileIndices[0], name: nameof(Data_Type7));

                    if (DataFileIndices[1] != 0)
                        s.LogWarning($"Object3D of type {Type} has additional referenced files");

                    break;

                case Object3DType.TextureAnimation:
                    Data_TextureAnimFrames = dataPack.SerializeFile<TIM_ArchiveFile>(s, Data_TextureAnimFrames, DataFileIndices[0], name: nameof(Data_TextureAnimFrames));

                    if (DataFileIndices[1] != 0)
                        s.LogWarning($"Object3D of type {Type} has additional referenced files");

                    break;

                case Object3DType.Type_21:
                    Data_TMD = dataPack.SerializeFile<PS1_TMD>(s, Data_TMD, DataFileIndices[0], logIfNotFullyParsed: false, name: nameof(Data_TMD));
                    Data_Type21 = dataPack.SerializeFile<RawData_File>(s, Data_Type21, DataFileIndices[1], name: nameof(Data_Type21));

                    if (DataFileIndices[2] != 0)
                        s.LogWarning($"Object3D of type {Type} has additional referenced files");

                    break;

                default:
                    s.LogWarning($"Unknown Object3D type {Type}");
                    break;
            }
        }

        public enum Object3DType : short
        {
            Invalid = -1,
            None = 0,
            Type_1 = 1,

            Type_3 = 3,

            Type_5 = 5,
            Type_6 = 6,
            Type_7 = 7,
            Type_8 = 8,
            Type_9 = 9,
            TextureAnimation = 10, // Swaps out data in VRAM to cause textures to animate

            Type_21 = 21,
        }

        // TODO: Name and move to files
        public class Object3DType3Data_File : BaseFile
        {
            public int[] Offsets { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Offsets = s.SerializeArrayUntil(Offsets, x => x == -1, () => -1, name: nameof(Offsets));
            }
        }
        public class Object3DType7Data_File : BaseFile
        {
            public short EntriesCount { get; set; }
            public short Short_02 { get; set; }
            public Entry[] Entries { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                EntriesCount = s.Serialize<short>(EntriesCount, name: nameof(EntriesCount));
                Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
                Entries = s.SerializeObjectArray<Entry>(Entries, EntriesCount, name: nameof(Entries));
            }

            public class Entry : BinarySerializable
            {
                public short Short_00 { get; set; }
                public short Short_02 { get; set; }
                public short Short_04 { get; set; }

                public override void SerializeImpl(SerializerObject s)
                {
                    Short_00 = s.Serialize<short>(Short_00, name: nameof(Short_00));
                    Short_02 = s.Serialize<short>(Short_02, name: nameof(Short_02));
                    Short_04 = s.Serialize<short>(Short_04, name: nameof(Short_04));
                }
            }
        }
    }
}