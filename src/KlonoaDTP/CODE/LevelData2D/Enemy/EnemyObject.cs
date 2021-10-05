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