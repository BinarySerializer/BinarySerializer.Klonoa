using System;

namespace BinarySerializer.Klonoa.KH
{
    public class WorldMapPack_ArchiveFile : ArchiveFile
    {
        public WorldMapPack_ArchiveFile() => Pre_Type = ArchiveFileType.KH_WMAP;

        public BaseFile[] Files { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            Files ??= new BaseFile[OffsetTable.FilesCount];

            for (int i = 0; i < Files.Length; i++)
            {
                OffsetTable.KH_WMAP_Entry entry = OffsetTable.KH_WMAP_Entries[i];

                if (entry.FileLength == 0)
                    continue;

                void serializeFile<T>(IStreamEncoder encoder = null)
                    where T : BaseFile, new()
                {
                    Files[i] = SerializeFile<T>(s, (T)Files[i], i, fileEncoder: encoder, name: $"{entry.Name}_{entry.ID}");
                }

                WorldMapFileType fileType = Enum.TryParse(entry.Name, out WorldMapFileType type) ? type : WorldMapFileType.Unknown;

                switch (fileType)
                {
                    case WorldMapFileType.BG0B:
                        serializeFile<RawData_File>(); // TODO: Parse
                        break;
                    
                    case WorldMapFileType.BG0P:
                        serializeFile<RawData_File>(); // TODO: Parse
                        break;

                    case WorldMapFileType.BG0W:
                        serializeFile<RawData_File>(); // TODO: Parse
                        break;

                    case WorldMapFileType.BG0V:
                        serializeFile<RawData_File>(); // TODO: Parse
                        break;

                    case WorldMapFileType.T2S4:
                        serializeFile<RawData_File>(); // TODO: Parse
                        break;

                    case WorldMapFileType.T2VA:
                        serializeFile<RawData_File>(); // TODO: Parse
                        break;

                    case WorldMapFileType.OBJ4:
                        serializeFile<RawData_File>(); // TODO: Parse
                        break;

                    case WorldMapFileType.OBJP:
                        serializeFile<RawData_File>(); // TODO: Parse
                        break;

                    case WorldMapFileType.TEST:
                        serializeFile<Graphics_File>();
                        break;
                    
                    case WorldMapFileType.WMAP:
                        serializeFile<Graphics_File>();
                        break;

                    case WorldMapFileType.VMAP:
                        serializeFile<RawData_File>(new BytePairEncoder()); // TODO: Parse
                        break;

                    case WorldMapFileType.Unknown:
                    default:
                        serializeFile<RawData_File>();
                        break;
                }
            }
        }
    }
}