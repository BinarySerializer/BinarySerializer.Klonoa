namespace BinarySerializer.Klonoa.DTP
{
    public class LevelCollisionSectorMap_File : BaseFile
    {
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public ushort Depth { get; set; }

        public KlonoaVector16 Pivot { get; set; }

        public ushort CollisionGridOffset { get; set; }
        public ushort LevelCollisionSectorItemStructuresOffset { get; set; }
        public ushort CollisionIndicesOffset { get; set; }

        public LevelCollisionSectorItem[][] CollisionGrid { get; set; } // Each sector is 256x256x256

        public override void SerializeImpl(SerializerObject s)
        {
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));
            Depth = s.Serialize<ushort>(Depth, name: nameof(Depth));
            s.SerializePadding(2, logIfNotNull: true);
            Pivot = s.SerializeObject<KlonoaVector16>(Pivot, name: nameof(Pivot));
            s.SerializePadding(2, logIfNotNull: true);
            CollisionGridOffset = s.Serialize<ushort>(CollisionGridOffset, name: nameof(CollisionGridOffset));
            LevelCollisionSectorItemStructuresOffset = s.Serialize<ushort>(LevelCollisionSectorItemStructuresOffset, name: nameof(LevelCollisionSectorItemStructuresOffset));
            CollisionIndicesOffset = s.Serialize<ushort>(CollisionIndicesOffset, name: nameof(CollisionIndicesOffset));

            s.DoAt(Offset + CollisionGridOffset * 2, () =>
            {
                var collisionIndicesPointer = Offset + CollisionIndicesOffset * 2;
                var levelCollisionSectorItemStructuresPointer = Offset + LevelCollisionSectorItemStructuresOffset * 2;

                CollisionGrid ??= new LevelCollisionSectorItem[Width][];

                for (int i = 0; i < CollisionGrid.Length; i++)
                {
                    CollisionGrid[i] = s.SerializeObjectArray<LevelCollisionSectorItem>(CollisionGrid[i], Height, onPreSerialize: x =>
                    {
                        x.Pre_CollisionIndicesPointer = collisionIndicesPointer;
                        x.Pre_LevelCollisionSectorItemStructuresPointer = levelCollisionSectorItemStructuresPointer;
                        x.Pre_Depth = Depth;
                    }, name: $"{nameof(CollisionGrid)}[{i}]");
                }
            });

            // Got to the end of the file
            s.Goto(Offset + Pre_FileSize);
        }
    }
}