namespace BinarySerializer.Klonoa.KH
{
    public class Map_File : BaseFile
    {
        public Pointer Pre_SharedDataPointer { get; set; }

        public uint ObjectsOffset { get; set; }
        public uint MapTriggerObjectsOffset { get; set; }
        public uint SpecialMapLayerOffset { get; set; }
        public uint[] MapLayerOffsets { get; set; }

        public MapObjects MapObjects { get; set; }
        public MapTriggerObjects MapTriggerObjects { get; set; }

        public MapLayer SpecialMapLayer { get; set; } // Has palette, tileset and collision
        public MapLayer[] MapLayers { get; set; } // Has the map layers

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("KM", 4);
            s.SerializePadding(12, logIfNotNull: true);
            ObjectsOffset = s.Serialize<uint>(ObjectsOffset, name: nameof(ObjectsOffset));
            MapTriggerObjectsOffset = s.Serialize<uint>(MapTriggerObjectsOffset, name: nameof(MapTriggerObjectsOffset));
            SpecialMapLayerOffset = s.Serialize<uint>(SpecialMapLayerOffset, name: nameof(SpecialMapLayerOffset));
            s.SerializePadding(4, logIfNotNull: true);
            MapLayerOffsets = s.SerializeArray<uint>(MapLayerOffsets, 4, name: nameof(MapLayerOffsets));
            s.SerializePadding(16, logIfNotNull: true);

            if (ObjectsOffset != 0)
                s.DoAt(Offset + ObjectsOffset, () => MapObjects = s.SerializeObject<MapObjects>(MapObjects, name: nameof(MapObjects)));

            if (MapTriggerObjectsOffset != 0)
                s.DoAt(Offset + MapTriggerObjectsOffset, () => MapTriggerObjects = s.SerializeObject<MapTriggerObjects>(MapTriggerObjects, name: nameof(MapTriggerObjects)));

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