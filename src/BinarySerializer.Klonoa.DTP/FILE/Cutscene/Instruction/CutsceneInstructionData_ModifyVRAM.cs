using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_ModifyVRAM : BaseCutsceneInstructionData
    {
        public byte Byte_00 { get; set; }
        public int Int_02 { get; set; }

        public ModifyType Type => Int_02 < 0 ? ModifyType.ClearImage : ModifyType.MoveImage;

        public Rect ClearRegion => new(0x374, (short)(Byte_00 * -0x10 + 0x1f0), 0xc, 0x10);
        public Rect MoveRegion_Source => new(0x3f4, (short)(Int_02 * 0x10 + 0x180), 0xc, 0x10);
        public Rect MoveRegion_Destination => new(0x374, (short)(Byte_00 * -0x10 + 0x1f0), 0xc, 0x10);

        public override void SerializeImpl(SerializerObject s)
        {
            Byte_00 = s.Serialize<byte>(Byte_00, name: nameof(Byte_00));
            s.SerializePadding(1, logIfNotNull: false);
            Int_02 = s.Serialize<int>(Int_02, name: nameof(Int_02));
        }

        public enum ModifyType
        {
            ClearImage,
            MoveImage,
        }
    }
}