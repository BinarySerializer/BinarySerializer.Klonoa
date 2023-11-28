namespace BinarySerializer.Klonoa.BV
{
    /// <summary>
    /// Defines a group of texcoords, which is made up of multiple texcoord maps.<br/>
    /// Starting from <c>Texcoords[TexcoordsOffset]</c>, there is a 2D array of texcoords associated with this group:
    /// <c>VMDG_Texcoord[TexcoordMapCount][TexcoordCount]</c>.
    /// </summary>
    public class VMDG_TexcoordGroup : BinarySerializable
    {
        /// <summary>
        /// Start index of this texcoord group's texcoords in the <c>Texcoords</c> buffer.
        /// </summary>
        public ushort TexcoordsOffset { get; set; }

        /// <summary>
        /// The number of texcoords in this texcoord group.
        /// </summary>
        public ushort TexcoordCount { get; set; }

        /// <summary>
        /// The number of texcoord maps in this texcoord group.<br/>
        /// Texcoord maps are used to swap between different sets of UVs, primarily for character facial expressions.
        /// </summary>
        public ushort TexcoordMapCount { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializePadding(2, logIfNotNull: true);
            TexcoordsOffset = s.Serialize<ushort>(TexcoordsOffset, name: nameof(TexcoordsOffset));
            TexcoordCount = s.Serialize<ushort>(TexcoordCount, name: nameof(TexcoordCount));
            TexcoordMapCount = s.Serialize<ushort>(TexcoordMapCount, name: nameof(TexcoordMapCount));
        }
    }
}