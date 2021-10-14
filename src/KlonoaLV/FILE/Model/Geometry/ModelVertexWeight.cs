namespace BinarySerializer.Klonoa.LV
{
    public class ModelVertexWeight : BinarySerializable
    {
        public byte[] Weights { get; set; }
        public byte WeightsSum { get; set; } // Divides weights so they all add up to 1.0
        public float[] NormalizedWeights => new float[] { Weights[0] / WeightsSum, Weights[1] / WeightsSum, Weights[2] / WeightsSum, Weights[3] / WeightsSum };

        public override void SerializeImpl(SerializerObject s)
        {
            Weights = s.SerializeArray<byte>(Weights, 4, name: nameof(Weights));

            WeightsSum = 0;
            foreach (byte w in Weights) WeightsSum += w;
        }
    }
}