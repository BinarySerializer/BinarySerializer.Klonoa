using System.Collections.Generic;

namespace BinarySerializer.Klonoa
{
    public abstract class KlonoaSettings
    {
        /// <summary>
        /// The game version
        /// </summary>
        public abstract KlonoaGameVersion Version { get; }

        /// <summary>
        /// Optional set of relocated files. The key is the parent archive file pointers. This gets used when parsing an <see cref="ArchiveFile"/>
        /// to correctly determine the file sizes if any of its files have been moved from their original locations. This is primarily for
        /// Klonoa Heroes where repacking an archive isn't always possible due to it being inside of a ROM.
        /// </summary>
        public Dictionary<Pointer, HashSet<RelocatedFile>> RelocatedFiles { get; set; }

        public class RelocatedFile
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="originalPointer">The original file pointer before it was relocated from the offset table. This should only be set if the original data still exists in the archive. If the archive was repacked after the file was relocated then this should be null.</param>
            /// <param name="newPointer">The new file pointer. This should match the pointer in the archive offset table.</param>
            /// <param name="fileSize">The new file size</param>
            public RelocatedFile(Pointer originalPointer, Pointer newPointer, uint fileSize)
            {
                OriginalPointer = originalPointer;
                NewPointer = newPointer;
                FileSize = fileSize;
            }

            /// <summary>
            /// The original file pointer before it was relocated from the offset table. This should only be set if the original data still exists
            /// in the archive. If the archive was repacked after the file was relocated then this should be null.
            /// </summary>
            public Pointer OriginalPointer { get; }

            /// <summary>
            /// The new file pointer. This should match the pointer in the archive offset table.
            /// </summary>
            public Pointer NewPointer { get; }

            /// <summary>
            /// The new file size
            /// </summary>
            public uint FileSize { get; }
        }
    }
}