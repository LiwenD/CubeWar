#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace YummyGame.Framework
{
    public partial class ABPathTools
    {
        public static string GetAssetBundleOutputDir(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "standalone";
                case BuildTarget.iOS:
                    return "ios";
                case BuildTarget.Android:
                    return "android";
                default:
                    return "standalone";
            }
        }

        public static string GetAssetBundleOutPath(BuildTarget target)
        {
            return Utility.PathCombile(Config.AssetBundleExportRoot, GetAssetBundleOutputDir(target));
        }

        public static string GetBuildExtension(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return ".exe";
                case BuildTarget.iOS:
                    return ".ipa";
                case BuildTarget.Android:
                    return ".apk";
                default:
                    return ".exe";
            }
        }

        public static BuildTarget ReGetBuildTarget(string platform)
        {
            switch (platform)
            {
                case "standalone":
                    return BuildTarget.StandaloneWindows64;
                case "ios":
                    return BuildTarget.iOS;
                case "android":
                    return BuildTarget.Android;
                default:
                    return BuildTarget.StandaloneWindows64;
            }
        }
    }
}

#endif
