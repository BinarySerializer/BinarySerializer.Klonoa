using System;
using System.Collections.Generic;

namespace BinarySerializer.Klonoa.DTP
{
    public abstract class BaseHardCodedObjectsLoader
    {
        protected BaseHardCodedObjectsLoader(LevelPack_ArchiveFile levelPack, int binBlock)
        {
            LevelPack = levelPack;
            BinBlock = binBlock;

            CutsceneGameObjects3D = new List<GameObject3D>();
        }

        // Properties
        public LevelPack_ArchiveFile LevelPack { get; }
        public int BinBlock { get; }
        protected Context Context => LevelPack.Context;
        protected BinaryDeserializer Deserializer => Context.Deserializer;

        // Data
        public List<GameObject3D> CutsceneGameObjects3D { get; }

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

        protected void AddCutsceneGameObject3D(Action<GameObject3D> initAction)
        {
            var obj = new GameObject3D()
            {
                PrimaryType = PrimaryObjectType.CutsceneObject
            };

            initAction(obj);

            CutsceneGameObjects3D.Add(obj);
        }

        public abstract void LoadCutscene(int cutsceneIndex);
    }
}