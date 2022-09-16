using System;

namespace BinarySerializer.Klonoa.KH
{
    public class MapsPack_ArchiveFile : ArchiveFile
    {
        public MapsPack_ArchiveFile() => Pre_Type = ArchiveFileType.KH_KW;

        public Map_File[] Maps { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Maps ??= new Map_File[OffsetTable.FilesCount];

            KlonoaSettings_KH.MapID serializeMap = s.Context.TryGetKlonoaSettings<KlonoaSettings_KH>()?.SerializeMap;

            for (int i = 0; i < Maps.Length; i++)
            {
                if (serializeMap != null && !(serializeMap.ID1 == OffsetTable.KH_KW_Entries[i].MapID1 && serializeMap.ID2 == OffsetTable.KH_KW_Entries[i].MapID2 && serializeMap.ID3 == OffsetTable.KH_KW_Entries[i].MapID3))
                    continue;

                Maps[i] = SerializeFile<Map_File>(s, Maps[i], i, onPreSerialize: x => x.Pre_SharedDataPointer = OffsetTable.KH_KW_SharedDataPointer, fileEncoder: new BytePairEncoder(), name: $"{nameof(Maps)}[{i}]");
            }
        }

        public Map_File GetMap(int id1, int id2, int id3, bool deserializeIfNull = true)
        {
            int index = -1;

            for (int i = 0; i < OffsetTable.KH_KW_Entries.Length; i++)
            {
                var entry = OffsetTable.KH_KW_Entries[i];

                if (entry.MapID1 == id1 && entry.MapID2 == id2 && entry.MapID3 == id3)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
                throw new Exception($"Map with ID {id1}-{id2}-{id3} was not found");

            return GetMap(index, deserializeIfNull);
        }

        public Map_File GetMap(int index, bool deserializeIfNull = true)
        {
            if (Maps[index] == null)
            {
                if (!deserializeIfNull)
                    return null;

                Maps[index] = SerializeFile<Map_File>(Context.Deserializer, Maps[index], index, onPreSerialize: x => x.Pre_SharedDataPointer = OffsetTable.KH_KW_SharedDataPointer, fileEncoder: new BytePairEncoder(), name: $"{nameof(Maps)}[{index}]");
            }

            return Maps[index];
        }
    }
}