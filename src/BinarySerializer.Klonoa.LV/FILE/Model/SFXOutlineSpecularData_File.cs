namespace BinarySerializer.Klonoa.LV
{
    public class SFXOutlineSpecularData_File : BaseFile
    {
        /// <summary>
        /// The name/ID of the SFX model, usually 3 characters.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Outline flag for each SFX part.
        /// </summary>
        public OutlineFlag[] OutlineFlags { get; set; }

        /// <summary>
        /// Specular light toggle (0 or 1) for each SFX part.
        /// </summary>
        public byte[] SpecularToggle { get; set; }
        
        public override void SerializeImpl(SerializerObject s) 
        {
            Name = s.SerializeString(Name, name: nameof(Name));
            s.Align(16);
            if (Pre_FileSize > 0x10) {
                OutlineFlags = s.SerializeArray<OutlineFlag>(OutlineFlags, 64, name: nameof(OutlineFlags));
                SpecularToggle = s.SerializeArray<byte>(SpecularToggle, 64, name: nameof(SpecularToggle));
            } else {
                OutlineFlags ??= new OutlineFlag[64];
                SpecularToggle ??= new byte[64];
            }
        }
    }

    public enum OutlineFlag : byte
    {
        /// <summary>Enable outline</summary>
        Enable = 0,

        /// <summary>Enable outline and end outline group</summary>
        GroupEnd = 1,

        /// <summary>Enable outline and mark as outline group member</summary>
        GroupMember = 2,

        /// <summary>Enable outline and start outline group</summary>
        GroupStart = 3,

        /// <summary>Disable outline</summary>
        Disable = 4,

        /// <summary>Unknown (5)</summary>
        Unk5 = 5,

        /// <summary>Unknown (6)</summary>
        Unk6 = 6
    }
}