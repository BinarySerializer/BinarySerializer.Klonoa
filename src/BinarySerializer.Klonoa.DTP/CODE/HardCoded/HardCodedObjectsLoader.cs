using System.Linq;
using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    /// <summary>
    /// Handles loading hard-coded objects and their data
    /// </summary>
    public class HardCodedObjectsLoader : BaseHardCodedObjectsLoader
    {
        #region Constructor

        public HardCodedObjectsLoader(Loader loader, bool loadVramData = true) : base(loader, loadVramData) { }

        #endregion

        #region Cutscenes

        private void LoadCutsceneObjects_3_0()
        {
            // Window object
            AddGameObject(GlobalGameObjectType.Cutscene_Window, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(0),
                        Position = new KlonoaVector16(-0x10b0, -0x440, 0xdc0),
                        // TODO: Add rotation
                    },
                };
            });

            // Camera animation
            AddGameObject(GlobalGameObjectType.Cutscene_Camera, obj =>
            {
                obj.CameraAnimations = LoadCutsceneAsset<CameraAnimations_File>(1);
            });

            // TODO: Paths archive in file 3, used for some sprite object
        }

        private void LoadCutsceneObjects_3_1()
        {
            // Camera animation
            AddGameObject(GlobalGameObjectType.Cutscene_Camera, obj =>
            {
                obj.CameraAnimations = LoadCutsceneAsset<CameraAnimations_File>(2);
            });
        }

        private void LoadCutsceneObject_Ghadius(int headTmdIndex, int bodyTmdIndex, int animIndex, int textureAnimIndex, KlonoaVector16 pos, PS1_VRAMRegion vramRegion)
        {
            AddGameObject(GlobalGameObjectType.Cutscene_Ghadius, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    // Head
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(headTmdIndex),
                    },
                    // Body
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(bodyTmdIndex),
                    },
                };

                obj.Position = pos;

                if (textureAnimIndex != -1)
                {
                    // Mouth animation (FUN_5_7__8011bff0)
                    obj.TextureAnimationInfo = new KlonoaSettings_DTP.TextureAnimationInfo(false, 8);
                    var frames = new byte[4][];

                    for (int i = 0; i < 4; i++)
                        frames[i] = LoadCutsceneAsset<RawData_File>(textureAnimIndex + i).Data;

                    obj.RawTextureAnimation = new GameObjectData_RawTextureAnimation(frames, vramRegion);
                }

                if (animIndex != -1)
                {
                    // Vertex animation (FUN_5_7__8011cbf0)
                    obj.Models[1].VertexAnimation = new GameObjectData_ModelVertexAnimation(
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
                        obj.Models[1].VertexAnimation.VertexFrames[i] = LoadCutsceneAsset<TMDVertices_File>(animIndex + i, x => x.Pre_VerticesCount = 0x63);
                        obj.Models[1].VertexAnimation.NormalFrames[i] = LoadCutsceneAsset<TMDNormals_File>(animIndex + 11 + i, x => x.Pre_NormalsCount = 0x140);
                    }
                }
            });
        }

        private void LoadCutsceneObjects_5_0()
        {
            // Ghadius (FUN_5_7__8011bf00)
            LoadCutsceneObject_Ghadius(0, 1, 2, 25, new KlonoaVector16(24, -40, 0), new PS1_VRAMRegion(0x240, 0x100, 0x10, 0x40)); // TODO: Fix pos

            // TODO: What is file 24? 12 bytes.
        }

        private void LoadCutsceneObject_Karal_Pamela(int tmdIndex, int animIndex, int palIndex, KlonoaVector16 pos)
        {
            AddGameObject(GlobalGameObjectType.Cutscene_Karal_Pamela, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(tmdIndex, x => x.Pre_HasBones = true),
                    }
                };

                obj.Position = pos;

                var modelAnim = LoadCutsceneAsset<KaralModelBoneAnimation_ArchiveFile>(animIndex);

                // Convert to normal model animation format
                obj.Models[0].ModelAnimations = new ArchiveFile<ModelBoneAnimation_ArchiveFile>()
                {
                    Files = modelAnim.Rotations.Select(x => new ModelBoneAnimation_ArchiveFile
                    {
                        File_0 = modelAnim.File_0,
                        Rotations = x,
                        Positions = modelAnim.Positions
                    }).ToArray()
                };

                // Some palette (Karal only)
                if (palIndex != -1)
                {
                    obj.RawVRAMData = LoadCutsceneAsset<RawData_File>(palIndex);
                    obj.RawVRAMDataRegion = new PS1_VRAMRegion(0x110, 0x1eb, 0x10, 0x04);

                    if (LoadVRAMData)
                        Loader.AddToVRAM(obj.RawVRAMData.Data, obj.RawVRAMDataRegion);
                }
            });
        }

        private void LoadCutsceneObjects_7_5()
        {
            // Karal
            LoadCutsceneObject_Karal_Pamela(0, 1, 7, new KlonoaVector16(0x70000 >> 12, 0x57b000 >> 12, 0xf30000 >> 12));

            // Cage fence part
            AddGameObject(GlobalGameObjectType.Cutscene_CageFence, obj =>
            {
                obj.Positions = LoadCutsceneAsset<VectorAnimation_File>(3);
                obj.Rotations = LoadCutsceneAsset<VectorAnimation_File>(4, x =>
                {
                    x.Pre_ObjectsCount = obj.Positions.ObjectsCount;
                    x.Pre_FramesCount = obj.Positions.FramesCount;
                });

                PS1_TMD tmd = LoadCutsceneAsset<PS1_TMD>(2);

                obj.Models = Enumerable.Range(0, obj.Positions.ObjectsCount).Select(x => new GameObjectData_Model()
                {
                    TMD = tmd,
                    Position = obj.Positions.Vectors[0][x],
                    Rotation = obj.Rotations.Vectors[0][x],
                }).ToArray();

            });

            // TODO: File 5 has VRAM textures for end transition, file 6 has palettes
        }

        private void LoadCutsceneObjects_8_0()
        {
            LoadCutsceneObject_Karal_Pamela(0, 1, 2, new KlonoaVector16(0, 354, -1701));
        }

        private void LoadCutsceneObjects_8_1()
        {
            LoadCutsceneObject_Karal_Pamela(0, 1, 2, new KlonoaVector16(-13897, -1120, -1143));
        }

        private void LoadCutsceneObjects_9_3()
        {
            // Moving platform
            AddGameObject(GlobalGameObjectType.Cutscene_MovingPlatform, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(0),
                    }
                };

                obj.Collision = LoadCutsceneAsset<CollisionTriangles_File>(1);
                obj.Position = new KlonoaVector16(-6910, -1458, 462); // Defined in script
            });
        }

        private void LoadCutsceneObjects_10_0()
        {
            // Moving platform
            AddGameObject(GlobalGameObjectType.Cutscene_MovingPlatform, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(0),
                    }
                };

                obj.Position = new KlonoaVector16(34, -366, 2746); // Defined in script
            });
        }

        private void LoadCutsceneObjects_11_0()
        {
            AddGameObject(GlobalGameObjectType.Cutscene_MovingPlatform, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(0),
                    },
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(1),
                    },
                };

                obj.Collision = LoadCutsceneAsset<CollisionTriangles_File>(2);
                obj.Position = new KlonoaVector16(-192, 1280, 616); // Defined in script
            });
        }

        private void LoadCutsceneObject_Airplane(int tmdIndex, int propellerTmdIndex, KlonoaVector16 pos)
        {
            // Airplane
            AddGameObject(GlobalGameObjectType.Cutscene_Airplane, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    // Body
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(tmdIndex),
                        Position = pos,
                    },
                    // Propeller
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(propellerTmdIndex),
                        Position = new KlonoaVector16((short)(pos.X + 0), (short)(pos.Y + (-0x86000 >> 12)), (short)(pos.Z + (0x240000 >> 12))),
                    },
                };
            });
        }

        private void LoadCutsceneObjects_13_7()
        {
            // NOTE: Positions aren't really accurate and the propeller is duplicated
            LoadCutsceneObject_Airplane(0, 1, new KlonoaVector16(688, -2304, 4096));
            LoadCutsceneObject_Airplane(5, 1, new KlonoaVector16(688, -1500, 4096));
        }

        private void LoadCutsceneObjects_14_0()
        {
            // Airplane
            LoadCutsceneObject_Airplane(0, 1, new KlonoaVector16(-768, -768, -1024)); // Defined in script

            // Level geometry
            AddGameObject(GlobalGameObjectType.Cutscene_Geometry, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(2),
                    },
                };
            });
        }

        private void LoadCutsceneObjects_14_1()
        {
            // Pamela
            LoadCutsceneObject_Karal_Pamela(3, 4, -1, new KlonoaVector16(-3689, -468, 447));
        }

        private void LoadCutsceneObjects_15_0()
        {
            // Pamela
            LoadCutsceneObject_Karal_Pamela(0, 1, -1, new KlonoaVector16(-5246, 279, 606));
        }

        private void LoadCutsceneObjects_17_0()
        {
            // Ghadius
            LoadCutsceneObject_Ghadius(0, 1, 2, 24, new KlonoaVector16(-32, -64, 0), new PS1_VRAMRegion(0x2C0, 0x100, 0x10, 0x40));
        }

        private void LoadCutsceneObjects_18_0()
        {
            // Pamela
            LoadCutsceneObject_Karal_Pamela(0, 1, -1, new KlonoaVector16(-2877, 1007, 5868));
        }

        private void LoadCutsceneObjects_21_0()
        {
            // TODO: Position and duplicate
            // Canon platform
            AddGameObject(GlobalGameObjectType.Cutscene_BeamSource, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(0),
                    },
                };
            });
        }

        private void LoadCutsceneObjects_22_0()
        {
            // Karal
            LoadCutsceneObject_Karal_Pamela(0, 1, -1, new KlonoaVector16(0x70000 >> 12, 0x57b000 >> 12, 0xf30000 >> 12));

            // Pamela (offset y slightly)
            LoadCutsceneObject_Karal_Pamela(4, 5, -1, new KlonoaVector16(0x70000 >> 12, (0x57b000 >> 12) + 1500, 0xf30000 >> 12));

            // Ghadius (on floor)
            LoadCutsceneObject_Ghadius(2, 3, -1, -1, new KlonoaVector16(-0xf0000 >> 12, -0x8000 >> 12, 0x11a000 >> 12), null);
        }

        private void LoadCutsceneObjects_23_0()
        {
            // Pamela
            LoadCutsceneObject_Karal_Pamela(0, 1, -1, new KlonoaVector16(0x70000 >> 12, 0x57b000 >> 12, 0xf30000 >> 12));

            // Beam source
            AddGameObject(GlobalGameObjectType.Cutscene_BeamSource, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(0),
                    },
                };
            });
        }

        #endregion

        #region Bosses

        private void LoadBossObjects_8_0()
        {
            // Pamela
            AddGameObject(GlobalGameObjectType.Boss_Pamela, obj =>
            {
                var anim = LoadBossAsset<PamelaBossModelBoneAnimation_ArchiveFile>(1);

                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(0, x => x.Pre_HasBones = true), // Note: File 4 is a duplicate of this
                        ModelAnimations = new ArchiveFile<ModelBoneAnimation_ArchiveFile>()
                        {
                            Files = Enumerable.Range(0, anim.Rotations.Length).Select(x => new ModelBoneAnimation_ArchiveFile
                            {
                                File_0 = anim.File_0,
                                Rotations = anim.Rotations[x],
                                Positions = anim.Positions[x],
                            }).ToArray()
                        }
                    },
                };
            });
        }

        #endregion

        #region Public Methods

        public override void LoadObjects()
        {
            switch (BinBlock)
            {
                case 3 when LevelSector is 0:
                    LoadCutsceneObjects_3_0();
                    break;

                case 3 when LevelSector is 1:
                    LoadCutsceneObjects_3_1();
                    break;

                // TODO: 3 & 4 have boss assets defined, but they are unused - what are they? Seems to be some cutscene asset duplicates?

                case 5 when LevelSector is 0:
                    LoadCutsceneObjects_5_0();
                    break;

                case 7 when LevelSector is 5:
                    LoadCutsceneObjects_7_5();
                    break;

                case 8 when LevelSector is 0:
                    LoadCutsceneObjects_8_0();
                    LoadBossObjects_8_0();
                    break;

                case 8 when LevelSector is 1:
                    LoadCutsceneObjects_8_1();
                    break;

                case 9 when LevelSector is 3:
                    LoadCutsceneObjects_9_3();
                    break;

                case 10 when LevelSector is 0:
                    LoadCutsceneObjects_10_0();
                    break;

                // TODO: 10 files 1-5 have VRAM textures and palette for end transition

                case 11 when LevelSector is 0:
                    LoadCutsceneObjects_11_0();
                    break;

                case 13 when LevelSector is 7:
                    LoadCutsceneObjects_13_7();
                    break;

                // TODO: 13 2-4 have VRAM textures and palette for end transition

                case 14 when LevelSector is 0:
                    LoadCutsceneObjects_14_0();
                    break;

                case 14 when LevelSector is 1:
                    LoadCutsceneObjects_14_1();
                    break;

                case 15 when LevelSector is 0:
                    LoadCutsceneObjects_15_0();
                    break;

                // TODO: 16 0-2 have VRAM textures and palette for end transition

                case 17 when LevelSector is 0:
                    LoadCutsceneObjects_17_0();
                    break;

                case 18 when LevelSector is 0:
                    LoadCutsceneObjects_18_0();
                    break;

                // TODO: 19 0 has VRAM textures for end transition

                case 21 when LevelSector is 0:
                    LoadCutsceneObjects_21_0();
                    break;

                // TODO: 21 1-14 have VRAM textures and palette for end transition

                case 22 when LevelSector is 0:
                    LoadCutsceneObjects_22_0();
                    break;

                // TODO: 22 6 has VRAM textures for end transition

                case 23 when LevelSector is 0:
                    LoadCutsceneObjects_23_0();
                    break;
            }
        }

        #endregion
    }
}