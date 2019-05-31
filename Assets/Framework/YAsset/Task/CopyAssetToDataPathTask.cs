using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace YummyGame.Framework
{
    public class CopyAssetToDataPathTask : YummyTask<NullObject>
    {
        Queue<string> files;
        HashSet<string> writed;
        ITaskChain task;
        YummyTask<WWW> fileTask;
        YummyTask<WWW> copyTask;
        AssetManager.AssetInitializeProcess callback;
        FileStream usrDataStream;
        StreamWriter sw;
        bool copying = false;
        int finishCount = 0;
        int totalCount = 0;
        string directoryPath;
        public CopyAssetToDataPathTask(AssetManager.AssetInitializeProcess callback,TaskLooper looper = null) : base(looper)
        {
            this.callback = callback;
            LoadFiles();
            CreateInitialTask();
        }

        private void LoadFiles()
        {
            writed = new HashSet<string>();
            string directoryPath = Utility.PathCombile(Config.AssetRoot,
                            VersionManager.Instance.GetInitialVersion().ToString());
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
            string fileTxt = Utility.PathCombile(directoryPath, "files.txt");
            usrDataStream = new FileStream(fileTxt, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(usrDataStream);
            string str = null;
            while ((str = sr.ReadLine()) != null)
            {
                if (str == "") continue;
                writed.Add(str);
            }
            sr.Close();
            usrDataStream.Close();

            usrDataStream = new FileStream(fileTxt, FileMode.Append, FileAccess.Write);
            sw = new StreamWriter(usrDataStream);
        }

        private void CreateInitialTask()
        {
            string fileTxt = Utility.PathCombile(Application.streamingAssetsPath, "files.txt");
            fileTask = WWWTask.Get(fileTxt)
                .OnTaskFinish((_) =>
                {
                    string[] totalFiles = _.text.Split(new string[] { "\r\n" },StringSplitOptions.None);
                    files = new Queue<string>();
                    foreach (var file in totalFiles)
                    {
                        if (string.IsNullOrEmpty(file)) continue;
                        if (!writed.Contains(file)) files.Enqueue(file);
                    }
                    copying = true;
                    totalCount = totalFiles.Length;
                }).OnTaskError((err) =>
                {
                    fileTask.Reset();
                    fileTask.Start();
                }).OnTaskUpdate(() => { callback?.Invoke("正在检查资源", 0); });
        }

        protected override void OnTaskStartInternal()
        {
            DataManager.Instance.DeleteKey(Config.AssetVersionFlag);
            finishCount = writed.Count + 1;
            directoryPath = Utility.PathCombile(Config.AssetRoot,
                            VersionManager.Instance.GetInitialVersion().ToString());
            if (Directory.Exists(directoryPath)) Directory.Delete(directoryPath);
            fileTask.Start();
        }
        
        protected override void OnTaskUpdateInternal()
        {
            if (copying == false) return;
            if (copyTask != null) return;
            if (files.Count == 0)
            {
                sw.Close();
                usrDataStream.Close();
                DataManager.Instance.SetString(Config.AssetVersionFlag,VersionManager.Instance.GetInitialVersion().ToString());
                State = TaskState.Finish;
            }
            else
            {
                string line = files.Dequeue();
                if (writed.Contains(line)) return;
                string[] fileInfo = line.Split('|');
                if (fileInfo == null || fileInfo.Length <= 1) return;
                string fileName = fileInfo[0];
                string fileMD5 = fileInfo[1];
                
                callback?.Invoke("正在解压资源:"+fileName,(float)finishCount/ totalCount);
                copyTask = WWWTask.Get(Utility.PathCombile(Application.streamingAssetsPath,fileName))
                    .OnTaskFinish((_)=>
                    {  
                        byte[] data = _.bytes;
                        string createPath =
                        Utility.PathCombile(directoryPath, fileName);
                        string createDirectory = Path.GetDirectoryName(createPath);
                        if (!Directory.Exists(createDirectory))
                        {
                            Directory.CreateDirectory(createDirectory);
                        }
                        if (File.Exists(createPath)) File.Delete(createPath);

                        using (FileStream fs = new FileStream(createPath, FileMode.Create, FileAccess.Write))
                        {
                            fs.Write(data, 0, data.Length);
                        }
                        if (Utility.MD5Stream(createPath) != fileMD5)
                        {
                            files.Enqueue(line);
                        }
                        else
                        {
                            sw.WriteLine(line);
                            sw.Flush();
                            writed.Add(line);
                            finishCount++;
                        }
                        copyTask = null;

                    }).OnTaskError((err)=>
                    {
                        Debug.LogError(err);
                        files.Enqueue(line);
                    });
            }
        }
    }
}
