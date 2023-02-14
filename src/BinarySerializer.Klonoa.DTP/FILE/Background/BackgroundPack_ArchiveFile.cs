using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// The backgrounds in a level. File extension is BA.
    /// </summary>
    public class BackgroundPack_ArchiveFile : ArchiveFile
    {
        public ArchiveFile<TIM> TIMFiles { get; set; } // Tilesets
        public ArchiveFile<CEL> CELFiles { get; set; } // Tiles
        public ArchiveFile<BGD> BGDFiles { get; set; } // Maps
        public ArchiveFile<BackgroundGameObjects_File> BackgroundGameObjectsFiles { get; set; } // One for each sector

        protected override void SerializeFiles(SerializerObject s)
        {
            TIMFiles = SerializeFile<ArchiveFile<TIM>>(s, TIMFiles, 0, name: nameof(TIMFiles));
            CELFiles = SerializeFile<ArchiveFile<CEL>>(s, CELFiles, 1, name: nameof(CELFiles));
            BGDFiles = SerializeFile<ArchiveFile<BGD>>(s, BGDFiles, 2, name: nameof(BGDFiles));
            BackgroundGameObjectsFiles = SerializeFile<ArchiveFile<BackgroundGameObjects_File>>(s, BackgroundGameObjectsFiles, 3, name: nameof(BackgroundGameObjectsFiles));
        }
    }
}