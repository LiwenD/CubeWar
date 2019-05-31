using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

#if XLUA_SUPPORT
using XLua;
#endif

namespace YummyGame.Framework
{
    public class AssetInitializeListener
    {
        public AssetManager.AssetInitializeProcess ProgressInfo;
        public AssetManager.AssetUpdateAssetConfirm ConfirmCallback;
        public AssetManager.AssetErrorCallback ErrorCallback;
        public AssetManager.AssetInitializeComplete InitializeCallback;
    }

    public class AssetManager:UnitySingleton<AssetManager>
    {
        public delegate void AssetLoadComplete(Asset asset);
        public delegate void AssetInitializeProcess(string msg, float progress);
        public delegate void AssetUpdateAssetConfirm(long size, Action confirmCallback,Action cancelCallback);
        public delegate void AssetErrorCallback(string error);
        public delegate void AssetInitializeComplete();

        public List<string> searchPaths { get; private set; }
        private Dictionary<string, Asset> m_assets;
        private Dictionary<string, YummyTask<NullObject>> m_tasks;
        private Version initialVersion;

#if XLUA_SUPPORT
        public LuaEnv lua { get; private set; }
#endif

        private void Awake()
        {
            m_assets = new Dictionary<string, Asset>();
            m_tasks = new Dictionary<string, YummyTask<NullObject>>();
            initialVersion = VersionManager.Instance.GetInitialVersion();

#if XLUA_SUPPORT
            lua = new LuaEnv();
#endif
        }

        private void Update()
        {
#if XLUA_SUPPORT
            lua.Tick();
#endif
        }

        public bool HasAsset(string path)
        {
            return m_assets.ContainsKey(path)&&m_assets[path].IsValid;
        }

        public Asset Load(string path,Type type)
        {
            if (HasAsset(path))
            {
                return m_assets[path];
            }
            if (Config.UseAssetBundle)
            {
                return LoadByAssetBundle(path, type);
            }
            else
            {
                return LoadByResources(path, type);
            }
        }

        public Asset GetAsset(string path)
        {
            if (HasAsset(path))
            {
                return m_assets[path];
            }
            return null;
        }

        private Asset LoadByResources(string path,Type type)
        {
            Object obj = Resources.Load(path, type);
            if (!m_assets.ContainsKey(path))
            {
                Asset asset = new Asset(path, type, obj);
                m_assets.Add(path, asset);
            }
            else
            {
                m_assets[path].asset = obj;
            }
            
            return m_assets[path];
        }

        private Asset LoadByAssetBundle(string path, Type type)
        {
            return null;
        }

        public YummyTask<NullObject> LoadAsync(string path, Type type)
        {
            if (m_assets.ContainsKey(path)&&m_assets[path].IsValid)
            {
                return ImmediateTask.Start();
            }
            if (m_tasks.ContainsKey(path))
            {
                return m_tasks[path];
            }
            if (Config.UseAssetBundle)
            {
#if CHECK_UPDATE_EDITOR || !UNITY_EDITOR
                return LoadAsyncByAssetBundle(path, type);
#else
                return LoadAsyncByEditor(path, type);
#endif
            }
            else
            {
                return LoadAsyncByResourecs(path, type);
            }
        }

        private YummyTask<NullObject> LoadAsyncByResourecs(string path, Type type)
        {
            var task = ResourcesTask.Load(path, type);
            m_tasks.Add(path, task.GetListener());
            task.OnTaskFinish((asset) =>
            {
                m_tasks.Remove(path);
                if (!m_assets.ContainsKey(path))
                    m_assets.Add(path, asset);
                else
                    m_assets[path] = asset;
            });
            return m_tasks[path];
        }
#if UNITY_EDITOR
        private YummyTask<NullObject> LoadAsyncByEditor(string path, Type type)
        {
            var resPath = Utility.PathCombile("Assets", Config.AssetBundleEditorRoot,path) + ABPathTools.GetObjectSuffix(type);
            Object obj = AssetDatabase.LoadAssetAtPath(resPath, type);
            if (!m_assets.ContainsKey(path))
            {
                Asset asset = new Asset(path, type, obj);
                m_assets.Add(path, asset);
            }
            else
            {
                m_assets[path].asset = obj;
            }
            return ImmediateTask.Start();
        }
#endif

