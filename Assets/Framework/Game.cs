using System;
using System.Collections.Generic;

#if XLUA_SUPPORT
using XLua;
#endif

namespace YummyGame.Framework
{
    public static class Game
    {
        public static TaskLooper GlobalLooper = Facade.Instance.taskLooper;
        public static AssetLoader GlobalLoader = Facade.Instance.loader;
        public static AssetLoader SceneLoader = SceneManager.Instance.Loader;
#if XLUA_SUPPORT
        public static LuaEnv Lua = AssetManager.Instance.lua;
#endif
        public static ITask ChangeScene(string sceneName)
        {
            return SceneManager.Instance.LoadScene(sceneName);
        }

        public static void Initalize(AssetManager.AssetInitializeComplete success,
            AssetManager.AssetErrorCallback error = null)
        {
            var ui = UIManager.Instance;
            AssetManager.Instance.Initalize(success, error);
        }

        public static YummyTask<long> OpenUI<T>(string layer)where T : UIWindow
        {
            return UIManager.Instance.OpenUI<T>(layer);
        }

        public static void CloseUI(long uuid,bool destroy = false)
        {
            UIManager.Instance.CloseUI(uuid, destroy);
        }
    }
}
