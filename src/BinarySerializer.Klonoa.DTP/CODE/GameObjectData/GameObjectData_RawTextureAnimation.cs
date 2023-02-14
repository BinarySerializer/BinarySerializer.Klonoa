using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class GameObjectData_RawTextureAnimation
    {
        public GameObjectData_RawTextureAnimation(byte[][] frames, Rect region)
        {
            Frames = frames;
            Region = region;
        }

        public byte[][] Frames { get; }
        public Rect Region { get; }
    }
}