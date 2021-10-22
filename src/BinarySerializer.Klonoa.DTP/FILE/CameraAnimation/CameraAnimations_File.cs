using System.Linq;

namespace BinarySerializer.Klonoa.DTP
{
    public class CameraAnimations_File : BaseFile
    {
        public Pointer AnimationsPointer { get; set; }
        public Pointer FlagsPointer { get; set; }
        public Pointer FramesPointer { get; set; }

        public CameraAnimation[] Animations { get; set; }
        public byte[] Flags { get; set; } // One bit per frame, determines if the data is relative or absolute
        public CameraAnimationFrame[] Frames { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Parsed at 0x800816e8

            AnimationsPointer = s.SerializePointer(AnimationsPointer, anchor: Offset, name: nameof(AnimationsPointer));
            FlagsPointer = s.SerializePointer(FlagsPointer, anchor: Offset, name: nameof(FlagsPointer));
            FramesPointer = s.SerializePointer(FramesPointer, anchor: Offset, name: nameof(FramesPointer));

            s.DoAt(AnimationsPointer, () => Animations = s.SerializeObjectArray<CameraAnimation>(Animations, (FlagsPointer - AnimationsPointer) / 4, name: nameof(Animations)));
            s.DoAt(FlagsPointer, () => Flags = s.SerializeArray<byte>(Flags, FramesPointer - FlagsPointer, name: nameof(Flags)));
            s.DoAt(FramesPointer, () =>
            {
                var i = 0;

                Frames = s.SerializeObjectArray(Frames, Animations.Last().Frame, onPreSerialize: x =>
                {
                    x.Pre_IsRelative = (Flags[i / 8] & (0x80 >> (i & 7))) != 0;
                    i++;
                }, name: nameof(Frames));
            });

            s.Goto(Offset + Pre_FileSize);
        }
    }
}