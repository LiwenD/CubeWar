using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public abstract class YAssetTask:YummyTask<NullObject>
    {
        AssetManager.AssetInitializeProcess progressCallback;
        AssetManager.AssetErrorCallback errorCallback;
        public YAssetTask(AssetManager.AssetInitializeProcess progressCallback,
            AssetManager.AssetErrorCallback errorCallback,TaskLooper looper = null) : base(looper)
        {
            this.progressCallback = progressCallback;
            this.errorCallback = errorCallback;
        }

        protected void ErrorCallback(string err)
        {
            State = TaskState.Failure;
            error = err;
            errorCallback?.Invoke(err);
        }

        protected void ProgressCallback(string msg,int current,int total)
        {
            progressCallback?.Invoke(msg,(float)current/total);
        }

        protected void ProgressCallback(string msg, int current)
        {
            ProgressCallback(msg, current, 1);
        }
    }
}
