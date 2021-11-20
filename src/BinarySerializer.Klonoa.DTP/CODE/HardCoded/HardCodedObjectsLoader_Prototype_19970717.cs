using BinarySerializer.PS1;

namespace BinarySerializer.Klonoa.DTP
{
    public class HardCodedObjectsLoader_Prototype_19970717 : HardCodedObjectsLoader
    {
        #region Constructor

        public HardCodedObjectsLoader_Prototype_19970717(Loader loader, bool loadVramData = true) : base(loader, loadVramData) { }

        #endregion

        #region Cutscenes

        protected override void LoadCutsceneObjects_3_0()
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
                obj.CameraAnimations = LevelPack.CutscenePack.Cutscenes[0].Proto_CameraAnimations;
            });

            // Sprites movement paths
            AddGameObject(GlobalGameObjectType.Cutscene_Paths, obj =>
            {
                obj.MovementPaths = LevelPack.CutscenePack.Cutscenes[0].Proto_MovementPaths;
            });
        }

        protected override void LoadCutsceneObjects_3_1()
        {
            // Camera animation
            AddGameObject(GlobalGameObjectType.Cutscene_Camera, obj =>
            {
                obj.CameraAnimations = LevelPack.CutscenePack.Cutscenes[2].Proto_CameraAnimations;
            });

            // Seems to be mostly broken data, so ignore
            //// Sprites movement paths
            //AddGameObject(GlobalGameObjectType.Cutscene_Paths, obj =>
            //{
            //    obj.MovementPaths = LevelPack.CutscenePack.Cutscenes[2].Proto_MovementPaths;
            //});
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