using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace YummyGame.Framework
{
    public class AssetBundleManager:UnitySingleton<AssetBundleManager>
    {
        private AssetBundleManifest manifest;
        private Dictionary<string, Bundle> m_bundles = new Dictionary<string, Bundle>();
        private Dictionary<string, YummyTask<NullObject>> m_tasks = new Dictionary<string, YummyTask<NullObject>>();
        public bool HasBundle(string path)
        {
            return m_bundles.ContainsKey(path)&&m_bundles[path].IsValid;
        }

        public AssetBundle LoadBundle(string path)
        {
            if (HasBundle(path)) return m_bundles[path].Get().asset as AssetBundle;

            foreach (var p in AssetManager.Instance.searchPaths)
            {
                string searchPath = Utility.PathCombile(p, path);
                if (!File.Exists(searchPath)) continue;
                AssetBundle bundle = AssetBundle.LoadFromFile(searchPath);
                if (bundle == null) continue;
                Bundle b = new Bundle(path, bundle, 0);
                m_bundles.Add(path, b);
                return b.Get().asset as AssetBundle;
            }
            return null;
        }

        public Bundle GetBundle(string path)
        {
            if (m_bundles.ContainsKey(path)&&m_bundles[path].IsValid)
            {
                return m_bundles[path];
            }
            return null;
        }

        public string[] GetAssetBundleDependencies(string abPath)
        {
            if (manifest == null) return null;
            return manifest.GetAllDependencies(abPath);
        }

        public YummyTask<NullObject> LoadBundleAsync(string path)
        {
            if (HasBundle(path)) return ImmediateTask.Start();
            if (m_tasks.ContainsKey(path)) return m_tasks[path];

            var taskList = new TaskList();
            var list = new List<string>();
            _BuildTask(ref taskList, path, ref list);

            taskList.Start();
            return taskList;
        }

        private void _BuildTask(ref TaskList task, string abPath,ref List<string> builded)
        {
            string[] dependences = manifest.GetDirectDependencies(abPath);
            List<string> tmp = new List<string>();
            foreach (var d in dependences)
            {
                if (builded.Contains(d)) continue;
                builded.Add(d);
                tmp.Add(d);
                _BuildTask(ref task, d, ref builded);
            }
            if (m_tasks.ContainsKey(abPath))
            {
                task.Append(m_tasks[abPath]);
            }
            else
            {
                var newTask = AssetBundleTask.LoadBundle(abPath).OnTaskFinish((_) =>
                {
                    if (m_tasks.ContainsKey(abPath))
                        m_tasks.Remove(abPath);
                    Bundle asset = null;
                    if (!m_bundles.ContainsKey(abPath))
                    {
                        asset = new Bundle(abPath, _, 0);
                        m_bundles.Add(abPath, asset);
                        tmp.ForEach((dependence) =>
                        {
                            try
                            {
                                asset.AddDependence(m_bundles[dependence]);
                            }
                            catch
                            {
                                Debug.LogError("exception:" + abPath + "  needs:" + dependence + " " + Time.frameCount);
                            }
                        });
                    }
                    else
                    {
                        asset = m_bundles[abPath];
                        asset.asset = _;
                        asset.bundle = _;
                    }

                });
                m_tasks.Add(abPath, newTask.GetListener());
                task.Append(newTask);
            }
        }

        public bool LoadManifest()
        {
            var assetbundle = LoadBundle(ABPathTools.GetRuntimePlatformName(Application.platform));
            if (assetbundle == null) return false;
            manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            assetbundle.Unload(false);
            return true;
        }

        public void ClearAllBundles()
        {
            foreach (var bundle in m_bundles.Values)
            {
                if (bundle.IsValid) bundle.Release();
            }
            m_bundles.Clear();
        }

        public void DebugReference()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kv in m_bundles)
            {
                sb.AppendLine(kv.Key + ":" + kv.Value.referenceCount);
            }
            Debug.Log(sb.ToString());
        }
    }
}
