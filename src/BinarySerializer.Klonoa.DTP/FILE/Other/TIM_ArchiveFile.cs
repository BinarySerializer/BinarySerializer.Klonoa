using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    // According to the prototype the source extensions for the data in this is .TIA (TIM archive?)

    /// <summary>
    /// An archive file where every file is a TIM file
    /// </summary>
    public class TIM_ArchiveFile : ArchiveFile<BaseFile<PS1_TIM>>
    {
        public override bool DisableNotFullySerializedWarning => true; // If it only has a palette the dummy texture data can be of varying length
    }
}