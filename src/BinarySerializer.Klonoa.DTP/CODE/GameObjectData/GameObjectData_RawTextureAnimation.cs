using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class GameObjectData_RawTextureAnimation
    {
        public GameObjectData_RawTextureAnimation(byte[][] frames, PS1_VRAMRegion region)
        {
            Frames = frames;
            Region = region;
        }

        public byte[][] Frames { get; }
        public PS1_VRAMRegion Region { get; }
    }
}