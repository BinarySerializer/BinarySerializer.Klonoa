namespace BinarySerializer.Klonoa.DTP
{
    public class GameObjectData_ModelVertexAnimation
    {
        public GameObjectData_ModelVertexAnimation(TMDVertices_File[] vertexFrames, TMDNormals_File[] normalFrames, int[] frameIndices, int[] frameSpeeds)
        {
            VertexFrames = vertexFrames;
            NormalFrames = normalFrames;
            FrameIndices = frameIndices;
            FrameSpeeds = frameSpeeds;
        }

        public TMDVertices_File[] VertexFrames { get; }
        public TMDNormals_File[] NormalFrames { get; }

        public int[] FrameIndices { get; }
        public int[] FrameSpeeds { get; }
    }
}