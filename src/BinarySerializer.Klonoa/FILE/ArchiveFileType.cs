namespace BinarySerializer.Klonoa
{
    /// <summary>
    /// The different available types of archive files
    /// </summary>
    public enum ArchiveFileType
    {
        /// <summary>
        /// The default type. Has a header with an offset for each file.
        /// </summary>
        Default,

        /// <summary>
        /// The PF archive type in Klonoa Heroes. Has offsets and lengths for each file.
        /// </summary>
        KH_PF,

        /// <summary>
        /// The TP archive type in Klonoa Heroes. Has a header with an offset for each file.
        /// </summary>
        KH_TP,

        /// <summary>
        /// The KW archive type in Klonoa Heroes. Used for level maps and identifies them based on their ID. Has an end pointer.
        /// </summary>
        KH_KW,
    }
}