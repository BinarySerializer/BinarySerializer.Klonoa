using BinarySerializer.GBA;

namespace BinarySerializer.Klonoa.KH
{
    public class KlonoaHeroesROM : GBA_ROMBase
    {
        public UnknownPack_ArchiveFile UnknownPack { get; set; }
        public ArchiveFile<Animation_File> AnimationPack1 { get; set; }
        public AnimationsPack2_ArchiveFile AnimationPack2 { get; set; }
        public UIPack_ArchiveFile UIPack { get; set; }
        public StoryPack_ArchiveFile StoryPack { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            base.SerializeImpl(s);

            // TODO: Move pointers to pointer table to avoid hard-coding them

            s.DoAt(new Pointer(0x08267940, Offset.File), () => UnknownPack = s.SerializeObject<UnknownPack_ArchiveFile>(UnknownPack, name: nameof(UnknownPack)));
            s.DoAt(new Pointer(0x08399f10, Offset.File), () => AnimationPack1 = s.SerializeObject<ArchiveFile<Animation_File>>(AnimationPack1, x => x.Pre_Type = ArchiveFileType.KH_TP, name: nameof(AnimationPack1)));
            s.DoAt(new Pointer(0x088fda70, Offset.File), () => AnimationPack2 = s.SerializeObject<AnimationsPack2_ArchiveFile>(AnimationPack2, name: nameof(AnimationPack2)));
            s.DoAt(new Pointer(0x0828fd20, Offset.File), () => UIPack = s.SerializeObject<UIPack_ArchiveFile>(UIPack, name: nameof(UIPack)));
            s.DoAt(new Pointer(0x0873d350, Offset.File), () => StoryPack = s.SerializeObject<StoryPack_ArchiveFile>(StoryPack, name: nameof(StoryPack)));
        }
    }
}