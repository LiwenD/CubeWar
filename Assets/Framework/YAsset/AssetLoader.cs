using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace YummyGame.Framework
{
    public class AssetLoader
    {
        public delegate void AssetLoaderCallback<T>(T asset);
        public delegate void AssetLoaderCallback();
        private Dictionary<string,Asset> m_assets = new Dictionary<string,Asset>();
        public void LoadAssetAsync<T>(string path, AssetLoaderCallback<T> callback)
            where T:Object
        {
            if (m_assets.ContainsKey(path)&&m_assets[path].IsValid)
            {
                callback((T)(m_assets[path].asset));
                return;
            }
            if (m_assets.ContainsKey(path) && !m_assets[path].IsValid)
            {
                m_assets.Remove(path);
            }

            AssetManager.Instance.LoadAsync(path, typeof(T)).OnTaskFinish((_) =>
            {
                if (!m_assets.ContainsKey(path))
                {
                    Asset asset = AssetManager.Instance.GetAsset(path).Get();
                    m_assets.Add(path, asset);
                }
                callback((T)(m_assets[path].asset));
            }).OnTaskError((err)=>
            {
                Debug.LogError(err);
            });
        }

        public T GetAsset<T>(string path)
            where T : Object
        {
            if (m_assets.ContainsKey(path)) { return (T)m_assets[path].asset; }
            return default(T);
        }

        public void LoadAssetsAsync(string[] paths,Type[] types, AssetLoaderCallback callback)
        {
            if (paths.Length != types.Length)
            {
                throw new Exception("调用错误");
            }
            var seq = new TaskList();
            var index = -1;
            foreach (var path in paths)
            {
                index++;
                if (m_assets.ContainsKey(path))
                {
                    continue;
                }
                seq.Join(AssetManager.Instance.LoadAsync(path, types[index]).OnTaskFinish((_) =>
                {
                    if (m_assets.ContainsKey(path)) return;
                    Asset asset = AssetManager.Instance.GetAsset(path).Get();
                    m_assets.Add(path, asset);
                }).OnTaskError((err) =>
                {
                    Debug.LogError(err);
                }));
            }
            if(seq.TaskCount == 0)
            {
                callback();
            }
            else
            {
#if BUNDLE_TEST_MODE && UNITY_EDITOR
                callback();
#else
                seq.OnTaskFinish((_)=>
                {
                    callback();
                }).Start();
#endif
            }
            
        }

        public void Destroy()
        {
            foreach (var asset in m_assets.Values)
            {
                asset.Release();
            }
            m_assets.Clear();
        }
    }

    public class AssetLoaderTask<T> : YummyTask<T>
    {
        public AssetLoaderTask()
        {

        }
        protected override void OnTaskUpdateInternal()
        {
            
        }
    }
}
