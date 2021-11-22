namespace BinarySerializer.Klonoa.DTP
{
    public class CutsceneInstructionData_PlaySound : BaseCutsceneInstructionData
    {
        public byte ObjIndex { get; set; }
        public SoundReference SoundRef { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ObjIndex = s.Serialize<byte>(ObjIndex, name: nameof(ObjIndex));
            s.SerializePadding(1, logIfNotNull: false);
            SoundRef = s.SerializeObject<SoundReference>(SoundRef, name: nameof(SoundRef));
            s.SerializePadding(2, logIfNotNull: false);
        }
    }
}