        private YummyTask<NullObject> LoadAsyncByAssetBundle(string path, Type type)
        {
            var task = AssetLoadByAssetBundleTask.LoadAsset(path, type);
            m_tasks.Add(path, task.GetListener());
            task.OnTaskFinish((asset) =>
            {
                m_tasks.Remove(path);
                if (!m_assets.ContainsKey(path))
                    m_assets.Add(path, asset);
                else
                    m_assets[path] = asset;
            });
            return m_tasks[path];
        }

        public void GC()
        {
            foreach (var asset in m_assets.Values)
            {
                asset.Release();
            }
            m_assets.Clear();
            AssetBundleManager.Instance.ClearAllBundles();
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        public void Initalize(AssetManager.AssetInitializeComplete success)
        {
            Initalize(new AssetInitializeListener() { InitializeCallback = success });
        }

        public void Initalize(AssetInitializeListener listener)
        {
            if (searchPaths == null)
                searchPaths = new List<string>();
            
            ITaskChain task = Task.Sequence();
            
            task
#if CHECK_UPDATE_EDITOR || !UNITY_EDITOR
                .If(()=> (!DataManager.Instance.HasKey(Config.AssetVersionFlag) ||
                Version.FromString(DataManager.Instance.GetString(Config.AssetVersionFlag)) < initialVersion)
                , new CopyAssetToDataPathTask(listener.ProgressInfo))
                .Event(()=>VersionManager.Instance.AddVersion(initialVersion))
                //todo

                .Wait(new CheckVersionTask(listener.ProgressInfo, listener.ErrorCallback))
                .Wait(new CheckUpdateTask(listener.ProgressInfo,listener.ErrorCallback, listener.ConfirmCallback))
#endif
                .Event(() =>
            {
                VersionManager.Instance.versions.Reverse();
                _InitialSearchPath();
#if CHECK_UPDATE_EDITOR || !UNITY_EDITOR
                AssetBundleManager.Instance.LoadManifest();
#endif
#if XLUA_SUPPORT
                _InitLua();
#endif
            }).Event(()=> { listener.InitializeCallback?.Invoke(); });
            task.Start();
        }

#if XLUA_SUPPORT
        private void _InitLua()
        {
            lua.AddLoader(LuaLoader);

            lua.DoString(@"
            function __G__TRACEBACK__(text)
                local msg = ''
                msg = msg .. '\n----------------------------------------\n'
                    msg = msg .. 'LUA ERROR: ' .. tostring(text) .. '\n'
                    msg = msg .. debug.traceback()
                    msg = msg .. '\n----------------------------------------\n'
                    print(msg)
            end
        ");
        }

        private byte[] LuaLoader(ref string filePath)
        {
#if CHECK_UPDATE_EDITOR || !UNITY_EDITOR
            foreach (var version in VersionManager.Instance.versions)
            {
                string fullPath = Utility.PathCombile(Config.AssetRoot, version.ToString(), Config.LuaSource, filePath)+".bytes";
                if (File.Exists(fullPath))
                {
                    return File.ReadAllBytes(fullPath);
                }
            }
#else
            string fullPath = Utility.PathCombile(Application.dataPath, Config.LuaSource, filePath)+".lua";
            if (File.Exists(fullPath))
            {
                return File.ReadAllBytes(fullPath);
            }
#endif
            return null;
        }
#endif

        private void _InitialSearchPath()
        {
            foreach (var version in VersionManager.Instance.versions)
            {
                searchPaths.Add(Utility.PathCombile(Config.AssetRoot, version.ToString()));
            }
        }

        private void OnDestroy()
        {
#if XLUA_SUPPORT
            lua.Dispose();
#endif
        }

        public void AddSearchPath(string path)
        {
            if (searchPaths == null)
                searchPaths = new List<string>();
            searchPaths.Add(path);
        }

        public string GetVerificyFilePath(Version version)
        {
            return Utility.PathCombile(Config.AssetRoot, version.ToString(), "files.txt");
        }

        public void LoadLuaFile(string path,string func)
        {
#if XLUA_SUPPORT
            lua.DoString(string.Format(@"
                xpcall(function() 
                    local module = require '{0}';
                    module:{1}();
                end
                ,__G__TRACEBACK__);
            ", path,func));
#endif
        }
    }
}
