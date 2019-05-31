using System;
using System.Collections.Generic;
using UnityEngine;
#if XLUA_SUPPORT
using XLua;
#endif

namespace YummyGame.Framework
{
    public class Facade:UnitySingleton<Facade>
    {
        public TaskLooper taskLooper;
        public AssetLoader loader;

#if XLUA_SUPPORT
        public LuaEnv Lua;
#endif

        private void Awake()
        {
            taskLooper = gameObject.AddComponent<TaskLooper>();
            loader = new AssetLoader();

#if UNITY_STANDALONE || UNITY_EDITOR
            Application.logMessageReceived+=LogErrorDebug;
#endif
        }

        private void OnDestroy()
        {
            loader.Destroy();
        }

#if UNITY_STANDALONE || UNITY_EDITOR
        private void LogErrorDebug(string condition, string stackTrace, LogType type)
        {
            if(type == LogType.Error)
            {
                DebugView.Log("ERROR " + condition + "\n" + stackTrace + "\n");
            }
        }
#endif
    }
}
