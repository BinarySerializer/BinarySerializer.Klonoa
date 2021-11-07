using System.Linq;
using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// Handles loading hard-coded objects and their data
    /// </summary>
    public class HardCodedObjectsLoader : BaseHardCodedObjectsLoader
    {
        public HardCodedObjectsLoader(Loader loader, LevelPack_ArchiveFile levelPack, int binBlock, bool loadVramData = true) : base(loader, levelPack, binBlock, loadVramData) { }

        private void LoadCutscene_3_0()
        {
            // Window object
            AddCutsceneGameObject3D(GlobalGameObjectType.Cutscene_3_0_Window, obj =>
            {
                // TODO: Add rotation
                obj.Data_TMD = LoadCutsceneAsset<PS1_TMD>(0);
                obj.Data_Position = new KlonoaVector16(-0x10b0, -0x440, 0xdc0);
            });

            // Camera animation
            AddCutsceneGameObject3D(GlobalGameObjectType.Cutscene_Camera, obj =>
            {
                obj.Data_CameraAnimations = LoadCutsceneAsset<CameraAnimations_File>(1);
            });

            // TODO: Paths archive in file 3, used for some sprite object
        }

        private void LoadCutscene_3_1()
        {
            // Camera animation
            AddCutsceneGameObject3D(GlobalGameObjectType.Cutscene_Camera, obj =>
            {
                obj.Data_CameraAnimations = LoadCutsceneAsset<CameraAnimations_File>(2);
            });
        }

        private void LoadCutscene_5_0()
        {
            // Ghadius (FUN_5_7__8011bf00)
            AddCutsceneGameObject3D(GlobalGameObjectType.Cutscene_5_0_Ghadius, obj =>
            {
                obj.Data_TMD = LoadCutsceneAsset<PS1_TMD>(1); // Body
                obj.Data_TMD_Secondary = LoadCutsceneAsset<PS1_TMD>(0); // Head
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

            // TODO: What is file 24? 12 bytes. Gradient for Ghadius?
        }

        private void LoadCutscene_7_1()
        {
            // Karal
            AddCutsceneGameObject3D(GlobalGameObjectType.Cutscene_7_1_Karal, obj =>
            {
                obj.Data_TMD = LoadCutsceneAsset<PS1_TMD>(0, x => x.Pre_HasBones = true);
                LoadCutsceneAsset<RawData_ArchiveFile>(1);
                obj.Data_Position = new KlonoaVector16(0x70000 >> 12, 0x57b000 >> 12, 0xf30000 >> 12);

                var modelAnim = LoadCutsceneAsset<KaralModelBoneAnimation_ArchiveFile>(1);

                // Convert to normal model animation format
                obj.Data_ModelAnimations = new ArchiveFile<ModelBoneAnimation_ArchiveFile>()
                {
                    Files = modelAnim.Rotations.Select(x => new ModelBoneAnimation_ArchiveFile
                    {
                        File_0 = modelAnim.File_0,
                        Rotations = x,
                        Positions = modelAnim.Positions
                    }).ToArray()
                };

                // Some palette?
                obj.Data_RawVRAMData = LoadCutsceneAsset<RawData_File>(7);
                obj.Data_RawVRAMDataRegion = new PS1_VRAMRegion(0x110, 0x1eb, 0x10, 0x04);

                if (LoadVRAMData)
                    Loader.AddToVRAM(obj.Data_RawVRAMData.Data, obj.Data_RawVRAMDataRegion);
            });

            // Cage fence part
            AddCutsceneGameObject3D(GlobalGameObjectType.Cutscene_7_1_CageFence, obj =>
            {
                obj.Data_TMD = LoadCutsceneAsset<PS1_TMD>(2);
                obj.Data_Positions = LoadCutsceneAsset<VectorAnimation_File>(3);
                obj.Data_Rotations = LoadCutsceneAsset<VectorAnimation_File>(4, x =>
                {
                    x.Pre_ObjectsCount = obj.Data_Positions.ObjectsCount;
                    x.Pre_FramesCount = obj.Data_Positions.FramesCount;
                });
            });

            // TODO: File 5 has VRAM textures for end transition, file 6 has palettes
        }

        public override void LoadCutscene(int cutsceneIndex)
        {
            switch (BinBlock)
            {
                case 3 when cutsceneIndex is 0: LoadCutscene_3_0(); break;
                case 3 when cutsceneIndex is 1: LoadCutscene_3_1(); break;
                case 5 when cutsceneIndex is 0: LoadCutscene_5_0(); break;
                case 7 when cutsceneIndex is 1: LoadCutscene_7_1(); break;
                // Note: 7_2 is the second half of 7_1, but we don't load it again since it uses the same data
            }
        }
    }
}