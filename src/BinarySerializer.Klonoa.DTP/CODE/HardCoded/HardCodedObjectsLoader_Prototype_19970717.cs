using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class HardCodedObjectsLoader_Prototype_19970717 : HardCodedObjectsLoader
    {
        #region Constructor

        public HardCodedObjectsLoader_Prototype_19970717(Loader loader, bool loadVramData = true) : base(loader, loadVramData) { }

        #endregion

        #region Cutscenes

        protected void LoadCutscene_Intro(bool isMissingImageData)
        {
            // Window object
            AddGameObject(GlobalGameObjectType.Cutscene_Window, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(0),
                        IsMissingImageData = isMissingImageData,
                        Position = new KlonoaVector16(-0x10b0, -0x440, 0xdc0),
                        Rotation = new KlonoaVector16(0, 2892, 0),
                    },
                };
            });

            // Camera animation
            AddGameObject(GlobalGameObjectType.Cutscene_Camera, obj =>
            {
                obj.CameraAnimations = LevelPack.CutscenePack.Cutscenes[0].Proto_CameraAnimations;
            });

            // Sprites movement paths
            AddGameObject(GlobalGameObjectType.Cutscene_Paths, obj =>
            {
                obj.MovementPathsArchive = LevelPack.CutscenePack.Cutscenes[0].Proto_MovementPaths;
            });
        }

        protected override void LoadCutsceneObjects_3_0()
        {
            LoadCutscene_Intro(false);
        }

        protected override void LoadCutsceneObjects_3_1()
        {
            // Camera animation
            AddGameObject(GlobalGameObjectType.Cutscene_Camera, obj =>
            {
                obj.CameraAnimations = LevelPack.CutscenePack.Cutscenes[2].Proto_CameraAnimations;
            });

            // Sprites movement paths
            AddGameObject(GlobalGameObjectType.Cutscene_Paths, obj =>
            {
                obj.MovementPathsArchive = LevelPack.CutscenePack.Cutscenes[2].Proto_MovementPaths;
            });
        }

        protected virtual void LoadCutsceneObjects_9_0()
        {
            // Karal (unused - custom position)
            LoadCutsceneObject_Karal_Pamela(GlobalGameObjectType.Cutscene_Karal, 0, 1, -1, new KlonoaVector16(-4627, 793, -7347));
        }

        protected override void LoadCutsceneObjects_9_3()
        {
            // Moving platform
            AddGameObject(GlobalGameObjectType.Cutscene_MovingPlatform, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadCutsceneAsset<PS1_TMD>(2),
                    }
                };

                obj.Position = new KlonoaVector16(-6910, -1458, 462); // Defined in script
            });
        }

        protected override void LoadCutsceneObjects_13_7()
        {
            // NOTE: Positions aren't really accurate
            LoadCutsceneObject_Airplane(0, 1, new KlonoaVector16(688, -2304, 4096));
            LoadCutsceneObject_Airplane(0, 1, new KlonoaVector16(688, -1500, 4096));
        }

        protected override void LoadCutsceneObjects_21_0()
        {
            // NOTE: Cutscene 2 also has cam anims and paths, this time a copy from the second cutscene in the first level

            // The cutscene data is a copy from the intro, so load that
            LoadCutscene_Intro(true);
        }

        #endregion

        #region Bosses

        protected override void LoadBossObjects_14_0()
        {
            PS1_TIM tim = LoadBossAsset<PS1_TIM>(10);

            if (LoadVRAMData)
                Loader.AddToVRAM(tim);

            TIM_ArchiveFile timArchive = LoadBossAsset<TIM_ArchiveFile>(11);

            if (LoadVRAMData)
                foreach (PS1_TIM t in timArchive.Files)
                    Loader.AddToVRAM(t);

            timArchive = LoadBossAsset<TIM_ArchiveFile>(13);

            if (LoadVRAMData)
                // Note: The last one should not be loaded
                for (var i = 0; i < 3; i++)
                {
                    PS1_TIM t = timArchive.Files[i];
                    Loader.AddToVRAM(t);
                }

            timArchive = LoadBossAsset<TIM_ArchiveFile>(16);

            if (LoadVRAMData)
                foreach (PS1_TIM t in timArchive.Files)
                    Loader.AddToVRAM(t);

            // Some blue box - leftover debug obj
            AddGameObject(GlobalGameObjectType.Boss_BaladiumAttackPart, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(0),
                    }
                };

                obj.Position = new KlonoaVector16(900, -500, 1000); // Custom
            });

            // A row of blue boxes - leftover debug obj
            AddGameObject(GlobalGameObjectType.Boss_BaladiumAttackPart, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(1),
                    }
                };

                obj.Position = new KlonoaVector16(0, -250, 1000); // Custom
            });

            var ghosts = LoadBossAsset<BaladiumGhost_ArchiveFile>(12);

            // Ghost with animations
            AddGameObject(GlobalGameObjectType.Boss_BaladiumGhost, obj =>
            {
                var anim = LoadBossAsset<CommonBossModelBoneAnimation_ArchiveFile>(14, x => x.Pre_ModelsCount = 1);

                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = ghosts.Models[0],
                        ModelBoneAnimations = GameObjectData_ModelBoneAnimations.FromCommonBossModelBoneAnimation(anim),
                    }
                };

                obj.Position = new KlonoaVector16(0, -1500, 1000); // Custom
            });

            // Ghost attack parts
            AddGameObject(GlobalGameObjectType.Boss_BaladiumAttackPart, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = ghosts.Models[1],
                    }
                };

                obj.Position = new KlonoaVector16(-750, -1500, 1000); // Custom
            });
            AddGameObject(GlobalGameObjectType.Boss_BaladiumAttackPart, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = ghosts.Models[2],
                    }
                };

                obj.Position = new KlonoaVector16(750, -1500, 1000); // Custom
            });

            // Small ghost
            AddGameObject(GlobalGameObjectType.Boss_BaladiumAttackPart, obj =>
            {
                obj.Models = new GameObjectData_Model[]
                {
                    new GameObjectData_Model()
                    {
                        TMD = LoadBossAsset<PS1_TMD>(19),
                    }
                };

                obj.Position = new KlonoaVector16(0, -2000, 1000); // Custom
            });

            // Some attack models
            var tmds = LoadBossAsset<ArchiveFile<PS1_TMD>>(21);

            for (var i = 0; i < tmds.Files.Length; i++)
            {
                AddGameObject(GlobalGameObjectType.Boss_BaladiumAttackPart, obj =>
                {
                    obj.Models = new GameObjectData_Model[]
                    {
                        new GameObjectData_Model()
                        {
                            TMD = tmds.Files[i],
                        }
                    };

                    obj.Position = new KlonoaVector16((short)(200 + (200 * i)), -2000, 1000); // Custom
                });
            }

            // Load base
            base.LoadBossObjects_14_0();
        }

        protected override void LoadBossObjects_20_1()
        {
            TIM_ArchiveFile timArchive = LoadBossAsset<TIM_ArchiveFile>(2);

            if (LoadVRAMData)
                foreach (PS1_TIM t in timArchive.Files)
                    Loader.AddToVRAM(t);

            timArchive = LoadBossAsset<TIM_ArchiveFile>(4);

            if (LoadVRAMData)
                foreach (PS1_TIM t in timArchive.Files)
                    Loader.AddToVRAM(t);

            timArchive = LoadBossAsset<TIM_ArchiveFile>(8);

            if (LoadVRAMData)
                foreach (PS1_TIM t in timArchive.Files)
                    Loader.AddToVRAM(t);

            PS1_TIM tim = LoadBossAsset<PS1_TIM>(10);

            if (LoadVRAMData)
                Loader.AddToVRAM(tim);

            // Load base
            base.LoadBossObjects_20_1();
        }

        #endregion

        #region Public Methods

        public override void LoadObjects()
        {
            switch (BinBlock)
            {
                case 9 when LevelSector is 0:
                    LoadCutsceneObjects_9_0();
                    return;
            }

            base.LoadObjects();
        }

        #endregion
    }
}