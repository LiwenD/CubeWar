using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace YummyGame.Framework
{
    public class CheckUpdateTask : YAssetTask
    {
        private Version latestVersion;
        private AssetManager.AssetUpdateAssetConfirm confirmCallback;

        private ITask updateFileTask;
        private ITask updateTask;
        private Version serverVersion;
        private Queue<string> updateList;
        private bool updateing;
        private int finishCount;
        private int totalCount;

        private FileStream verifyFs;
        private StreamWriter sw;
        private HashSet<string> verifies;

        public CheckUpdateTask(AssetManager.AssetInitializeProcess progressCallback, 
            AssetManager.AssetErrorCallback errorCallback,
            AssetManager.AssetUpdateAssetConfirm confirmCallback,
            TaskLooper looper = null) : base(progressCallback, errorCallback, looper)
        {
            this.confirmCallback = confirmCallback;
        }

        protected override void OnKilledInternal()
        {
            killFile();
        }

        private void killFile()
        {
            if (verifyFs != null)
            {
                sw.Close();
                verifyFs.Close();
                sw = null;
                verifyFs = null;
            }
        }

        protected override void OnTaskStartInternal()
        {
            ProgressCallback("正在检查更新", 0);
            updateing = false;
            finishCount = 1;
            updateList = new Queue<string>();
            var task = WWWTask.Get(Config.AssetVersionURI).OnTaskError((err) =>
            {
                ErrorCallback(err);
            }).OnTaskFinish((msg) =>
            {
                if (!Version.IsValidVersionString(msg.text))
                {
                    ErrorCallback("不是一个有效的版本号：" + msg.text);
                    return;
                }
                serverVersion = Version.FromString(msg.text);
                latestVersion = VersionManager.Instance.GetLatestVersion();
                
                
                if (Version.IsFullUpdateVersion(latestVersion, serverVersion))
                {
                    ErrorCallback("暂时不支持整包更新");
                    ProgressCallback("请重新下载安装包", 0);
                    return;
                }
                else if (Version.IsHotUpdateVersion(latestVersion, serverVersion))
                {
                    ReadLocalVerifyFile();
                    FileMD5Task();
                }
                else
                {
                    State = TaskState.Finish;
                }
            });
            task.Start();
        }

        private void ReadLocalVerifyFile()
        {
            verifies = new HashSet<string>();
            string path = AssetManager.Instance.GetVerificyFilePath(serverVersion);
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            verifyFs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader sr = new StreamReader(verifyFs);

            string line;
            while ((line = sr.ReadLine())!=null)
            {
                if (line == "") continue;
                verifies.Add(line);
            }
            verifyFs.Close();
            sr.Close();

            verifyFs = new FileStream(path, FileMode.Append, FileAccess.Write);
            sw = new StreamWriter(verifyFs);
        }

        private void OnUpdateFinish()
        {
            string str = DataManager.Instance.GetString(Config.AssetHistoryVersionsFlag);
            VersionList list = VersionList.FromString(str);
            list.AddVersion(serverVersion);
            DataManager.Instance.SetString(Config.AssetHistoryVersionsFlag, list.ToString());
            killFile();

            State = TaskState.Finish;
        }

        protected override void OnTaskUpdateInternal()
        {
            if (!updateing) return;
            if(updateList.Count == 0)
            {
                VersionManager.Instance.AddVersion(serverVersion);
                OnUpdateFinish();
            }
            else
            {
                if (updateTask == null)
                {
                    string[] line = updateList.Peek().Split('|');
                    string updateURI = Utility.PathCombile(
                        Config.AssetUpdateURI,
                        ABPathTools.GetRuntimePlatformName(Application.platform),
                        latestVersion.ToString(), line[0]);
                    string targetPath = Utility.PathCombile(Config.AssetRoot, serverVersion.ToString(), line[0]);
                    string targetDir = Path.GetDirectoryName(targetPath);
                    if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);
                    updateTask = WWWTask.Get(updateURI).OnTaskFinish((_)=>
                    {
                        using(FileStream fs = new FileStream(targetPath, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            fs.Write(_.bytes, 0, _.bytes.Length);
                        }
                        string md5 = line[1];
                        if(md5 == Utility.MD5Stream(targetPath))
                        {
                            sw.WriteLine(updateList.Peek());
                            sw.Flush();
                            updateList.Dequeue();
                            finishCount++;
                        }
                        updateTask = null;
                    }).OnTaskError((err)=>
                    {
                        Debug.LogError(err);
                        updateTask = null;
                    });
                    ProgressCallback("更新：" + line[0], finishCount, totalCount);
                    Debug.Log("start update:" + updateURI);
                }
            }
        }

        private void FileMD5Task()
        {
            updateFileTask = WWWTask.Get(Utility.PathCombile(
                Config.AssetUpdateURI,
                ABPathTools.GetRuntimePlatformName(Application.platform),
                latestVersion.ToString(),"update.txt"))
                .OnTaskError((err)=>
                {
                    ErrorCallback(err);
                })
                .OnTaskFinish((_)=>
                {
                    string[] totalFiles = _.text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    if(totalFiles.Length == 0)
                    {
                        ErrorCallback("错误的校验文件,Version:" + latestVersion);
                        return;
                    }
                    long length = 0;

                    for (int i = 0; i < totalFiles.Length; i++)
                    {
                        if (string.IsNullOrEmpty(totalFiles[i])) continue;
                        if (verifies.Contains(totalFiles[i])) continue;
                        updateList.Enqueue(totalFiles[i]);
                        length += Convert.ToInt64(totalFiles[i].Split('|')[2]);
                        totalCount++;
                    }
                    
                    confirmCallback?.Invoke(length, UpdateFileTask, () =>
                      {
                          ErrorCallback("更新被取消");
                      });
                });
        }

        private void UpdateFileTask()
        {
            updateing = true;
        }
    }
}
