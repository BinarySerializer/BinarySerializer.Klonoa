namespace BinarySerializer.Klonoa.KH
{
    public class Map_File : BaseFile
    {
        public Pointer Pre_SharedDataPointer { get; set; }

        public uint EnemyObjectsOffset { get; set; }
        public uint TriggerObjectsOffset { get; set; }
        public uint SpecialMapLayerOffset { get; set; }
        public uint[] MapLayerOffsets { get; set; }

        public EnemyObjects EnemyObjects { get; set; }
        public TriggerObjects TriggerObjects { get; set; }

        public MapLayer SpecialMapLayer { get; set; } // Has palette, tileset and collision
        public MapLayer[] MapLayers { get; set; } // Has the map layers

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("KM", 4);
            s.SerializePadding(12, logIfNotNull: true);
            EnemyObjectsOffset = s.Serialize<uint>(EnemyObjectsOffset, name: nameof(EnemyObjectsOffset));
            TriggerObjectsOffset = s.Serialize<uint>(TriggerObjectsOffset, name: nameof(TriggerObjectsOffset));
            SpecialMapLayerOffset = s.Serialize<uint>(SpecialMapLayerOffset, name: nameof(SpecialMapLayerOffset));
            s.SerializePadding(4, logIfNotNull: true);
            MapLayerOffsets = s.SerializeArray<uint>(MapLayerOffsets, 4, name: nameof(MapLayerOffsets));
            s.SerializePadding(16, logIfNotNull: true);

            if (EnemyObjectsOffset != 0)
                s.DoAt(Offset + EnemyObjectsOffset, () => EnemyObjects = s.SerializeObject<EnemyObjects>(EnemyObjects, name: nameof(EnemyObjects)));

            if (TriggerObjectsOffset != 0)
                s.DoAt(Offset + TriggerObjectsOffset, () => TriggerObjects = s.SerializeObject<TriggerObjects>(TriggerObjects, name: nameof(TriggerObjects)));

            if (SpecialMapLayerOffset != 0)
                s.DoAt(Offset + SpecialMapLayerOffset, () => SpecialMapLayer = s.SerializeObject<MapLayer>(SpecialMapLayer, x => x.Pre_SharedDataPointer = Pre_SharedDataPointer, name: nameof(SpecialMapLayer)));

            MapLayers ??= new MapLayer[4];

            for (int i = 0; i < MapLayers.Length; i++)
            {
                if (MapLayerOffsets[i] == 0)
                    continue;

                s.DoAt(Offset + MapLayerOffsets[i], () => MapLayers[i] = s.SerializeObject<MapLayer>(MapLayers[i], x => x.Pre_SharedDataPointer = Pre_SharedDataPointer, name: $"{nameof(MapLayers)}[{i}]"));
            }

            // Go to the end
            s.Goto(Offset + Pre_FileSize);
        }
    }
}