using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace YummyGame.Framework
{
    public partial class ABPathTools
    {
        public static string GetRuntimePlatformName(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "standalone";
                case RuntimePlatform.IPhonePlayer:
                    return "ios";
                case RuntimePlatform.Android:
                    return "android";
                default:
                    return "standalone";
            }
        }

        public static string GetObjectSuffix(Type type)
        {
            if(type == typeof(GameObject))
            {
                return ".prefab";
            }else if(type == typeof(Material))
            {
                return ".mat";
            }else if(type == typeof(Shader))
            {
                return ".shader";
            }else if(type == typeof(TextAsset))
            {
                return ".txt";
            }
            return "";
        }

        public static string[] SplitResPath(string path,Type type)
        {
            string fullPath = Utility.PathCombile(Config.GameName, path);
            string[] splits = new string[2];
            int lastIndex = fullPath.LastIndexOf("/");
            if (type == typeof(Scene))
            {
                splits[0] = fullPath.Substring(0, lastIndex) + "." + Config.AssetBundleSceneSuffix;
            }
            else
            {
                splits[0] = fullPath.Substring(0, lastIndex) + "." + Config.AssetBundleSuffix;
            }
            splits[0] = splits[0].ToLower();
            splits[1] = fullPath.Substring(lastIndex + 1) + GetObjectSuffix(type);
            return splits;
        }

    }
}
