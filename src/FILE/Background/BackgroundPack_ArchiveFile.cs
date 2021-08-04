using BinarySerializer.PS1;

namespace BinarySerializer.KlonoaDTP
{
    /// <summary>
    /// The backgrounds in a level
    /// </summary>
    public class BackgroundPack_ArchiveFile : ArchiveFile
    {
        public ArchiveFile<PS1_TIM> TIMFiles { get; set; } // Tilesets
        public ArchiveFile<PS1_CEL> CELFiles { get; set; } // Tiles
        public ArchiveFile<PS1_BGD> BGDFiles { get; set; } // Maps
        public ArchiveFile<BackgroundDescriptors_File> BackgroundDescriptorsFiles { get; set; } // One for each sector

        protected override void SerializeFiles(SerializerObject s)
        {
            TIMFiles = SerializeFile<ArchiveFile<PS1_TIM>>(s, TIMFiles, 0, name: nameof(TIMFiles));
            CELFiles = SerializeFile<ArchiveFile<PS1_CEL>>(s, CELFiles, 1, name: nameof(CELFiles));
            BGDFiles = SerializeFile<ArchiveFile<PS1_BGD>>(s, BGDFiles, 2, name: nameof(BGDFiles));
            BackgroundDescriptorsFiles = SerializeFile<ArchiveFile<BackgroundDescriptors_File>>(s, BackgroundDescriptorsFiles, 3, name: nameof(BackgroundDescriptorsFiles));
        }
    }
}