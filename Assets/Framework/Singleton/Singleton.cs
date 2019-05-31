using System;
using System.Collections.Generic;

namespace YummyGame.Framework
{
    public class Singleton<T> where T:class
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = Activator.CreateInstance<T>();
                }
                return _instance;
            }
        }
    }
}
