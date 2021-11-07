using System;
using System.Collections.Generic;

namespace BinarySerializer.Klonoa.DTP
{
    // Ugly class for loading hard-coded objects and their assets. Primarily for map viewer so those objects can be included.
    // Ideally everything would be loaded based on the object type, spawned from either the cutscene script or other objects.
    public abstract class BaseHardCodedObjectsLoader
    {
        protected BaseHardCodedObjectsLoader(Loader loader, bool loadVramData = true)
        {
            Loader = loader;
            LoadVRAMData = loadVramData;

            GameObjects = new List<GameObject3D>();
        }

        // Properties
        public Loader Loader { get; }
        public LevelPack_ArchiveFile LevelPack => Loader.LevelPack;
        public int BinBlock => Loader.BINBlock;
        public int LevelSector => Loader.LevelSector;
        public bool LoadVRAMData { get; }
        protected Context Context => LevelPack.Context;
        protected BinaryDeserializer Deserializer => Context.Deserializer;

        // Data
        public List<GameObject3D> GameObjects { get; }

        protected T LoadAsset<T>(ArchiveFile pack, int index, Action<T> onPreSerialize = null)
            where T : BinarySerializable, new()
        {
            return pack.SerializeFile<T>(Deserializer, default, index, onPreSerialize: onPreSerialize, name: $"Asset[{index}]");
        }

        protected T LoadCutsceneAsset<T>(int index, Action<T> onPreSerialize = null)
            where T : BinarySerializable, new()
        {
            return LoadAsset<T>(LevelPack.CutscenePack.CutsceneAssets, index, onPreSerialize);
        }

        protected void AddGameObject(GlobalGameObjectType type, Action<GameObject3D> initAction)
        {
            var obj = new GameObject3D()
            {
                PrimaryType = PrimaryObjectType.CutsceneObject,
                GlobalGameObjectType = type,
            };

            initAction(obj);

            GameObjects.Add(obj);
        }

        public abstract void LoadObjects();
    }
}