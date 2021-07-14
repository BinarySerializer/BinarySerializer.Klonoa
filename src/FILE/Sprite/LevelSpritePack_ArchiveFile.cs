namespace BinarySerializer.KlonoaDTP
{
    public class LevelSpritePack_ArchiveFile : BaseArchiveFile
    {
        // The game does this:
        // - Modify all offsets to pointers
        // - Enumerate all files (always 71 of them) and copy the pointers to an array
        // - Enumerate every pointer in this new array, skipping the first 2 and ignoring any pointer which is the same as file 0 - then modifying these files sub-file offsets to pointers
        // - Add the fixed sprites file to this list of files and increment the file count

        // First file is always a dummy file
        public RawData_File DummyFile { get; set; }

        public PlayerSprites_ArchiveFile PlayerSprites { get; set; }

        // Each file seems to correspond to an object type. For example the first one is the Moo enemy.
        // If an object is unused in the level then the file is nulled out too (it points to the dummy entry).
        public Sprites_ArchiveFile[] Sprites { get; set; }

        protected override void SerializeFiles(SerializerObject s)
        {
            DummyFile = SerializeFile<RawData_File>(s, DummyFile, 0, name: nameof(DummyFile));
            PlayerSprites = SerializeFile<PlayerSprites_ArchiveFile>(s, PlayerSprites, 1, name: nameof(PlayerSprites));

            Sprites ??= new Sprites_ArchiveFile[71 - 2];

            // Enumerate remaining file
            for (int i = 0; i < Sprites.Length; i++)
            {
                // Ignore dummy files
                if (OffsetTable.FilePointers[2 + i] == OffsetTable.FilePointers[0])
                    continue;

                Sprites[i] = SerializeFile<Sprites_ArchiveFile>(s, Sprites[i], 2 + i, name: $"{nameof(Sprites)}[{i}]");
            }
        }
    }
}