using BinarySerializer.GBA;

namespace BinarySerializer.Klonoa.KH
{
    public class KlonoaHeroesROM : GBA_ROMBase
    {
        public Graphics_File Font { get; set; }

        public StoryPack_ArchiveFile StoryPack { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            base.SerializeImpl(s);

            // TODO: Move pointers to pointer table to avoid hard-coding them

            s.DoAt(new Pointer(0x0829666c, Offset.File), () => Font = s.SerializeObject<Graphics_File>(Font, name: nameof(Font)));
            s.DoAt(new Pointer(0x0873d350, Offset.File), () => StoryPack = s.SerializeObject<StoryPack_ArchiveFile>(StoryPack, name: nameof(StoryPack)));
        }
    }
}