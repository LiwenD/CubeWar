using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using YummyGame.Framework.Editor;

namespace YummyGame.Framework
{
    public class PatchFileInfo
    {
        public string name;
        public string md5;
        public string size;

        public override string ToString()
        {
            return name + "|" + md5 + "|" + size;
        }
    }
    public class BuildScript
    {
        [MenuItem("Yummy/ClearAllFlag")]
        public static void ClearAllFlag()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Yummy/AssetBundles/BundleMode")]
        public static void SwitchAssetBundle()
        {
            string[] symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Split(';');
            List<string> s = new List<string>();
            bool add = true;
            foreach (var item in symbols)
            {
                if(item == "BUNDLE_TEST_MODE")
                {
                    add = false;
                }
                else
                {
                    s.Add(item);
                }
            }
            if (add) { s.Add("BUNDLE_TEST_MODE"); }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Count; i++)
            {
                if (i != s.Count - 1)
                    sb.Append(s[i]).Append(";");
                else
                    sb.Append(s[i]);
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, sb.ToString());
        }

        public static void BuildBase()
        {
            BuildTarget current;
            Version buildVersion;
            try
            {
                current = (BuildTarget)Enum.Parse(typeof(BuildTarget), EditorPrefs.GetString("build-target"));
                buildVersion = Version.FromString(EditorPrefs.GetString("build-version"));
            }catch(Exception e)
            {
                Debug.LogError(e.Message);
                return;
            }
            
            WriteBaseVersionFile(buildVersion.ToString());
            CollectResources();
            BuildAssetBundle(current);
            CreateBaseFiles(buildVersion, current);
            CopyToStreamingAssets(current);

            if(BuildPlayer(buildVersion.ToString(),current))
                WriteBasePatchesFile(buildVersion.ToString(), current);
            EditorPrefs.DeleteKey("build-target");
            EditorPrefs.DeleteKey("build-version");
            BuildWindow.OpenBuildWindow();
        }

        public static void BuildHotfix()
        {
            BuildTarget current;
            Version buildVersion;
            try
            {
                current = (BuildTarget)Enum.Parse(typeof(BuildTarget), EditorPrefs.GetString("build-target"));
                buildVersion = Version.FromString(EditorPrefs.GetString("build-version"));
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                return;
            }
            CollectResources();
            BuildAssetBundle(current);
            CreateBaseFiles(buildVersion,current);
            CreatePatchesFiles(buildVersion,current);

            WriteHotPatchesFile(buildVersion.ToString(), current);
            BuildWindow.OpenBuildWindow();
        }


