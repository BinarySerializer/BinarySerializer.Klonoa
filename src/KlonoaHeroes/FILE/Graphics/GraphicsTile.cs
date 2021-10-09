namespace BinarySerializer.Klonoa.KH
{
    public class GraphicsTile : BinarySerializable
    {
        public int TileSetIndex { get; set; }

        // TODO: Is this correct?
        public bool FlipX { get; set; }
        public bool FlipY { get; set; }
        public int PaletteIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeBitValues<ushort>(bitFunc =>
            {
                TileSetIndex = bitFunc(TileSetIndex, 10, name: nameof(TileSetIndex));
                FlipX = bitFunc(FlipX ? 1 : 0, 1, name: nameof(FlipX)) == 1;
                FlipY = bitFunc(FlipY ? 1 : 0, 1, name: nameof(FlipY)) == 1;
                PaletteIndex = bitFunc(PaletteIndex, 4, name: nameof(PaletteIndex));
            });
        }
    }
}