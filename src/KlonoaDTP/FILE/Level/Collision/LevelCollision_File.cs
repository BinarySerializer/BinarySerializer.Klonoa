namespace BinarySerializer.Klonoa.DTP
{
    public class LevelCollision_File : BaseFile
    {
        public ushort Width { get; set; }
        public ushort Height { get; set; }
        public ushort Depth { get; set; }

        public KlonoaVector16 Pivot { get; set; }

        public ushort CollisionGridOffset { get; set; }
        public ushort CollisionGridItemStructuresOffset { get; set; }
        public ushort CollisionIndicesOffset { get; set; }

        public LevelCollisionGridItem[][] CollisionGrid { get; set; } // Each cell is 256x256x256

        public override void SerializeImpl(SerializerObject s)
        {
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));
            Depth = s.Serialize<ushort>(Depth, name: nameof(Depth));
            s.SerializePadding(2, logIfNotNull: true);
            Pivot = s.SerializeObject<KlonoaVector16>(Pivot, name: nameof(Pivot));
            s.SerializePadding(2, logIfNotNull: true);
            CollisionGridOffset = s.Serialize<ushort>(CollisionGridOffset, name: nameof(CollisionGridOffset));
            CollisionGridItemStructuresOffset = s.Serialize<ushort>(CollisionGridItemStructuresOffset, name: nameof(CollisionGridItemStructuresOffset));
            CollisionIndicesOffset = s.Serialize<ushort>(CollisionIndicesOffset, name: nameof(CollisionIndicesOffset));

            s.DoAt(Offset + CollisionGridOffset * 2, () =>
            {
                var collisionIndicesPointer = Offset + CollisionIndicesOffset * 2;
                var collisionGridItemStructuresPointer = Offset + CollisionGridItemStructuresOffset * 2;

                CollisionGrid ??= new LevelCollisionGridItem[Width][];

                for (int i = 0; i < CollisionGrid.Length; i++)
                {
                    CollisionGrid[i] = s.SerializeObjectArray<LevelCollisionGridItem>(CollisionGrid[i], Height, onPreSerialize: x =>
                    {
                        x.Pre_CollisionIndicesPointer = collisionIndicesPointer;
                        x.Pre_CollisionGridItemStructuresPointer = collisionGridItemStructuresPointer;
                        x.Pre_Depth = Depth;
                    }, name: $"{nameof(CollisionGrid)}[{i}]");
                }
            });

            // Got to the end of the file
            s.Goto(Offset + Pre_FileSize);
        }
    }
}