using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// Handles loading hard-coded objects and their data
    /// </summary>
    public class HardCodedObjectsLoader : BaseHardCodedObjectsLoader
    {
        public HardCodedObjectsLoader(LevelPack_ArchiveFile levelPack, int binBlock) : base(levelPack, binBlock) { }

        private void Load_3_0()
        {
            // Window object
            AddCutsceneGameObject3D(obj =>
            {
                // TODO: Add rotation
                obj.Data_TMD = LoadCutsceneAsset<PS1_TMD>(0);
                obj.Data_Position = new KlonoaVector16(-0x10b0, -0x440, 0xdc0);
            });

            // Camera animation
            AddCutsceneGameObject3D(obj =>
            {
                obj.Data_CameraAnimations = LoadCutsceneAsset<CameraAnimations_File>(1);
            });

            // TODO: Paths archive in file 3, used for some sprite object
        }

        private void Load_3_1()
        {
            // Camera animation
            AddCutsceneGameObject3D(obj =>
            {
                obj.Data_CameraAnimations = LoadCutsceneAsset<CameraAnimations_File>(2);
            });
        }

        private void Load_5_0()
        {
            // Ghadius head (FUN_5_7__8011bf00)
            AddCutsceneGameObject3D(obj =>
            {
                obj.Data_TMD = LoadCutsceneAsset<PS1_TMD>(0);
                obj.Data_Position = new KlonoaVector16(0, -8, 0x4D);
            });

            // Ghadius body (FUN_5_7__8011bf00)
            AddCutsceneGameObject3D(obj =>
            {
                obj.Data_TMD = LoadCutsceneAsset<PS1_TMD>(1);
                // TODO: Set at a reasonable position

                // Mouth animation (FUN_5_7__8011bff0)
                obj.TextureAnimationInfo = new KlonoaSettings_DTP.TextureAnimationInfo(false, 8);
                var frames = new byte[4][];

                for (int i = 0; i < 4; i++)
                    frames[i] = LoadCutsceneAsset<RawData_File>(25 + i).Data;

                obj.Data_RawTextureAnimation = new GameObject3D.RawTextureAnimation(frames, new PS1_VRAMRegion(0x240, 0x100, 0x10, 0x40));

                // Vertex animation (FUN_5_7__8011cbf0)
                obj.Data_VertexAnimation = new GameObject3D.ModelVertexAnimation(
                    vertexFrames: new TMDVertices_File[11], 
                    normalFrames: new TMDNormals_File[11], 
                    frameIndices: new int[]
                    {
                        0x00, 0x01, 0x02, 0x03,
                        0x04, 0x05, 0x06, 0x07,
                        0x08, 0x09, 0x0a, 0x09,
                        0x0a, 0x09, 0x08, 0x07,
                        0x06, 0x05, 0x04, 0x03,
                    }, 
                    frameSpeeds: new int[]
                    {
                        0x2d, 0x1e, 0x2d, 0x04,
                        0x06, 0x05, 0x0a, 0x08,
                        0x06, 0x04, 0x05, 0x05,
                        0x04, 0x05, 0x07, 0x08,
                        0x0a, 0x05, 0x06, 0x04,
                    });

                for (int i = 0; i < 11; i++)
                {
                    obj.Data_VertexAnimation.VertexFrames[i] = LoadCutsceneAsset<TMDVertices_File>(2 + i, x => x.Pre_VerticesCount = 0x63);
                    obj.Data_VertexAnimation.NormalFrames[i] = LoadCutsceneAsset<TMDNormals_File>(2 + 11 + i, x => x.Pre_NormalsCount = 0x140);
                }
            });
        }

        public override void LoadCutscene(int cutsceneIndex)
        {
            switch (BinBlock)
            {
                case 3 when cutsceneIndex is 0: Load_3_0(); break;
                case 3 when cutsceneIndex is 1: Load_3_1(); break;
                case 5 when cutsceneIndex is 0: Load_5_0(); break;
            }
        }
    }
}