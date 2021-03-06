﻿using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace YummyGame.Framework
{
    public class SceneManager:UnitySingleton<SceneManager>
    {
        private bool _isLoading;

#if CHECK_UPDATE_EDITOR || !UNITY_EDITOR
        private Bundle _lastBundle;
#endif
        public AssetLoader Loader { get; private set; }
        public ITask LoadScene(string scenePath)
        {
            if (_isLoading) return null;
            ResetLoader();
#if CHECK_UPDATE_EDITOR || !UNITY_EDITOR
            if (_lastBundle != null)
            {
                _lastBundle.Release();
                _lastBundle = null;
                
                Resources.UnloadUnusedAssets();
                System.GC.Collect();
            }
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            string[] res = ABPathTools.SplitResPath(scenePath, typeof(Scene));
            string abPath = res[0];
            _isLoading = true;
            var task = AssetBundleManager.Instance.LoadBundleAsync(abPath);
            task.OnTaskFinish((_) =>
            {
                _lastBundle = AssetBundleManager.Instance.GetBundle(abPath);
                Loader = new AssetLoader();
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                _isLoading = false;
            });
            return task;
#else
            //scenePath = Utility.PathCombile("Assets",Config.AssetBundleEditorRoot, scenePath)+".unity";
            //EditorSceneManager.LoadSceneInPlayMode(scenePath,new LoadSceneParameters(LoadSceneMode.Single));
            Loader = new AssetLoader();
            return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scenePath).AsyncAsTask();

            //return ImmediateTask.Start();
#endif
        }

        private void ResetLoader()
        {
            if (Loader != null)
            {
                Loader.Destroy();
                Loader = null;
            }
        }
    }
}
