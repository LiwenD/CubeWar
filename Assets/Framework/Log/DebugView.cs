using System;
using System.Collections.Generic;
#if UNITY_STANDALONE || UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
namespace YummyGame.Framework
{
    public static class DebugView
    {
#if UNITY_STANDALONE || UNITY_EDITOR
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern void OutputDebugString(string message);
#endif
        static string flag = "yummy:";
        public static void Log(string msg)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            OutputDebugString(flag + msg + "\n");
#endif
        }
    }
}
