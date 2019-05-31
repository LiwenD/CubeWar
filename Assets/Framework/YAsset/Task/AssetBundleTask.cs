using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace YummyGame.Framework
{
    public static class AssetBundleTask
    {
        public static AssetBundleTask_Internal LoadBundle(string path)
        {
            AssetBundleTask_Internal task = new AssetBundleTask_Internal(path);
            //task.Start();
            return task;
        }
        public class AssetBundleTask_Internal : YummyTask<AssetBundle>
        {
            private string absolutePath;
            private string relativePath;
            IEnumerator<string> enumerator;
            AssetBundleCreateRequest request;
            public AssetBundleTask_Internal(string relativePath)
            {
                this.relativePath = relativePath;
                enumerator = AssetManager.Instance.searchPaths.GetEnumerator();
            }
            protected override void OnTaskUpdateInternal()
            {
                if (request == null)
                {
                    if (!enumerator.MoveNext())
                    {
                        State = TaskState.Failure;
                        error = "找不到对应的资源:" + relativePath;
                        return;
                    }
                    try
                    {
                        string searchPath = Utility.PathCombile(enumerator.Current, relativePath);
                        if(File.Exists(searchPath))
                            request = AssetBundle.LoadFromFileAsync(searchPath);
                    }
                    catch { }
                }
                else
                {
                    if (request.isDone)
                    {
                        if (request.assetBundle != null)
                        {
                            State = TaskState.Finish;
                            target = request.assetBundle;
                        }
                        else
                        {
                            request = null;
                        }
                    }
                }

            }
        }
    }
    
}
