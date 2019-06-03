using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml.Serialization;

namespace YummyGame.Framework.Editor
{
    public class BuildVersionInfo
    {
        public string baseVersion;
        public string latestVersion;
    }
    public class BuildWindow:EditorWindow
    {
        private string nextBaseVersion;
        public Patches patches;
        private Vector2 scroll;
        private Dictionary<string,List<BuildVersionInfo>> versionsInfo = new Dictionary<string,List<BuildVersionInfo>>();
        private List<bool> folders = new List<bool>();
        private void OnEnable()
        {
            LoadPatches();
        }

        public void LoadPatches()
        {
            versionsInfo.Clear();
            folders.Clear();
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
            if (patches.patches.Count == 0) nextBaseVersion = Version.zero.ToString();
            bool nextFind = false;
            foreach (var platform in patches.patches)
            {
                folders.Add(true);
                versionsInfo.Add(platform.platform, new List<BuildVersionInfo>());
                foreach (var patch in platform.patches)
                {
                    BuildVersionInfo v = new BuildVersionInfo();
                    v.baseVersion = patch.baseVersion;
                    Version currentBase = Version.FromString(v.baseVersion);
                    if (nextFind == false)
                    {
                        nextFind = true;
                        nextBaseVersion = currentBase.AddRevision(1).ToString();
                    }
                    else
                    {
                        if (currentBase >= Version.FromString(nextBaseVersion))
                        {
                            nextBaseVersion = currentBase.AddRevision(1).ToString();
                        }
                    }
                    Version version = Version.FromString(v.baseVersion);
                    if (patch.hotFix.Count == 0)
                    {
                        v.latestVersion = (version + 1).ToString();
                    }
                    else
                    {
                        bool init = false;
                        Version max = null;
                        foreach (var hotfix in patch.hotFix)
                        {
                            Version current = Version.FromString(hotfix);
                            if (init == false)
                            {
                                max = current;
                                init = true;
                            }
                            else max = current;
                            v.latestVersion = (max + 1).ToString();
                        }
                    }
                    versionsInfo[platform.platform].Add(v);
                }
                
            }
        }

        private void BaseInfo()
        {
            nextBaseVersion = EditorGUILayout.TextField("基准包:", nextBaseVersion);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("打包Windows"))
            {
                if (Version.IsBaseVersion(nextBaseVersion))
                {
                    BuildBasePackage(nextBaseVersion, BuildTarget.StandaloneWindows64);
                }

            }
            if (GUILayout.Button("打包Android"))
            {
                if (Version.IsBaseVersion(nextBaseVersion))
                {
                    BuildBasePackage(nextBaseVersion, BuildTarget.Android);
                }
            }
            if (GUILayout.Button("打包IOS"))
            {
                if (Version.IsBaseVersion(nextBaseVersion))
                {
                    BuildBasePackage(nextBaseVersion, BuildTarget.iOS);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnGUI()
        {
            try
            {
                BaseInfo();
                int foldIndex = 0;
                scroll = EditorGUILayout.BeginScrollView(scroll);
                foreach (var info in versionsInfo)
                {
                    folders[foldIndex] = EditorGUILayout.Foldout(folders[foldIndex], info.Key);
                    if (folders[foldIndex])
                    {
                        foreach (var version in info.Value)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("基准包" + version.baseVersion);
                            EditorGUILayout.LabelField("热更版本" + version.latestVersion);
                            if (GUILayout.Button("打热更包"))
                            {
                                BuildHotFixPackage(version.latestVersion, info.Key);
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }

                    foldIndex++;
                }
                EditorGUILayout.EndScrollView();
            }
            catch
            {
                LoadPatches();
            }
            
        }

        private void BuildBasePackage(string version,BuildTarget target)
        {
            if (!Version.IsBaseVersion(version)) return;
            EditorPrefs.SetString("build-version", version);
            EditorPrefs.SetString("build-target", target.ToString());
            Close();
            BuildScript.BuildBase();
        }

        private void BuildHotFixPackage(string version, string platform)
        {
            EditorPrefs.SetString("build-version", version);
            BuildTarget target = ABPathTools.ReGetBuildTarget(platform);
            EditorPrefs.SetString("build-target", target.ToString());
            Close();
            BuildScript.BuildHotfix();
        }

        [MenuItem("Yummy/窗体/打包")]
        public static void OpenBuildWindow()
        {
            BuildWindow window = GetWindow<BuildWindow>();
            window.Show();
        }
    }
}
