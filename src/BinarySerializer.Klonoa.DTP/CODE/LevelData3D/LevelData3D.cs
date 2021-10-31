namespace BinarySerializer.Klonoa.DTP
{
    public class LevelData3D : BinarySerializable
    {
        public int Pre_SectorsCount { get; set; }
        public ArchiveFile Pre_ObjectAssets { get; set; }

        public Pointer SectorGameObjectsPointer { get; set; }
        public Pointer MovementPathCamerasPointer { get; set; }
        public Pointer Pointer_2 { get; set; }
        public Pointer MovementPathFOVsPointer { get; set; }

        // Serialized from pointers
        public SectorGameObjects3D[] SectorGameObjects3D { get; set; } // One for each sector
        public MovementPathCameras MovementPathCameras { get; set; }
        public Data2Structs[] Data2 { get; set; } // One for each sector
        public MovementPathFOVs[] MovementPathFOVs { get; set; } // One for each sector

        public override void SerializeImpl(SerializerObject s)
        {
            SectorGameObjectsPointer = s.SerializePointer(SectorGameObjectsPointer, name: nameof(SectorGameObjectsPointer));
            MovementPathCamerasPointer = s.SerializePointer(MovementPathCamerasPointer, name: nameof(MovementPathCamerasPointer));
            Pointer_2 = s.SerializePointer(Pointer_2, name: nameof(Pointer_2));
            MovementPathFOVsPointer = s.SerializePointer(MovementPathFOVsPointer, name: nameof(MovementPathFOVsPointer));

            s.DoAt(SectorGameObjectsPointer, () =>
            {
                SectorGameObjects3D = s.SerializeObjectArrayUntil<SectorGameObjects3D>(
                    obj: SectorGameObjects3D, 
                    conditionCheckFunc: x => x.Objects[0].Short_0E == -1, 
                    onPreSerialize: x => x.Pre_ObjectAssets = Pre_ObjectAssets,
                    name: nameof(SectorGameObjects3D));

                var sectorToParse = Loader.GetLoader(s.Context).LevelSector;
                for (int i = 0; i < SectorGameObjects3D.Length; i++)
                {
                    if (sectorToParse == -1 || sectorToParse == i)
                    {
                        foreach (var m in SectorGameObjects3D[i].Objects)
                            m.SerializeDataFiles(s);
                    }
                }
            });
            s.DoAt(MovementPathCamerasPointer, () => MovementPathCameras = s.SerializeObject<MovementPathCameras>(MovementPathCameras, x => x.Pre_SectorsCount = Pre_SectorsCount, name: nameof(MovementPathCameras)));
            s.DoAt(Pointer_2, () => Data2 = s.SerializeObjectArrayUntil<Data2Structs>(Data2, x => x.Entries[0].Values[1] == -1, name: nameof(Data2)));
            s.DoAt(MovementPathFOVsPointer, () => MovementPathFOVs = s.SerializeObjectArrayUntil<MovementPathFOVs>(MovementPathFOVs, x => x.PathFOVs[0].FOVs[0].MovementPathPosition == -2, name: nameof(MovementPathFOVs)));
        }

        // TODO: Name and move to separate files
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
    }
}