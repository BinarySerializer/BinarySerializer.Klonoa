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

        protected virtual void LoadCutsceneObjects_3_0()
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
                        Rotation = new KlonoaVector16(0, 2892, 0),
                    },
                };
            });

            // Camera animation
            AddGameObject(GlobalGameObjectType.Cutscene_Camera, obj =>
            {
                obj.CameraAnimations = LoadCutsceneAsset<CameraAnimations_File>(1);
            });

            // Sprites movement paths
            AddGameObject(GlobalGameObjectType.Cutscene_Paths, obj =>
            {
                obj.MovementPathsArchive = LoadCutsceneAsset<ArchiveFile<MovementPath_File>>(3);
            });
        }

        protected virtual void LoadCutsceneObjects_3_1()
        {
            // Camera animation
            AddGameObject(GlobalGameObjectType.Cutscene_Camera, obj =>
            {
                obj.CameraAnimations = LoadCutsceneAsset<CameraAnimations_File>(2);
            });
        }

        protected virtual void LoadCutsceneObject_Ghadius(int headTmdIndex, int bodyTmdIndex, int animIndex, int textureAnimIndex, KlonoaVector16 pos, KlonoaVector16 rot, PS1_VRAMRegion vramRegion)
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
                obj.Rotation = rot;

                if (textureAnimIndex != -1)
                {
                    // Mouth animation (FUN_5_7__8011bff0)
                    obj.TextureAnimationInfo = new KlonoaSettings_DTP_PS1.TextureAnimationInfo(false, 8);
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

        protected virtual void LoadCutsceneObjects_5_0()
        {
            // Ghadius (FUN_5_7__8011bf00)
            LoadCutsceneObject_Ghadius(0, 1, 2, 25, 
                new KlonoaVector16(0, (-0x8000 >> 12) - (0x259000 >> 12), -0x400), // Custom z value 
                new KlonoaVector16(0, 0xa40000 >> 12, 0), 
                new PS1_VRAMRegion(0x240, 0x100, 0x10, 0x40));

            // Sprite
            AddGameObject(GlobalGameObjectType.Cutscene_Sprites, obj =>
            {
                SpriteTexture tex = LoadCutsceneAsset<SpriteTexture>(24);

                obj.Sprites = new GameObjectData_Sprites[]
                {
                    new GameObjectData_Sprites()
                    {
                        Sprites = new Sprite_File[]
                        {
                            new Sprite_File()
                            {
                                Textures = new SpriteTexture[]
                                {
                                    tex
                                }
                            }
                        }
                    }
                };
              });
        }

        protected virtual void LoadCutsceneObject_Karal_Pamela(GlobalGameObjectType type, int tmdIndex, int animIndex, int palIndex, KlonoaVector16 pos, KlonoaVector16 rot = null)
        {
            AddGameObject(type, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(tmdIndex, x => x.Pre_HasBones = true),
                    }
                };

                obj.Position = pos;
                obj.Rotation = rot;

                var modelAnim = LoadCutsceneAsset<KaralModelBoneAnimation_ArchiveFile>(animIndex);

                // Convert to normal model animation format
                obj.Models[0].ModelBoneAnimations = new GameObjectData_ModelBoneAnimations()
                {
                    InitialBonePositions = modelAnim.Positions,
                    Animations = modelAnim.Rotations.Select(x => new GameObjectData_ModelBoneAnimation
                    {
                        BoneRotations = x,
                    }).ToArray(),
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

        protected virtual void LoadCutsceneObjects_7_5()
        {
            // Karal
            LoadCutsceneObject_Karal_Pamela(GlobalGameObjectType.Cutscene_Karal, 0, 1, 7, new KlonoaVector16(0x70000 >> 12, 0x57b000 >> 12, 0xf30000 >> 12), new KlonoaVector16(0, 0x6000 >> 12, 0));

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

        protected virtual void LoadCutsceneObjects_8_0()
        {
            LoadCutsceneObject_Karal_Pamela(GlobalGameObjectType.Cutscene_Karal, 0, 1, 2, new KlonoaVector16(0, 354, -2500), new KlonoaVector16(0, 0x800, 0)); // Custom position and rotation
        }

        protected virtual void LoadCutsceneObjects_8_1()
        {
            LoadCutsceneObject_Karal_Pamela(GlobalGameObjectType.Cutscene_Karal, 0, 1, 2, new KlonoaVector16(-13897, -1120, -1143));
        }

        protected virtual void LoadCutsceneObjects_9_3()
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

        protected virtual void LoadCutsceneObjects_10_0()
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

        protected virtual void LoadCutsceneObjects_11_0()
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

        protected virtual void LoadCutsceneObject_Airplane(int tmdIndex, int propellerTmdIndex, KlonoaVector16 pos)
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

        protected virtual void LoadCutsceneObjects_13_7()
        {
            // NOTE: Positions aren't really accurate
            LoadCutsceneObject_Airplane(0, 1, new KlonoaVector16(688, -2304, 4096));
            LoadCutsceneObject_Airplane(5, 1, new KlonoaVector16(688, -1500, 4096));
        }

        protected virtual void LoadCutsceneObjects_14_0()
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

        protected virtual void LoadCutsceneObjects_14_1()
        {
            // Pamela
            LoadCutsceneObject_Karal_Pamela(GlobalGameObjectType.Cutscene_Pamela, 3, 4, -1, new KlonoaVector16(-3689, -468, 447));
        }

        protected virtual void LoadCutsceneObjects_15_0()
        {
            // Pamela
            LoadCutsceneObject_Karal_Pamela(GlobalGameObjectType.Cutscene_Pamela, 0, 1, -1, new KlonoaVector16(-5246, 279, 606));
        }

        protected virtual void LoadCutsceneObjects_17_0()
        {
            // Ghadius
            LoadCutsceneObject_Ghadius(0, 1, 2, 24, new KlonoaVector16(-32, 3800, 0), new KlonoaVector16(0, 0x800, 0), new PS1_VRAMRegion(0x2C0, 0x100, 0x10, 0x40));
        }

        protected virtual void LoadCutsceneObjects_18_0()
        {
            // Pamela
            LoadCutsceneObject_Karal_Pamela(GlobalGameObjectType.Cutscene_Pamela, 0, 1, -1, new KlonoaVector16(-2877, 1007, 5868));
        }

        protected virtual void LoadCutsceneObjects_21_0()
        {
            // TODO: Position and duplicate
            // Canon platform
            AddGameObject(GlobalGameObjectType.Cutscene_Canon, obj =>
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

        protected virtual void LoadCutsceneObjects_21_1()
        {
            for (int i = 0; i < 3; i++)
            {
                RawData_File vramData = LoadCutsceneAsset<RawData_File>(12 + i);

                // Overwrites sprite data so the sprites in this sector will work
                if (LoadVRAMData)
                    Loader.AddToVRAM(vramData.Data, new PS1_VRAMRegion((short)(0x200 + (0x40 * i)), 0, 0x40, 0x100));
            }
        }

        protected virtual void LoadCutsceneObjects_22_0()
        {
            // Karal
            LoadCutsceneObject_Karal_Pamela(GlobalGameObjectType.Cutscene_Karal, 0, 1, -1, new KlonoaVector16(0x70000 >> 12, 0x57b000 >> 12, 0xf30000 >> 12));

            // Pamela (offset y slightly)
            LoadCutsceneObject_Karal_Pamela(GlobalGameObjectType.Cutscene_Pamela, 4, 5, -1, new KlonoaVector16(0x70000 >> 12, (0x57b000 >> 12) + 1500, 0xf30000 >> 12));

            // Ghadius (on floor)
            LoadCutsceneObject_Ghadius(2, 3, -1, -1, new KlonoaVector16(-0xf0000 >> 12, -0x8000 >> 12, 0x11a000 >> 12), null, null);
        }

        protected virtual void LoadCutsceneObjects_23_0()
        {
            // Pamela
            LoadCutsceneObject_Karal_Pamela(GlobalGameObjectType.Cutscene_Pamela, 0, 1, -1, new KlonoaVector16(0x70000 >> 12, 0x57b000 >> 12, 0xf30000 >> 12));

            // Beam source
            AddGameObject(GlobalGameObjectType.Cutscene_BeamSource, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(2),
                    },
                };

                obj.Position = new KlonoaVector16((0x70000 >> 12) + 250, 0x57b000 >> 12, 0xf30000 >> 12); // Custom
            });
        }

        #endregion

        #region Bosses

        protected virtual void LoadBossObject_Sprites(int index)
        {
            // Sprites
            AddGameObject(GlobalGameObjectType.Cutscene_Sprites, obj =>
            {
                var sprites = LoadBossAsset<ArchiveFile<Sprites_ArchiveFile>>(index);

                obj.Sprites = sprites.Files.Select(x => new GameObjectData_Sprites()
                {
                    Sprites = x.Files
                }).ToArray();
            });
        }

        protected virtual void LoadBossObjects_8_0()
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
                        ModelBoneAnimations = new GameObjectData_ModelBoneAnimations()
                        {
                            Animations = Enumerable.Range(0, anim.Rotations.Length).Select(x => new GameObjectData_ModelBoneAnimation
                            {
                                BoneRotations = anim.Rotations[x],
                                BonePositions = anim.Positions[x],
                            }).ToArray()
                        }
                    },
                };

                // TODO: Files 3 and 5 have palettes
            });

            // Sprites
            LoadBossObject_Sprites(2);
        }

        protected virtual void LoadBossObjects_11_0()
        {
            // Gelg Bolm
            AddGameObject(GlobalGameObjectType.Boss_GelgBolm, obj =>
            {
                var anim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(12, x => x.Pre_ModelsCount = 4);

                obj.Models = new GameObjectData_Model[]
                {
                    // Legs
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(8, x => x.Pre_HasBones = true), // 10 bones

                        // Note: This animation includes the other models as well
                        ModelBoneAnimations = GameObjectData_ModelBoneAnimations.FromCommonBossModelBoneAnimation(anim)
                    },

                    // Body inside
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(9),
                    },

                    // Shell 1
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(10),
                    },

                    // Shell 2
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(11),
                    },
                };

                //obj.Position = new KlonoaVector16(0x500000 >> 12, 0x300000 >> 12, 0x200000 >> 12);
                obj.Position = new KlonoaVector16(0, -50, 0); // Custom

                // What is the below data for? Appears unused?
                obj.TIM = LoadBossAsset<PS1_TIM>(5);
                obj.TIMArchive = LoadBossAsset<TIM_ArchiveFile>(13);

                if (LoadVRAMData)
                {
                    Loader.AddToVRAM(obj.TIM);

                    foreach (var t in obj.TIMArchive.Files)
                        Loader.AddToVRAM(t);
                }
            });

            // Attack ball
            AddGameObject(GlobalGameObjectType.Boss_GelgBolmAttack, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(14),
                    },
                };
                obj.Position = new KlonoaVector16(1280, -768, 640); // Custom position so it's out of the way
            });

            // TODO: Files 15, 18, 19 has vertex anim data belonging to the TMDs in files 9, 10, 11
            // 0-3: Pairs of vertices/normals
            // 4-> ??
            // 10-> ??
            LoadBossAsset<RawData_ArchiveFile>(15);
            LoadBossAsset<RawData_ArchiveFile>(18);
            LoadBossAsset<RawData_ArchiveFile>(19);
            
            // Background leafs
            var leaves = LoadBossAsset<GelgBolmBossLeaves_ArchiveFile>(16);
            var leafPositions = new KlonoaVector16[]
            {
                new KlonoaVector16(-1088, 768, 2016),
                new KlonoaVector16(-179, 1216, 1344),
                new KlonoaVector16(441, 723, 2093),
                new KlonoaVector16(1440, 1094, 1798),
            };

            for (int i = 0; i < 4; i++)
            {
                AddGameObject(GlobalGameObjectType.Boss_GelgBolmLeaf, obj =>
                {
                    obj.Models = new GameObjectData_Model[]
                    {
                        new GameObjectData_Model()
                        {
                            TMD = leaves.Models[i]
                        },
                    };
                    obj.Position = leafPositions[i];
                    // TODO: Vertex animations
                });
            }

            // TODO: File 17 has what seems to palette data
            LoadBossAsset<RawData_File>(17);

            // TODO: What are the two last models? White rectangles?
            var totemModels = LoadBossAsset<ArchiveFile<PS1_TMD>>(20); // 3+2
            var totemModelPositions = new KlonoaVector16[]
            {
                new KlonoaVector16(0, 0, 0),
                new KlonoaVector16(-90, -2298, 0),
                new KlonoaVector16(83, -2298, 0),
            };

            AddGameObject(GlobalGameObjectType.Boss_GelgBolmTotem, obj =>
            {
                obj.Models = totemModels.Files.Take(3).Select((x, i) => new GameObjectData_Model()
                {
                    TMD = x,
                    Position = totemModelPositions[i]
                }).ToArray();
                obj.Position = new KlonoaVector16(0, 1530, 851);
            });

            // TODO: File 21 has unknown data
            LoadBossAsset<RawData_File>(21);
        }

        protected virtual void LoadBossObjects_14_0()
        {
            // Baladium
            AddGameObject(GlobalGameObjectType.Boss_Baladium, obj =>
            {
                var anim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(9, x => x.Pre_ModelsCount = 1);

                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(2, x => x.Pre_HasBones = true),
                        ModelBoneAnimations = GameObjectData_ModelBoneAnimations.FromCommonBossModelBoneAnimation(anim),
                        Rotation = new KlonoaVector16(0, 0x800, 0) // Custom
                    }
                };

                obj.Position = new KlonoaVector16(0, -0x300000 >> 12, 0xa00000 >> 12);
            });

            // Obstacles
            AddGameObject(GlobalGameObjectType.Boss_BaladiumObstacle, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(6),
                        Position = new KlonoaVector16(0, -500, 1000), // Custom
                    },
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(20),
                        Position = new KlonoaVector16(300, -500, 1000), // Custom
                    },
                };
            });

            // Attack part
            AddGameObject(GlobalGameObjectType.Boss_BaladiumAttackPart, obj =>
            {
                var models = LoadBossAsset<ArchiveFile<PS1_TMD>>(8);

                obj.Models = models.Files.Select(x => new GameObjectData_Model()
                {
                    TMD = x
                }).ToArray();

                obj.Position = new KlonoaVector16(600, -500, 1000); // Custom
            });

            // TODO: Palettes
            LoadBossAsset<RawData_ArchiveFile>(15); // Palettes

            // Sprites
            LoadBossObject_Sprites(17);

            // TODO: Palettes
            LoadBossAsset<RawData_File>(18); // Palettes

            // TODO: Palette (not available in July proto)
            LoadBossAsset<RawData_File>(23); // Palette
        }

        protected virtual void LoadBossObjects_17_0()
        {
            // Joka
            AddGameObject(GlobalGameObjectType.Boss_Joka, obj =>
            {
                var anim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(1, x =>
                {
                    x.Pre_ModelsCount = 3;
                    x.Pre_DoModelPositionsComeFirst = true;
                    x.Pre_DoesPositionsFileHaveHeader = true;
                });

                obj.Models = new GameObjectData_Model[]
                {
                    // Body
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(0, x => x.Pre_HasBones = true),
                        ModelBoneAnimations = GameObjectData_ModelBoneAnimations.FromCommonBossModelBoneAnimation(anim),
                    },

                    // Hands
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(2),
                    },
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(3),
                    },
                };
            });

            // Joka creature form
            AddGameObject(GlobalGameObjectType.Boss_JokaCreature, obj =>
            {
                var anim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(5, x =>
                {
                    x.Pre_ModelsCount = 3;
                    x.Pre_DoModelPositionsComeFirst = true;
                    x.Pre_DoesPositionsFileHaveHeader = true;
                });

                obj.Models = new GameObjectData_Model[]
                {
                    // Body
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(4, x => x.Pre_HasBones = true),
                        ModelBoneAnimations = GameObjectData_ModelBoneAnimations.FromCommonBossModelBoneAnimation(anim),
                    },

                    // Claws
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(6),
                    },
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(7),
                    },
                };

                obj.Position = new KlonoaVector16(3000, 0, 0); // Custom
            });

            // Joka transformation
            AddGameObject(GlobalGameObjectType.Boss_JokaTransformation, obj =>
            {
                // TODO: Model has 2 objects. Other files in archive are vertex animations. Defines vertices and normals for both objects?
                var archive = LoadBossAsset<RawData_ArchiveFile>(8);

                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = archive.SerializeFile<PS1_TMD>(Deserializer, default, 0),
                    },
                };

                obj.Position = new KlonoaVector16(4500, 0, 0); // Custom
            });

            // Level geometry
            AddGameObject(GlobalGameObjectType.Boss_Geometry, obj =>
            { 
                obj.Models = new GameObjectData_Model[]
                {
                    // Claws
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(9),
                    },
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(10),
                    },
                };
            });

            // TODO: 11 and 12 have TIM files with palettes in July prototype

            // TODO: File 13 has pal
            // TODO: File 14 has pal?
            // TODO: File 15 has pal? Unused?

            // TODO: July prototype has a TIM file here instead
            if (Loader.GameVersion != KlonoaGameVersion.DTP_Prototype_19970717)
                LoadBossObject_Sprites(16);
        }

        protected virtual void LoadBossObjects_20_0()
        {
            // Note: These do not really get used here, but to allow them to be viewed in Ray1Map it's easier to load them here as well
            // Sprites
            LoadBossObject_Sprites(6);
        }

        protected virtual void LoadBossObjects_20_1()
        {
            var mainModels = LoadBossAsset<ArchiveFile<PS1_TMD>>(0);

            // Ghadius head and hands
            AddGameObject(GlobalGameObjectType.Boss_Ghadius, obj =>
            {
                var anim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(5, x =>
                {
                    x.Pre_ModelsCount = 3;
                });

                obj.Models = new GameObjectData_Model[]
                {
                    // Head
                    new GameObjectData_Model()
                    {
                        TMD = mainModels.Files[0],
                        ModelBoneAnimations = GameObjectData_ModelBoneAnimations.FromCommonBossModelBoneAnimation(anim)
                    },

                    // Hands
                    new GameObjectData_Model()
                    {
                        TMD = mainModels.Files[1]
                    },
                    new GameObjectData_Model()
                    {
                        TMD = mainModels.Files[2]
                    },
                };

                obj.Position = new KlonoaVector16(0, 0, 2500); // Custom
                obj.Rotation = new KlonoaVector16(0, 0x800, 0); // Custom
            });

            // Ghadius other parts
            AddGameObject(GlobalGameObjectType.Boss_GhadiusParts, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    // Red head
                    new GameObjectData_Model()
                    {
                        TMD = mainModels.Files[3],
                        Position = new KlonoaVector16(2000, 0, 2500), // Custom
                        Rotation = new KlonoaVector16(0, 0x800, 0), // Custom
                    },

                    // Back of heads
                    new GameObjectData_Model()
                    {
                        TMD = mainModels.Files[4],
                        Position = new KlonoaVector16(3000, 0, 2500), // Custom
                        Rotation = new KlonoaVector16(0, 0x800, 0), // Custom
                    },
                    new GameObjectData_Model()
                    {
                        TMD = mainModels.Files[5],
                        Position = new KlonoaVector16(4000, 0, 2500), // Custom
                        Rotation = new KlonoaVector16(0, 0x800, 0), // Custom
                    },
                };
            });

            // Ghadius body
            AddGameObject(GlobalGameObjectType.Boss_GhadiusParts, obj =>
            {
                // TODO: Other files in archive are vertex animations
                var archive = LoadBossAsset<RawData_ArchiveFile>(1);

                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = archive.SerializeFile<PS1_TMD>(Deserializer, default, 0),
                    },
                };

                obj.Position = new KlonoaVector16(0, 0, 2500); // Custom
                obj.Rotation = new KlonoaVector16(0, 0x800, 0); // Custom
            });

            // Attack part
            AddGameObject(GlobalGameObjectType.Boss_GhadiusAttack, obj =>
            {
                var tmds = LoadBossAsset<ArchiveFile<PS1_TMD>>(3);

                obj.Models = tmds.Files.Select(x => new GameObjectData_Model()
                {
                    TMD = x,

                    Position = new KlonoaVector16(5000, 0, 2500), // Custom
                    Rotation = new KlonoaVector16(0, 0x800, 0), // Custom
                }).ToArray();
            });

            // Sprites
            LoadBossObject_Sprites(6);

            var tmds7 = LoadBossAsset<ArchiveFile<PS1_TMD>>(7);

            AddGameObject(GlobalGameObjectType.Boss_GhadiusAttack, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = tmds7.Files[0],

                        Position = new KlonoaVector16(6000, 0, 2500), // Custom
                        Rotation = new KlonoaVector16(0, 0x800, 0), // Custom
                    }
                };
            });

            AddGameObject(GlobalGameObjectType.Boss_GhadiusAttack, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = tmds7.Files[1],

                        Position = new KlonoaVector16(6500, 0, 2500), // Custom
                        Rotation = new KlonoaVector16(0, 0x800, 0), // Custom
                    }
                };
            });

            // TODO: File 9 has palettes
            LoadBossAsset<RawData_File>(9);

            // TODO: File 11 has TIM
            LoadBossAsset<PS1_TIM>(11);

            // TODO: File 12 has TIM
            LoadBossAsset<PS1_TIM>(12);

            // TODO: File 13 has texture data
            LoadBossAsset<RawData_File>(13);

            // TODO: File 14 has palettes (doesn't exist in July proto)
            LoadBossAsset<RawData_File>(14);
        }

        protected virtual void LoadBossObjects_23_0()
        {
            bool isJulyProto = Loader.GameVersion == KlonoaGameVersion.DTP_Prototype_19970717;
            IStreamEncoder encoder = !isJulyProto ? new LevelSectorEncoder() : null;

            // Nahatomb (boss)
            AddGameObject(GlobalGameObjectType.Boss_Nahatomb, obj =>
            {
                var anim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(2, x =>
                {
                    x.Pre_ModelsCount = 1;
                    x.Pre_DoModelPositionsComeFirst = true;
                    x.Pre_DoesPositionsFileHaveHeader = true;
                    x.Pre_HasInitialPositions = true;
                    x.Pre_HasInitialRotations = true;
                }, encoder: encoder);

                var tmd = LoadBossAsset<PS1_TMD>(0, x =>
                {
                    x.Pre_HasColorTable = true;
                    x.Pre_HasBones = true;
                    x.Pre_HasBonePositions = true;
                }, encoder: encoder);

                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = tmd,
                        IsMissingImageData = isJulyProto,
                        ModelBoneAnimations = GameObjectData_ModelBoneAnimations.FromCommonBossModelBoneAnimation(anim),
                    }
                };

                // TODO: Morph targets
                LoadBossAsset<ArchiveFile<DVF_File>>(1, encoder: encoder); // DVF
            });

            // Nahatomb (after beating boss)
            AddGameObject(GlobalGameObjectType.Boss_Nahatomb, obj =>
            {
                var anim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(5, x =>
                {
                    x.Pre_ModelsCount = 1;
                    x.Pre_DoModelPositionsComeFirst = true;
                    x.Pre_DoesPositionsFileHaveHeader = true;
                    x.Pre_HasInitialPositions = true;
                    x.Pre_HasInitialRotations = true;
                }, encoder: encoder);

                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(3, encoder: encoder),
                        IsMissingImageData = isJulyProto,
                        ModelBoneAnimations = GameObjectData_ModelBoneAnimations.FromCommonBossModelBoneAnimation(anim),
                    }
                };

                // TODO: Vertex animation
                LoadBossAsset<RawData_ArchiveFile>(4, encoder: encoder); // Vertex animation

                obj.Position = new KlonoaVector16(3000, 0, 0); // Custom
            });

            // Tongue
            AddGameObject(GlobalGameObjectType.Boss_NahatombTongue, obj =>
            {
                var anim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(7, x =>
                {
                    x.Pre_ModelsCount = 1;
                    x.Pre_DoModelPositionsComeFirst = true;
                    x.Pre_DoesPositionsFileHaveHeader = true;
                    x.Pre_HasInitialPositions = true;
                    x.Pre_HasInitialRotations = true;
                }, encoder: encoder);

                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(6, x => x.Pre_HasBones = true, encoder: encoder),
                        IsMissingImageData = isJulyProto,
                        ModelBoneAnimations = GameObjectData_ModelBoneAnimations.FromCommonBossModelBoneAnimation(anim),
                    }
                };

                obj.Position = new KlonoaVector16(-3000, 0, 0); // Custom
            });

            // Attack
            AddGameObject(GlobalGameObjectType.Boss_NahatombAttack, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(19, encoder: encoder),
                    }
                };
                 
                obj.Position = new KlonoaVector16(0, 0, -3000); // Custom
            });

            // TODO: Vertex animations
            var canonArchive = LoadBossAsset<NahatombBossCanons_ArchiveFile>(16);

            for (int i = 0; i < 5; i++)
            {
                AddGameObject(GlobalGameObjectType.Boss_NahatombCanon, obj =>
                {
                    obj.Models = new GameObjectData_Model[]
                    {
                        new GameObjectData_Model()
                        {
                            TMD = canonArchive.CanonBase,
                        },
                        new GameObjectData_Model()
                        {
                            TMD = canonArchive.Canons[i],
                        },
                    };

                    obj.Position = new KlonoaVector16((short)(500 + (500 * i)), 0, -3000); // Custom
                });
            }

            LoadBossObject_Sprites(15);

            if (isJulyProto)
            {
                // Nahatomb first form without bones
                AddGameObject(GlobalGameObjectType.Boss_Nahatomb, obj =>
                {
                    obj.Models = new GameObjectData_Model[]
                    {
                        new GameObjectData_Model()
                        {
                            TMD = LoadBossAsset<PS1_TMD>(22),
                            IsMissingImageData = true,
                        },
                    };

                    obj.Position = new KlonoaVector16(6000, 0, 0); // Custom

                    // TODO: 23, 24, 25 have vertex data
                });

                // Nahatomb (seems half broken?)
                AddGameObject(GlobalGameObjectType.Boss_Nahatomb, obj =>
                {
                    obj.Models = new GameObjectData_Model[]
                    {
                        new GameObjectData_Model()
                        {
                            TMD = LoadBossAsset<PS1_TMD>(26, x =>
                            {
                                x.Pre_HasColorTable = true;
                                x.Pre_HasBones = true;
                                x.Pre_HasBonePositions = true;
                            }),
                            IsMissingImageData = true,
                        },
                    };

                    obj.Position = new KlonoaVector16(9000, 0, 0); // Custom

                    // TODO: Morph targets
                    LoadBossAsset<ArchiveFile<DVF_File>>(27); // DVF
                });
            }
            else
            {
                // TODO: TIM files (not an animation, they reference different VRAM locations)
                LoadBossAsset<TIM_ArchiveFile>(22);
            }
        }

        protected virtual void LoadBossObjects_23_1()
        {
            bool isJulyProto = Loader.GameVersion == KlonoaGameVersion.DTP_Prototype_19970717;
            IStreamEncoder encoder = !isJulyProto ? new LevelSectorEncoder() : null;

            // Face
            AddGameObject(GlobalGameObjectType.Boss_NahatombFace, obj =>
            {
                var anim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(11, x =>
                {
                    x.Pre_ModelsCount = 1;
                    x.Pre_DoModelPositionsComeFirst = true;
                    x.Pre_HasInitialPositions = true;
                    x.Pre_HasInitialRotations = true;
                }, encoder: encoder);

                var boneAnim = GameObjectData_ModelBoneAnimations.FromCommonBossModelBoneAnimation(anim);

                // TODO: Would be better to only set the bone animation once for the entire object
                obj.Models = new GameObjectData_Model[]
                {
                    // Eyes
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(8, encoder: encoder),
                        IsMissingImageData = isJulyProto,
                        ModelBoneAnimations = boneAnim,
                    },
                    // Mouth
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(9, encoder: encoder),
                        IsMissingImageData = isJulyProto,
                        ModelBoneAnimations = boneAnim,
                    },
                };

                obj.Position = new KlonoaVector16(0, 0, 500); // Custom

                // TODO: Vertex animation
                LoadBossAsset<RawData_ArchiveFile>(10, encoder: encoder); // Vertex animation
            });

            if (isJulyProto)
            {
                var tmds = LoadBossAsset<ArchiveFile<PS1_TMD>>(17);

                // Colored gems
                for (var i = 0; i < tmds.Files.Length; i++)
                {
                    AddGameObject(GlobalGameObjectType.Boss_NahatombGem, obj =>
                    {
                        obj.Models = new GameObjectData_Model[]
                        {
                            new GameObjectData_Model()
                            {
                                TMD = tmds.Files[i],
                            }
                        };

                        obj.Position = new KlonoaVector16((short)(-750+ 300 * i), 0, -500); // Custom
                    });
                }
            }
            else
            {
                // Gem
                AddGameObject(GlobalGameObjectType.Boss_NahatombGem, obj =>
                {
                    obj.Models = new GameObjectData_Model[]
                    {
                        new GameObjectData_Model()
                        {
                            TMD = LoadBossAsset<PS1_TMD>(17, encoder: encoder),
                        }
                    };

                    obj.Position = new KlonoaVector16(0, 0, -500); // Custom
                });
            }
        }

        protected virtual void LoadBossObjects_23_2()
        {
            bool isJulyProto = Loader.GameVersion == KlonoaGameVersion.DTP_Prototype_19970717;
            IStreamEncoder encoder = !isJulyProto ? new LevelSectorEncoder() : null;

            // Nahatomb (boss)
            AddGameObject(GlobalGameObjectType.Boss_Nahatomb, obj =>
            {
                var anim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(14, x =>
                {
                    x.Pre_ModelsCount = 1;
                    x.Pre_DoModelPositionsComeFirst = true;
                    x.Pre_DoesPositionsFileHaveHeader = true;
                    x.Pre_HasInitialPositions = true;
                    x.Pre_HasInitialRotations = true;
                }, encoder: encoder);

                var tmd = LoadBossAsset<PS1_TMD>(12, x =>
                {
                    x.Pre_HasColorTable = true;
                    x.Pre_HasBones = true;
                    x.Pre_HasBonePositions = true;
                }, encoder: encoder);

                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = tmd,
                        IsMissingImageData = isJulyProto,
                        ModelBoneAnimations = GameObjectData_ModelBoneAnimations.FromCommonBossModelBoneAnimation(anim),
                    }
                };

                // TODO: Morph targets
                LoadBossAsset<ArchiveFile<DVF_File>>(13, encoder: encoder); // DVF
            });

            if (isJulyProto)
            {
                var tmds = LoadBossAsset<ArchiveFile<PS1_TMD>>(18);

                // Duplicates of the platforms loaded below, so ignore for now
                // Platforms
                //foreach (PS1_TMD tmd in tmds.Files)
                //{
                //    AddGameObject(GlobalGameObjectType.Boss_NahatombPlatform, obj =>
                //    {
                //        obj.Models = new GameObjectData_Model[]
                //        {
                //            new GameObjectData_Model()
                //            {
                //                TMD = tmd,
                //            }
                //        };
                //    });
                //}
            }
            else
            {
                // Green platform
                AddGameObject(GlobalGameObjectType.Boss_NahatombPlatform, obj =>
                {
                    obj.Models = new GameObjectData_Model[]
                    {
                        new GameObjectData_Model()
                        {
                            TMD = LoadBossAsset<PS1_TMD>(18, encoder: new LevelSectorEncoder()),
                        }
                    };

                    obj.Position = new KlonoaVector16(5000, 0, 0); // Custom
                });
            }

            // 8 * 4
            PS1_TMD[] platformTMDPieces = LoadBossAsset<ArchiveFile<PS1_TMD>>(20, encoder: encoder).Files;

            var platformAnim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(21, x =>
            {
                x.Pre_ModelsCount = 9;
                x.Pre_DoModelPositionsComeFirst = true;
                x.Pre_HasInitialPositions = false;
            }, encoder: encoder);

            for (int i = 0; i < 8; i++)
            {
                // Platform
                AddGameObject(GlobalGameObjectType.Boss_NahatombPlatform, obj =>
                {
                    KlonoaVector16[][] positions = platformAnim.ModelPositions[i].Vectors;
                    VectorAnimationKeyFrames_File rotations = platformAnim.Rotations[i];

                    obj.Models = platformTMDPieces.Skip(8 * (i % 4)).Take(8).Select((x, pieceIndex) => new GameObjectData_Model()
                    {
                        TMD = x,
                        Position = positions[0][1 + pieceIndex],
                        Rotation = new KlonoaVector16(
                            (short)rotations.GetValues((1 + pieceIndex) * 3 + 0)[0],
                            (short)rotations.GetValues((1 + pieceIndex) * 3 + 1)[0],
                            (short)rotations.GetValues((1 + pieceIndex) * 3 + 2)[0])
                    }).ToArray();

                    obj.Position = positions[0][0];
                    obj.Rotation = new KlonoaVector16(
                        (short)rotations.GetValues(0 * 3 + 0)[0],
                        (short)rotations.GetValues(0 * 3 + 1)[0],
                        (short)rotations.GetValues(0 * 3 + 2)[0]);
                });
            }
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
                    LoadBossObjects_11_0();
                    break;

                case 13 when LevelSector is 7:
                    LoadCutsceneObjects_13_7();
                    break;

                // TODO: 13 2-4 have VRAM textures and palette for end transition

                case 14 when LevelSector is 0:
                    LoadCutsceneObjects_14_0();
                    LoadBossObjects_14_0();
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
                    LoadBossObjects_17_0();
                    break;

                case 18 when LevelSector is 0:
                    LoadCutsceneObjects_18_0();
                    break;

                // TODO: 19 0 has VRAM textures for end transition

                case 20 when LevelSector is 0:
                    LoadBossObjects_20_0();
                    break;

                case 20 when LevelSector is 1:
                    LoadBossObjects_20_1();
                    break;

                case 21 when LevelSector is 0:
                    LoadCutsceneObjects_21_0();
                    break;

                case 21 when LevelSector is 1:
                    LoadCutsceneObjects_21_1();
                    break;

                // TODO: 21 1-11 have VRAM textures and palette for end transition

                case 22 when LevelSector is 0:
                    LoadCutsceneObjects_22_0();
                    break;

                // TODO: 22 6 has VRAM textures for end transition

                case 23 when LevelSector is 0:
                    LoadCutsceneObjects_23_0();
                    LoadBossObjects_23_0();
                    break;

                case 23 when LevelSector is 1:
                    LoadBossObjects_23_1();
                    break;

                case 23 when LevelSector is 2:
                    LoadBossObjects_23_2();
                    break;
            }
        }

        #endregion
    }
}