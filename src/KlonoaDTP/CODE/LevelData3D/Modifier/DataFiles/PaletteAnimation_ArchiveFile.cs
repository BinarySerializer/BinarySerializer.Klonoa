using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class PaletteAnimation_ArchiveFile : ArchiveFile<ArchiveFile<PaletteAnimation_ArchiveFile.Palette>>
    {
        public LoaderConfiguration_DTP.PaletteAnimationInfo AnimationInfo { get; set; }
        public PS1_VRAMRegion[] VRAMRegions { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize archive
            base.SerializeImpl(s);

            // Get the animation info
            var loader = Loader_DTP.GetLoader(s.Context);
            AnimationInfo = loader.Config.PaletteAnimationInfos[loader.BINBlock];

            s.DoAt(new Pointer(AnimationInfo.Address_Regions, loader.FindCodeFile(AnimationInfo.Address_Regions)), () =>
                VRAMRegions = s.SerializeObjectArray<PS1_VRAMRegion>(VRAMRegions, OffsetTable.FilesCount, name: nameof(VRAMRegions)));
        }

        public class Palette : BinarySerializable
        {
            public RGBA5551Color[] Colors { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                Colors = s.SerializeObjectArray<RGBA5551Color>(Colors, 16, name: nameof(Colors));
            }
        }
    }
}