        private static void BuildAssetBundle(BuildTarget target)
        {
            string output = ABPathTools.GetAssetBundleOutPath(target);
            if (!Directory.Exists(Config.AssetBundleExportRoot))
            {
                Directory.CreateDirectory(Config.AssetBundleExportRoot);
            }
            if (Directory.Exists(output))
            {
                Directory.Delete(output,true);
            }
            Directory.CreateDirectory(output);
            CopyLuaFiles(target);
            BuildPipeline.BuildAssetBundles(output, BuildAssetBundleOptions.None, target);

            StringBuilder sb = new StringBuilder();
            string[] files = Directory.GetFiles(output, "*.*", SearchOption.AllDirectories);

            foreach (var fullpath in files)
            {
                string md5 = Utility.MD5Stream(fullpath);
                string length = Utility.GetFileLength(fullpath).ToString();
                string relativePath = fullpath.Substring(output.Length+1).Replace("\\","/");
                sb.AppendLine(relativePath + "|" + md5+"|"+ length);
            }
            string filePath = Utility.PathCombile(output, "files.txt");
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(sb.ToString());
                sw.Close();
            }
        }

        private static void WriteBasePatchesFile(string baseVersion,BuildTarget target)
        {
            try
            {
                Patches patches = null;
                string createRoot = Utility.PathCombile(Application.dataPath, "../Patches");
                if (!Directory.Exists(createRoot))
                {
                    Directory.CreateDirectory(createRoot);
                }
                string patchesFile = Utility.PathCombile(createRoot, "patches.txt");
                if (!File.Exists(patchesFile))
                {
                    patches = new Patches();
                }
                else
                {
                    using (FileStream fs = new FileStream(patchesFile, FileMode.Open, FileAccess.Read))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Patches));
                        patches = serializer.Deserialize(fs) as Patches;
                    }
                }
                foreach (var platform in patches.patches)
                {
                    if(platform.platform == ABPathTools.GetAssetBundleOutputDir(target))
                    {
                        platform.patches.Add(new Patch(baseVersion));
                        RealWritePatches(patches);
                        return;
                    }
                }
                var platFormPatch = new PlatformPatch();
                platFormPatch.platform = ABPathTools.GetAssetBundleOutputDir(target);
                platFormPatch.patches.Add(new Patch(baseVersion));
                patches.patches.Add(platFormPatch);

                RealWritePatches(patches);
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message + "\n" + e.StackTrace);
            }
                
        }

        private static void WriteHotPatchesFile(string hotVersion, BuildTarget target)
        {
            try
            {
                Version baseversion = Version.FromString(hotVersion).GetBaseVersion();
                Patches patches = null;
                string createRoot = Utility.PathCombile(Application.dataPath, "../Patches");
                if (!Directory.Exists(createRoot))
                {
                    Directory.CreateDirectory(createRoot);
                }
                string patchesFile = Utility.PathCombile(createRoot, "patches.txt");

                using (FileStream fs = new FileStream(patchesFile, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Patches));
                    patches = serializer.Deserialize(fs) as Patches;
                }

                foreach (var platform in patches.patches)
                {
                    if (platform.platform == ABPathTools.GetAssetBundleOutputDir(target))
                    {
                        foreach (var patch in platform.patches)
                        {
                            if(patch.baseVersion == baseversion.ToString())
                            {
                                patch.hotFix.Add(hotVersion);
                                RealWritePatches(patches);
                                return;
                            }
                        }
                        return;
                    }
                }
                Debug.LogError("写入记录出错");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + "\n" + e.StackTrace);
            }

        }

        private static void RealWritePatches(Patches patches)
        {
            string createRoot = Utility.PathCombile(Application.dataPath, "../Patches");
            if (!Directory.Exists(createRoot))
            {
                Directory.CreateDirectory(createRoot);
            }
            string patchesFile = Utility.PathCombile(createRoot, "patches.txt");
            using (FileStream fs = new FileStream(patchesFile, FileMode.OpenOrCreate, FileAccess.Write))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Patches));
                serializer.Serialize(fs, patches);
            }
        }

        private static void CreatePatchesFiles(Version buildVersion, BuildTarget target)
        {
            string output = ABPathTools.GetAssetBundleOutPath(target);
            string baseFile = Utility.PathCombile(output, "files.txt");
            Version currentVersion = buildVersion.GetBaseVersion();
            string basePath = GetVersionPatchesPath(target, currentVersion);
            if (!File.Exists(basePath))
            {
                Debug.LogError("版本不存在:" + currentVersion);
                return;
            }
            List<Dictionary<string, PatchFileInfo>> patches = new List<Dictionary<string, PatchFileInfo>>();
            while (true)
            {
                if (currentVersion > buildVersion) break;
                string currentPath = GetVersionPatchesPath(target, currentVersion);
                if (!File.Exists(currentPath))
                {
                    Debug.LogError("版本不存在:" + currentVersion);
                    break;
                }
                Dictionary<string, PatchFileInfo> data = new Dictionary<string, PatchFileInfo>();
                using (FileStream fs = new FileStream(currentPath, FileMode.Open, FileAccess.Read))
                {
                    StreamReader sr = new StreamReader(fs);
                    string line = sr.ReadLine();
                    while (line != null)
                    {
                        string[] fileInfo = line.Split('|');
                        data.Add(fileInfo[0], new PatchFileInfo()
                        {
                            name = fileInfo[0],
                            md5 = fileInfo[1],
                            size = fileInfo[2]
                        });
                        line = sr.ReadLine();
                    }
                    sr.Close();
                }
                patches.Add(data);
                currentVersion += 1;
            }
            RealCreatePatches(buildVersion,target,patches);
        }

        private static bool BuildPlayer(string dir, BuildTarget target)
        {
            if (!Directory.Exists(Config.BuildOutputPath)) Directory.CreateDirectory(Config.BuildOutputPath);
            BuildPlayerOptions options = new BuildPlayerOptions();
            options.scenes = GetLevelsFromBuildSettings();
            options.options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
            string buildDir = Utility.PathCombile(Config.BuildOutputPath, dir);
            options.locationPathName = Utility.PathCombile(buildDir, "build"+ABPathTools.GetBuildExtension(target));
            options.target = target;
            BuildPipeline.BuildPlayer(options);

            return File.Exists(options.locationPathName);
        }

        private static string[] GetLevelsFromBuildSettings()
        {
            List<string> levels = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; ++i)
            {
                if (EditorBuildSettings.scenes[i].enabled)
                    levels.Add(EditorBuildSettings.scenes[i].path);
            }

            return levels.ToArray();
        }

        private static void RealCreatePatches(Version buildVersion, BuildTarget buildTarget,List<Dictionary<string,PatchFileInfo>> patches)
        {
            if (patches.Count <= 1) return;
            string output = ABPathTools.GetAssetBundleOutPath(buildTarget);
            Dictionary<string, PatchFileInfo> latest = patches[patches.Count - 1];

            Version baseVersion = buildVersion.GetBaseVersion();

            Dictionary<Version, List<PatchFileInfo>> updateLists = new Dictionary<Version, List<PatchFileInfo>>();
            for (int i = 0; i < patches.Count - 1; i++)
            {
                Version current = baseVersion + i;
                updateLists.Add(current, new List<PatchFileInfo>());
                var target = patches[i];
                foreach (var fileName in latest.Keys)
                {
                    if (!target.ContainsKey(fileName))
                    {
                        updateLists[current].Add(latest[fileName]);
                    }
                    else
                    {
                        if (target[fileName].md5 != latest[fileName].md5)
                        {
                            updateLists[current].Add(latest[fileName]);
                        }
                    }
                }
            }
            
            string updateExportPath = Utility.PathCombile(Config.UpdateListExportPath, ABPathTools.GetAssetBundleOutputDir(buildTarget));
            Utility.CreateDirectoryEmpty(updateExportPath);
            string updateTmpPath = Utility.PathCombile(updateExportPath, "temp");
            foreach (var item in updateLists)
            {
                Debug.Log(item.Key);
                if (!Directory.Exists(updateTmpPath))
                {
                    Directory.CreateDirectory(updateTmpPath);
                }
                var current = Utility.PathCombile(updateExportPath, item.Key.ToString());
                if (!Directory.Exists(current))
                {
                    Directory.CreateDirectory(current);
                }
                var currentFile = Utility.PathCombile(current, "update.txt");
                var resZip = Utility.PathCombile(current, "res.zip");
                using (FileStream fs = new FileStream(currentFile, FileMode.Create, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    
                    foreach (var file in item.Value)
                    {
                        string copyPath = Utility.PathCombile(output, file.name);
                        string desPath = Utility.PathCombile(updateTmpPath, file.name);
                        string dir = Path.GetDirectoryName(desPath);
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                        Utility.CopyFile(copyPath, desPath);
                        sw.WriteLine(file.ToString());
                        Debug.Log(desPath);
                    }
                    sw.Close();
                }
                Utility.CopyDirectory(updateTmpPath, current);
                //EasyZip zip = new EasyZip();
                //zip.Zip(resZip, updateTmpPath);
                //zip.Unzip(resZip, current);
                //File.Delete(resZip);
                Directory.Delete(updateTmpPath,true);
            }
        }

        private static void CreateBaseFiles(Version buildVersion, BuildTarget target)
        {
            string output = ABPathTools.GetAssetBundleOutPath(target);
            string baseFile = Utility.PathCombile(output, "files.txt");
            string createRoot = Utility.PathCombile(Application.dataPath, "../Patches");
            if (!Directory.Exists(createRoot))
            {
                Directory.CreateDirectory(createRoot);
            }
            string createPath = Utility.PathCombile(createRoot, ABPathTools.GetAssetBundleOutputDir(target));
            if (!Directory.Exists(createPath))
            {
                Directory.CreateDirectory(createPath);
            }
            Utility.CopyFile(baseFile, Utility.PathCombile(createPath, buildVersion.ToString()));
        }

        private static string GetVersionPatchesPath(BuildTarget target,Version version)
        {
            return Utility.PathCombile(Application.dataPath,
                "../Patches", ABPathTools.GetAssetBundleOutputDir(target),
                version.ToString());
        }

        private static void CopyToStreamingAssets(BuildTarget target)
        {
            string output = ABPathTools.GetAssetBundleOutPath(target);
            if (Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.Delete(Application.streamingAssetsPath, true);
            }
            if (!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }
            Utility.CopyDirectory(output, Application.streamingAssetsPath);
            //EasyZip zip = new EasyZip();
            //var zipFile = Utility.PathCombile(Application.streamingAssetsPath, Config.ZipName);
            //zip.Zip(zipFile, output);
            //zip.Unzip(zipFile, Application.streamingAssetsPath);

            //File.Delete(zipFile);

            AssetDatabase.Refresh();
        }

        private static void WriteBaseVersionFile(string version)
        {
            try
            {
                using (FileStream fs = new FileStream(Utility.PathCombile(Application.dataPath, "Resources/Version.txt")
                    , FileMode.OpenOrCreate, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(version);
                    sw.Close();
                    AssetDatabase.Refresh();
                }
                using (FileStream fs = new FileStream(Utility.PathCombile(Application.dataPath, "Resources/Verify.txt")
                    , FileMode.OpenOrCreate, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(DateTime.UtcNow.Ticks);
                    sw.Close();
                    AssetDatabase.Refresh();
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
                //return false;
            }
            //return true;
        }

        //private static string GetBuildVersion(bool checkBaseVersion)
        //{
        //    if (!File.Exists(Config.BuildVersionPath)) throw new Exception("找不到版本文件");
        //    string version = File.ReadAllText(Config.BuildVersionPath);
        //    return version;
        //    if (checkBaseVersion)
        //    {
        //        if (!Version.IsBaseVersion(version)) throw new Exception("版本文件不对" + version);
        //        return version;
        //    }
        //    else
        //    {
        //        if (Version.IsBaseVersion(version)) throw new Exception("版本文件不对" + version);
        //        return version;
        //    }
        //}

        private static void CollectResources()
        {
            string root = Utility.PathCombile(Application.dataPath,Config.AssetBundleEditorRoot);
            if (!Directory.Exists(root)) return;

            DirectoryInfo rootInfo = new DirectoryInfo(root);
            foreach (var file in rootInfo.GetFiles())
            {
                SetFileABLabelAbsolute(file);
            }

            DirectoryInfo[] allScenes = rootInfo.GetDirectories();
            foreach (var sceneDir in allScenes)
            {
                var files = sceneDir.GetFiles();
                var dirs = sceneDir.GetDirectories();
                int index = 0;
                foreach (var typeDir in dirs)
                {
                    ShowProgress("正在收集资源:", index++, files.Length + dirs.Length);
                    _RealSetABLabel(typeDir, sceneDir.Name + "/" + typeDir.Name);
                }
                
                foreach (var file in files)
                {
                    ShowProgress("正在收集资源:", index++, files.Length + dirs.Length);
                    SetFileABLabel(file, sceneDir.Name);
                }
            }
            EditorUtility.ClearProgressBar();
            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void _RealSetABLabel(FileSystemInfo scene, string name)
        {
            DirectoryInfo dirInfos = scene as DirectoryInfo;
            FileSystemInfo[] fileInfos = dirInfos.GetFileSystemInfos();
            foreach (FileSystemInfo info in fileInfos)
            {
                FileInfo file = info as FileInfo;
                if (file.Extension.Equals(".png")
                    || file.Extension.Equals(".jpg")
                    || file.Extension.Equals(".jpeg")) continue;
                if (file != null)
                {
                    //是文件，设置标记
                    SetFileABLabel(file, name);
                }
                else
                {
                    //是目录，递归调用
                    _RealSetABLabel(info, name);
                }
            }
        }

        private static void SetFileABLabel(FileInfo file, string abname)
        {
            int resPathIndex = file.FullName.IndexOf("Assets");
            string resPath = file.FullName.Substring(resPathIndex);
            resPath = resPath.Replace('\\', '/');

            if (Path.GetExtension(resPath) == ".meta")
            {
                return;
            }

            AssetImporter importer = AssetImporter.GetAtPath(resPath);
            importer.assetBundleName = Utility.PathCombile(Config.GameName,abname);
            if (file.Extension == ".unity")
            {
                importer.assetBundleVariant = Config.AssetBundleSceneSuffix;
            }
            else
            {
                importer.assetBundleVariant = Config.AssetBundleSuffix;
            }
        }

        private static void SetFileABLabelAbsolute(FileInfo file)
        {
            int resPathIndex = file.FullName.IndexOf("Assets");
            string resPath = file.FullName.Substring(resPathIndex);
            resPath = resPath.Replace('\\', '/');

            if (Path.GetExtension(resPath) == ".meta")
            {
                return;
            }

            AssetImporter importer = AssetImporter.GetAtPath(resPath);
            importer.assetBundleName = Config.GameName;
            if (file.Extension == ".unity")
            {
                importer.assetBundleVariant = Config.AssetBundleSceneSuffix;
            }
            else
            {
                importer.assetBundleVariant = Config.AssetBundleSuffix;
            }
        }

        private static void CopyLuaFiles(BuildTarget target)
        {
            string path = Utility.PathCombile(Application.dataPath, Config.LuaSource);
            if (!Directory.Exists(path)) return;
            string output = ABPathTools.GetAssetBundleOutPath(target);
            string[] files = Directory.GetFiles(path, "*.lua", SearchOption.AllDirectories);
            int index = 0;
            foreach (var file in files)
            {
                string targetPath = Utility.PathCombile(output,
                    file.Substring(Application.dataPath.Length + 1));
                targetPath = Path.ChangeExtension(targetPath, "bytes");
                Debug.Log(targetPath);
                ShowProgress("正在拷贝文件:" + file, index++, files.Length);
                Utility.CopyFile(file, targetPath);
            }

            EditorUtility.ClearProgressBar();
        }

        private static void ShowProgress(string info,int current,int total)
        {
            EditorUtility.DisplayProgressBar("Waiting...", info, (float)current / total);
        }
    }
}
