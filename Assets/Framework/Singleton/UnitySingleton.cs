using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YummyGame.Framework
{
    public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = GameObject.Find("Yummy");
                    if (go == null)
                    {
                        go = new GameObject("Yummy");
                        DontDestroyOnLoad(go);
                    }
                    _instance = go.GetOrAddComponent<T>();
                }
                return _instance;
            }
        }
    }
}
