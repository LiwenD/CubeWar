using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace YummyGame.Framework
{
    public static class AssetLoadByAssetBundleTask
    {
        public static AssetLoadByAssetBundleTask_Internal LoadAsset(string originalPath,Type type)
        {
            AssetLoadByAssetBundleTask_Internal task = new AssetLoadByAssetBundleTask_Internal(originalPath, type);
            task.OnTaskError((err)=>Debug.LogError(err)).Start();
            return task;
        }

        public class AssetLoadByAssetBundleTask_Internal : YummyTask<Asset>
        {
            private string abPath;
            private string resPath;
            private string originalPath;
            Type resType;
            TaskList seq;

            //ITask task;

            public AssetLoadByAssetBundleTask_Internal(string originalPath,Type type)
            {
                string[] res = ABPathTools.SplitResPath(originalPath, type);
                abPath = res[0];
                resPath = res[1];
                this.resType = type;
                this.originalPath = originalPath;
            }

            protected override void OnTaskStartInternal()
            {
                seq = new TaskList();
                seq.Append(AssetBundleManager.Instance.LoadBundleAsync(abPath));
                seq.Append(new AssetLoadByAssetBundleDirectTask_Internal(abPath, resPath, resType).OnTaskFinish((asset) =>
                {
                    target = new Asset(originalPath, resType, asset);
                    target.AddDependence(AssetBundleManager.Instance.GetBundle(abPath));
                }));
                seq.SetAutoKill(false).Start();
                //Debug.Log(abPath);
                //task = Task.Sequence()
                //    .Wait(AssetBundleManager.Instance.LoadBundleAsync(abPath))
                //    .Wait(new AssetLoadByAssetBundleDirectTask_Internal(abPath, resPath, resType).OnTaskFinish((asset) =>
                //    {
                //        target = new Asset(originalPath, resType, asset);
                //        target.AddDependence(AssetBundleManager.Instance.GetBundle(abPath));
                //    }))
                //    .Start();
            }


            protected override void OnTaskUpdateInternal()
            {
                //State = seq.State;
                //error = seq.error;
                //Debug.Log("1");
                State = seq.GetState();
                error = seq.error;

                if (State == TaskState.Finish)
                {
                    Debug.Log("finish internal");
                }
            }
        }

        private class AssetLoadByAssetBundleDirectTask_Internal : YummyTask<Object>
        {
            private AssetBundleRequest request;
            private string resPath;
            private string abPath;
            private Type type;
            public AssetLoadByAssetBundleDirectTask_Internal(string abPath,string resPath,Type type)
            {
                this.resPath = resPath;
                this.abPath = abPath;
                this.type = type;
            }

            protected override void OnTaskStartInternal()
            {
                AssetBundle bundle = AssetBundleManager.Instance.GetBundle(abPath).bundle;
                request = bundle.LoadAssetAsync(resPath, type);
            }

            protected override void OnTaskUpdateInternal()
            {
                if (request.isDone)
                {
                    if(request.asset == null)
                    {
                        error = "加载资源失败：" + resPath;
                        State = TaskState.Failure;
                    }
                    else
                    {
                        target = request.asset;
                        State = TaskState.Finish;
                    }
                }
            }
        }
    }
}
