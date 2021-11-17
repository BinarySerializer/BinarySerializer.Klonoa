namespace BinarySerializer.Klonoa.DTP
{
    public class EnemyObject : BinarySerializable
    {
        public LevelData2D Pre_LevelData2D { get; set; }
        public bool Pre_IsSpawnedObject { get; set; }

        public int MovementPathSpawnPosition { get; set; } // The position on the movement path which spawns the object. This is relative to the movement path the object is read from rather than the path it's set to using the MovementPath property.
        public int MovementPathPosition { get; set; } // Only set if MovementPath is not -1. This gets the same position as the position values.

        public PrimaryObjectType PrimaryType => PrimaryObjectType.Enemy_2D;
        public short SecondaryType { get; set; }
        public ushort DataIndex { get; set; }

        public KlonoaVector20 Position { get; set; }

        public short GraphicsIndex { get; set; } // This is an index to an array of functions which handles the graphics (NOTE: not always the case, depends on the type)
        public short MovementPath { get; set; } // -1 if not directly on a path
        public short GlobalSectorIndex { get; set; }
        public ushort Ushort_1E { get; set; }
        public ushort Ushort_20 { get; set; }
        public ushort Flags { get; set; } // Has flip flags?
        public short Short_24 { get; set; } // Usually -1

        public short Spawn_Short_06 { get; set; }
        public short Spawn_Short_08 { get; set; }
        public short Spawn_Short_0A { get; set; }

        // Additional data
        public BaseEnemyData Data { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            if (!Pre_IsSpawnedObject)
            {
                MovementPathSpawnPosition = s.Serialize<int>(MovementPathSpawnPosition, name: nameof(MovementPathSpawnPosition));
                MovementPathPosition = s.Serialize<int>(MovementPathPosition, name: nameof(MovementPathPosition));
                SecondaryType = s.Serialize<short>(SecondaryType, name: nameof(SecondaryType));
                DataIndex = s.Serialize<ushort>(DataIndex, name: nameof(DataIndex));
                Position = s.SerializeObject<KlonoaVector20>(Position, name: nameof(Position));
                GraphicsIndex = s.Serialize<short>(GraphicsIndex, name: nameof(GraphicsIndex));
                MovementPath = s.Serialize<short>(MovementPath, name: nameof(MovementPath));
                GlobalSectorIndex = s.Serialize<short>(GlobalSectorIndex, name: nameof(GlobalSectorIndex));
                Ushort_1E = s.Serialize<ushort>(Ushort_1E, name: nameof(Ushort_1E));
                Ushort_20 = s.Serialize<ushort>(Ushort_20, name: nameof(Ushort_20));
                Flags = s.Serialize<ushort>(Flags, name: nameof(Flags));
                Short_24 = s.Serialize<short>(Short_24, name: nameof(Short_24));
                s.SerializePadding(2, logIfNotNull: true);
            }
            else
            {
                SecondaryType = s.Serialize<short>(SecondaryType, name: nameof(SecondaryType));
                DataIndex = s.Serialize<ushort>(DataIndex, name: nameof(DataIndex));
                GraphicsIndex = s.Serialize<short>(GraphicsIndex, name: nameof(GraphicsIndex));
                Spawn_Short_06 = s.Serialize<short>(Spawn_Short_06, name: nameof(Spawn_Short_06));
                Spawn_Short_08 = s.Serialize<short>(Spawn_Short_08, name: nameof(Spawn_Short_08));
                Spawn_Short_0A = s.Serialize<short>(Spawn_Short_0A, name: nameof(Spawn_Short_0A));
            }

            switch (SecondaryType)
            {
                case 1: SerializeData<EnemyData_01>(s, Pre_LevelData2D.Enemy_Data_01_Pointer, 0x24); break;
                case 2: SerializeData<EnemyData_02>(s, Pre_LevelData2D.Enemy_Data_02_Pointer, 0x24); break;
                case 3: SerializeData<EnemyData_03>(s, Pre_LevelData2D.Enemy_Data_03_Pointer, 0x28); break;
                case 4: SerializeData<EnemyData_04>(s, Pre_LevelData2D.Enemy_Data_04_Pointer, 0x20); break;
                case 5: SerializeData<EnemyData_05>(s, Pre_LevelData2D.Enemy_Data_05_Pointer, 0x20); break;
                case 6: SerializeData<EnemyData_06>(s, Pre_LevelData2D.Enemy_Data_06_Pointer, 0x20); break;
                case 7: SerializeData<EnemyData_07>(s, Pre_LevelData2D.Enemy_Data_07_Pointer, 0x24); break;
                case 8: SerializeData<EnemyData_08>(s, Pre_LevelData2D.Enemy_Data_08_Pointer, 0x28); break;
                case 9: SerializeData<EnemyData_09>(s, Pre_LevelData2D.Enemy_Data_09_Pointer, 0x18); break;
                //case 10: SerializeData<EnemyData_10>(s, Pre_LevelData2D.Enemy_Data_10_Pointer, ); break;
                case 11: SerializeData<EnemyData_11>(s, Pre_LevelData2D.Enemy_Data_11_Pointer, 0x24); break;
                case 12: SerializeData<EnemyData_12>(s, Pre_LevelData2D.Enemy_Data_12_Pointer, 0x3C); break;
                case 13: SerializeData<EnemyData_13>(s, Pre_LevelData2D.Enemy_Data_13_Pointer, 0x24); break;
                case 14: SerializeData<EnemyData_14>(s, Pre_LevelData2D.Enemy_Data_14_Pointer, 0x2C); break;
                case 15: SerializeData<EnemyData_15>(s, Pre_LevelData2D.Enemy_Data_15_Pointer, 0x24); break;
                case 16: SerializeData<EnemyData_16>(s, Pre_LevelData2D.Enemy_Data_16_Pointer, 0x24); break;
                case 17: SerializeData<EnemyData_17>(s, Pre_LevelData2D.Enemy_Data_17_Pointer, 0x24); break;
                case 18: SerializeData<EnemyData_18>(s, Pre_LevelData2D.Enemy_Data_18_Pointer, 0x28); break;
                //case 19: SerializeData<EnemyData_19>(s, Pre_LevelData2D.Enemy_Data_19_Pointer, ); break;
                case 20: SerializeData<EnemyData_20>(s, Pre_LevelData2D.Enemy_Data_20_Pointer, 0x18); break;
                //case 21: SerializeData<EnemyData_21>(s, Pre_LevelData2D.Enemy_Data_21_Pointer, ); break;
                case 22: SerializeData<EnemyData_22>(s, Pre_LevelData2D.Enemy_Data_22_Pointer, 0x2C); break;
                //case 23: SerializeData<EnemyData_23>(s, Pre_LevelData2D.Enemy_Data_23_Pointer, ); break;
                case 24: SerializeData<EnemyData_24>(s, Pre_LevelData2D.Enemy_Data_24_Pointer, 0x2C); break;
                case 25: SerializeData<EnemyData_25>(s, Pre_LevelData2D.Enemy_Data_25_Pointer, 0x28); break;
                case 26: SerializeData<EnemyData_26>(s, Pre_LevelData2D.Enemy_Data_26_Pointer, 0x1C); break;
                case 27: SerializeData<EnemyData_27>(s, Pre_LevelData2D.Enemy_Data_27_Pointer, 0x2C); break;
                case 28: SerializeData<EnemyData_28>(s, Pre_LevelData2D.Enemy_Data_28_Pointer, 0x24); break;
                case 29: SerializeData<EnemyData_29>(s, Pre_LevelData2D.Enemy_Data_29_Pointer, 0x28); break;
                case 30: SerializeData<EnemyData_30>(s, Pre_LevelData2D.Enemy_Data_30_Pointer, 0x24); break;
                case 31: SerializeData<EnemyData_31>(s, Pre_LevelData2D.Enemy_Data_31_Pointer, 0x28); break;
                case 32: SerializeData<EnemyData_32>(s, Pre_LevelData2D.Enemy_Data_32_Pointer, 0x24); break;
                case 33: SerializeData<EnemyData_33>(s, Pre_LevelData2D.Enemy_Data_33_Pointer, 0x28); break;
                case 34: SerializeData<EnemyData_34>(s, Pre_LevelData2D.Enemy_Data_34_Pointer, 0x24); break;
                case 35: SerializeData<EnemyData_35>(s, Pre_LevelData2D.Enemy_Data_35_Pointer, 0x2C); break;
                case 36: SerializeData<EnemyData_36>(s, Pre_LevelData2D.Enemy_Data_36_Pointer, 0x28); break;
                case 37: SerializeData<EnemyData_37>(s, Pre_LevelData2D.Enemy_Data_37_Pointer, 0x28); break;
                case 38: SerializeData<EnemyData_38>(s, Pre_LevelData2D.Enemy_Data_38_Pointer, 0x28); break;
                case 39: SerializeData<EnemyData_39>(s, Pre_LevelData2D.Enemy_Data_39_Pointer, 0x24); break;
                case 40: SerializeData<EnemyData_40>(s, Pre_LevelData2D.Enemy_Data_40_Pointer, 0x14); break;
                //case 41: SerializeData<EnemyData_41>(s, Pre_LevelData2D.Enemy_Data_41_Pointer, ); break;
                //default: s.LogWarning($"Enemy data not parsed for enemy of type {SecondaryType}"); break;
            }
        }

        private void SerializeData<T>(SerializerObject s, Pointer pointer, int dataLength)
            where T : BaseEnemyData, new()
        {
            s.DoAt(pointer + DataIndex * dataLength, () => Data = s.SerializeObject<T>((T)Data, x =>
            {
                x.Pre_LevelData2D = Pre_LevelData2D;
                x.Pre_EnemyObj = this;
            }, name: nameof(Data)));
        }
    }
}