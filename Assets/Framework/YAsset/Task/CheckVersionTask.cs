using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace YummyGame.Framework
{
    public class CheckVersionTask : YAssetTask
    {
        public CheckVersionTask(AssetManager.AssetInitializeProcess progressCallback, AssetManager.AssetErrorCallback errorCallback, TaskLooper looper = null) : base(progressCallback, errorCallback, looper)
        {
        }

        protected override void OnTaskStartInternal()
        {
            ProgressCallback("正在检查客户端版本", 0);
        }

        protected override void OnTaskUpdateInternal()
        {
            if (!DataManager.Instance.HasKey(Config.AssetHistoryVersionsFlag))
            {
                DataManager.Instance.SetString(Config.AssetHistoryVersionsFlag,"");
            }

            VersionList list = VersionList.FromString(DataManager.Instance.GetString(Config.AssetHistoryVersionsFlag));
            foreach (var version in list.m_versions)
            {
                VersionManager.Instance.SetLatestVersion(version);
                VersionManager.Instance.AddVersion(version);
            }
            State = TaskState.Finish;
        }
    }
}
