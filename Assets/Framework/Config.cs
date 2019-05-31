using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace YummyGame.Framework
{
    public static class Config
    {
        //游戏名字
        public const string GameName = "game";

        //Assetbundle打包的根路径
        public const string AssetBundleEditorRoot = "yummy";

        //Lua打包的根路径
        public const string LuaSource = "lua";

        //Assetbundle后缀
        public const string AssetBundleSuffix = "ab";

        //Assetbundle场景后缀
        public const string AssetBundleSceneSuffix = "u3d";

        //资源压缩包名字
        public const string ZipName = "res.zip";

        //是否使用AssetBundle打包 false就是Resources打包
        public const bool UseAssetBundle = true;

        //资源更新地址
        public const string AssetUpdateURI = "http://47.100.121.103/update";

        //资源版本号地址
        public const string AssetVersionURI = "http://47.100.121.103/latest.txt";

        //熱更包的導出路徑
        public const string UpdateListExportPath = "D:/update";

        //资源根路径
        public static string AssetRoot
        {
            get
            {
                return Utility.PathCombile(Application.persistentDataPath, "res");
            }
        }

        //AssetBundle包导出路径根
        public static string AssetBundleExportRoot
        {
            get
            {
                return Utility.PathCombile(Application.dataPath, "../AssetBundles");
            }
        }

        //打整包的版本号读取地址
        public static string BuildVersionPath
        {
            get
            {
                return Utility.PathCombile(Application.dataPath, "../Version.txt");
            }
        }

        //打整包的版本号存储地址(必须在Resources目录且是Version名字)
        public static string BuildVersionStorePath
        {
            get
            {
                return Utility.PathCombile(Application.dataPath, "Resources/Version.txt");
            }
        }

        //资源初始化标记
        public static string AssetVersionFlag {
            get
            {
#if CHECK_UPDATE_EDITOR || !UNITY_EDITOR
                TextAsset ta = Resources.Load<TextAsset>("Verify");
                return "client_current_version"+ta.text;
#else
                return "client_current_version";
#endif
            }
        }

        //资源历史版本记录
        public static string AssetHistoryVersionsFlag {
            get
            {
#if CHECK_UPDATE_EDITOR || !UNITY_EDITOR
                TextAsset ta = Resources.Load<TextAsset>("Verify");
                return "client_versions" + ta.text;
#else
                return "client_versions";                
#endif
            }
        }

        public static string BuildOutputPath
        {
            get
            {
#if UNITY_EDITOR
                return Utility.PathCombile(Application.dataPath, "../builds");
#else
                return "";
#endif
            }
        }
    }
}
